using CsvParser.ErrorHandling.Interfaces;
using CsvParser.Shared.Constants;
using CsvParser.Validation.Models;

namespace CsvParser.ErrorHandling.Implementations;

/// <summary>
/// Concrete implementation of IErrorHandler for recording and managing parsing errors.
/// Provides detailed error tracking and reporting capabilities.
/// </summary>
public class CsvErrorHandler : IErrorHandler
{
    private readonly List<ParsingError> _errors = [];

    public void RecordValidationError(ValidationResult validationResult)
    {
        ArgumentNullException.ThrowIfNull(validationResult);

        foreach (var errorMessage in validationResult.Errors)
        {
            _errors.Add(new ParsingError(
                validationResult.RowNumber,
                validationResult.ColumnNumber,
                errorMessage,
                ErrorType.Validation)
            {
                Timestamp = DateTime.UtcNow
            });
        }
    }

    public void RecordParsingError(int rowNumber, int? columnNumber, string errorMessage, string? expectedValue = null, string? actualValue = null)
    {
        if (string.IsNullOrWhiteSpace(errorMessage))
        {
            return;
        }

        _errors.Add(new ParsingError(rowNumber, columnNumber, errorMessage, ErrorType.Parsing)
        {
            ExpectedValue = expectedValue,
            ActualValue = actualValue,
            Timestamp = DateTime.UtcNow
        });
    }

    public void RecordException(int rowNumber, Exception exception, string context)
    {
        ArgumentNullException.ThrowIfNull(exception);

        _errors.Add(new ParsingError(
            rowNumber,
            null,
            string.Format(ErrorMessages.ExceptionMessage, context, exception.Message),
            ErrorType.Exception)
        {
            Context = context,
            Timestamp = DateTime.UtcNow
        });
    }

    public void RecordTransformationError(int rowNumber, string fieldName, string originalValue, string errorMessage)
    {
        if (string.IsNullOrWhiteSpace(errorMessage))
        {
            return;
        }

        _errors.Add(new ParsingError(rowNumber, null, errorMessage, ErrorType.Transformation)
        {
            FieldName = fieldName,
            ActualValue = originalValue,
            Timestamp = DateTime.UtcNow
        });
    }

    public List<ParsingError> GetAllErrors()
    {
        return [.. _errors];
    }

    public List<ParsingError> GetErrorsForRow(int rowNumber)
    {
        return [.. _errors.Where(e => e.RowNumber == rowNumber)];
    }

    public bool HasErrors()
    {
        return _errors.Count > 0;
    }

    public int GetErrorCount()
    {
        return _errors.Count;
    }

    public void ClearErrors()
    {
        _errors.Clear();
    }

    /// <summary>
    /// Gets a summary of errors grouped by type
    /// </summary>
    /// <returns>Dictionary with error types as keys and counts as values</returns>
    public Dictionary<ErrorType, int> GetErrorSummary()
    {
        return _errors.GroupBy(e => e.ErrorType)
            .ToDictionary(g => g.Key, g => g.Count());
    }

    /// <summary>
    /// Gets errors grouped by row number
    /// </summary>
    /// <returns>Dictionary with row numbers as keys and error lists as values</returns>
    public Dictionary<int, List<ParsingError>> GetErrorsByRow()
    {
        return _errors.GroupBy(e => e.RowNumber)
            .ToDictionary(g => g.Key, g => g.ToList());
    }

    /// <summary>
    /// Exports errors to a formatted string for reporting
    /// </summary>
    /// <returns>Formatted error report</returns>
    public string ExportErrorReport()
    {
        if (_errors.Count == 0)
        {
            return ErrorMessages.NoErrors;
        }

        var report = new System.Text.StringBuilder();
        report.AppendLine(string.Format(ErrorMessages.ErrorReportTitle, DateTime.UtcNow));
        report.AppendLine(string.Format(ErrorMessages.TotalErrors, _errors.Count));
        report.AppendLine();

        // Group by error type
        var errorsByType = _errors.GroupBy(e => e.ErrorType);
        foreach (var group in errorsByType)
        {
            report.AppendLine(string.Format(ErrorMessages.ErrorTypeSummary, group.Key, group.Count()));
            foreach (var error in group)
            {
                report.AppendLine(string.Format(ErrorMessages.ErrorDetail, error));
            }

            report.AppendLine();
        }

        return report.ToString();
    }
}