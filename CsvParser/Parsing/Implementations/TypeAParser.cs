using CsvParser.ErrorHandling.Interfaces;
using CsvParser.Models;
using CsvParser.Shared.Constants;
using CsvParser.Transformation.Interfaces;
using CsvParser.Validation.Interfaces;
using CsvParser.Validation.Models;

namespace CsvParser.Parsing.Implementations;

/// <summary>
/// Parser for Type A CSV format:
/// CustomerID, Full Name, Email, Phone, Salary
/// </summary>
public class TypeAParser : BaseCsvParser<CustomerDto>
{
    private readonly string[] _expectedHeaders = [FieldNames.Ids.CustomerId, FieldNames.Names.FullName, FieldNames.Emails.Email, FieldNames.Phones.Phone, FieldNames.Salaries.Salary];

    public TypeAParser(IValidator<CustomerDto> validator, ITransformer<CustomerDto> transformer, IErrorHandler errorHandler)
        : base(validator, transformer, errorHandler)
    {
    }

    public override string GetParserType() => ParserTypes.TypeA;

    public override async Task<bool> ValidateFileStructureAsync(string filePath)
    {
        try
        {
            var lines = await File.ReadAllLinesAsync(filePath);
            if (lines.Length == 0)
            {
                return false;
            }

            var headers = ParseHeaderRow(lines[0]);

            // Check if all expected headers are present
            foreach (var expectedHeader in _expectedHeaders)
            {
                if (!headers.Contains(expectedHeader, StringComparer.OrdinalIgnoreCase))
                {
                    return false;
                }
            }

            return true;
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// Override to provide TypeA-specific validation logic
    /// </summary>
    protected override async Task<ValidationResult> ValidateRowDataAsync(Dictionary<string, string> rowData, int rowNumber)
    {
        var result = await base.ValidateRowDataAsync(rowData, rowNumber);

        // TypeA-specific validations
        if (rowData.TryGetValue(FieldNames.Names.FullName, out var fullName))
        {
            if (!string.IsNullOrWhiteSpace(fullName))
            {
                var nameParts = fullName.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                if (nameParts.Length < CsvParser.Shared.Constants.ValidationConstants.MinimumNameParts)
                {
                    result.AddError(string.Format(ErrorMessages.FullNameFormat, rowNumber));
                }
            }
        }

        return result;
    }
}