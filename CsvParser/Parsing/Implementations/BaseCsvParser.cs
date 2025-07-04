using CsvParser.ErrorHandling.Interfaces;
using CsvParser.Parsing.Interfaces;
using CsvParser.Shared.Constants;
using CsvParser.Transformation.Interfaces;
using CsvParser.Validation.Interfaces;
using CsvParser.Validation.Models;

namespace CsvParser.Parsing.Implementations;

/// <summary>
/// Abstract base class for CSV parsers that provides common functionality
/// and implements the core parsing logic using dependency injection.
/// </summary>
/// <typeparam name="T">The type of DTO to parse into</typeparam>
public abstract class BaseCsvParser<T>(
    IValidator<T> validator,
    ITransformer<T> transformer,
    IErrorHandler errorHandler)
    : ICsvParser<T> where T : class
{
    protected IValidator<T> Validator { get; set; } = validator;

    protected ITransformer<T> Transformer { get; set; } = transformer;

    protected IErrorHandler ErrorHandler { get; set; } = errorHandler;

    /// <summary>
    /// Main parsing method that orchestrates the parsing process
    /// </summary>
    public virtual async Task<List<T>> ParseAsync(string filePath)
    {
        try
        {
            // Validate file structure first
            if (!await ValidateFileStructureAsync(filePath))
            {
                ErrorHandler.RecordParsingError(Shared.Constants.ValidationConstants.HeaderRowIndex, null, string.Format(ErrorMessages.FileStructureIncompatible, GetParserType()));
                return [];
            }

            // Read CSV file lines
            var lines = await File.ReadAllLinesAsync(filePath);
            if (lines.Length < Shared.Constants.ValidationConstants.MinimumFileLines)
            {
                ErrorHandler.RecordParsingError(Shared.Constants.ValidationConstants.HeaderRowIndex, null, ErrorMessages.FileTooShort);
                return [];
            }

            // Parse header row
            var headers = ParseHeaderRow(lines[Shared.Constants.ValidationConstants.HeaderRowIndex]);

            // Parse data rows
            var dtos = new List<T>();
            for (int i = Shared.Constants.ValidationConstants.DataRowStartIndex; i < lines.Length; i++)
            {
                try
                {
                    var dto = await ParseDataRowAsync(lines[i], headers, i + 1);
                    if (dto != null)
                    {
                        dtos.Add(dto);
                    }
                }
                catch (Exception ex)
                {
                    ErrorHandler.RecordException(i + 1, ex, string.Format(ErrorMessages.ErrorParsingRow, i + 1));
                }
            }

            return dtos;
        }
        catch (Exception ex)
        {
            ErrorHandler.RecordException(Shared.Constants.ValidationConstants.HeaderRowIndex, ex, ErrorMessages.ErrorDuringFileParsing);
            return [];
        }
    }

    /// <summary>
    /// Abstract method that must be implemented by derived classes to validate file structure
    /// </summary>
    public abstract Task<bool> ValidateFileStructureAsync(string filePath);

    /// <summary>
    /// Abstract method that must be implemented by derived classes to return parser type
    /// </summary>
    public abstract string GetParserType();

    /// <summary>
    /// Parses the header row to extract column names
    /// </summary>
    protected virtual string[] ParseHeaderRow(string headerLine)
    {
        return [.. headerLine.Split(CsvParsingConstants.Separator).Select(h => h.Trim())];
    }

    protected virtual bool ValidateHeaders(string[] headers, string[] expectedHeaders)
    {
        return headers.Length > 0
                && expectedHeaders.Length > 0
                && headers.Length >= expectedHeaders.Length
                && expectedHeaders.All(h => headers.Contains(h, StringComparer.OrdinalIgnoreCase));
    }

    /// <summary>
    /// Parses a single data row and converts it to a DTO
    /// </summary>
    protected virtual async Task<T?> ParseDataRowAsync(string dataLine, string[] headers, int rowNumber)
    {
        try
        {
            // Split the data line into cells
            var cells = ParseDataCells(dataLine);

            // Create a dictionary mapping headers to cell values
            var rowData = new Dictionary<string, string>();
            for (int i = 0; i < Math.Min(headers.Length, cells.Length); i++)
            {
                rowData[headers[i]] = cells[i];
            }

            // Validate the row data
            var validationResult = await ValidateRowDataAsync(rowData, rowNumber);
            if (!validationResult.IsValid)
            {
                ErrorHandler.RecordValidationError(validationResult);
                return null;
            }

            // Transform the row data to DTO
            var dto = Transformer.TransformToDto(rowData);

            // Enrich with external data if needed
            dto = await Transformer.EnrichWithExternalDataAsync(dto);

            // Final validation of the DTO
            var finalValidation = Validator.ValidateDto(dto, rowNumber);
            if (!finalValidation.IsValid)
            {
                ErrorHandler.RecordValidationError(finalValidation);
                return null;
            }

            return dto;
        }
        catch (Exception ex)
        {
            ErrorHandler.RecordException(rowNumber, ex, string.Format(ErrorMessages.ErrorParsingDataRow, rowNumber));
            return null;
        }
    }

    /// <summary>
    /// Parses data cells from a CSV line, handling quoted values
    /// </summary>
    protected virtual string[] ParseDataCells(string dataLine)
    {
        // Simple CSV parsing - in a real implementation, you might use a library like CsvHelper
        return [.. dataLine.Split(CsvParsingConstants.Separator).Select(cell => cell.Trim().Trim(CsvParsingConstants.QuoteChar))];
    }

    /// <summary>
    /// Validates row data before transformation
    /// </summary>
    protected virtual Task<ValidationResult> ValidateRowDataAsync(Dictionary<string, string> rowData, int rowNumber)
    {
        var result = new ValidationResult(true, rowNumber);
        // Validate each cell individually
        foreach (var kvp in rowData)
        {
            var cellValidation = Validator.ValidateSingleCell(kvp.Value, kvp.Key, rowNumber, 0);
            if (!cellValidation.IsValid)
            {
                result.AddError(string.Format(ErrorMessages.ColumnValidationError, kvp.Key, string.Join(", ", cellValidation.Errors)));
            }
        }

        // Validate related cells together
        var multiCellValidation = Validator.ValidateMultiCell(rowData, rowNumber);
        if (!multiCellValidation.IsValid)
        {
            foreach (var error in multiCellValidation.Errors)
            {
                result.AddError(error);
            }
        }

        return Task.FromResult(result);
    }
}