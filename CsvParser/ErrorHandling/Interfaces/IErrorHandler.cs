using CsvParser.ErrorHandling.Models;
using CsvParser.Validation.Models;

namespace CsvParser.ErrorHandling.Interfaces;

/// <summary>
/// Interface for handling and recording errors during CSV parsing operations.
/// Provides detailed error tracking and reporting capabilities.
/// </summary>
public interface IErrorHandler
{
    /// <summary>
    /// Records a validation error with detailed context
    /// </summary>
    /// <param name="validationResult">The validation result containing error details</param>
    void RecordValidationError(ValidationResult validationResult);

    /// <summary>
    /// Records a parsing error with specific details
    /// </summary>
    /// <param name="rowNumber">Row number where the error occurred</param>
    /// <param name="columnNumber">Column number where the error occurred (optional)</param>
    /// <param name="errorMessage">Description of the error</param>
    /// <param name="expectedValue">Expected value (optional)</param>
    /// <param name="actualValue">Actual value that caused the error (optional)</param>
    void RecordParsingError(int rowNumber, int? columnNumber, string errorMessage, string? expectedValue = null, string? actualValue = null);

    /// <summary>
    /// Records an exception that occurred during parsing
    /// </summary>
    /// <param name="rowNumber">Row number where the exception occurred</param>
    /// <param name="exception">The exception that was thrown</param>
    /// <param name="context">Additional context about where the exception occurred</param>
    void RecordException(int rowNumber, Exception exception, string context);

    /// <summary>
    /// Records a transformation error
    /// </summary>
    /// <param name="rowNumber">Row number where the transformation failed</param>
    /// <param name="fieldName">Name of the field that failed transformation</param>
    /// <param name="originalValue">Original value that couldn't be transformed</param>
    /// <param name="errorMessage">Description of the transformation error</param>
    void RecordTransformationError(int rowNumber, string fieldName, string originalValue, string errorMessage);

    /// <summary>
    /// Gets all recorded errors for reporting
    /// </summary>
    /// <returns>Collection of all recorded errors</returns>
    List<ParsingError> GetAllErrors();

    /// <summary>
    /// Gets errors for a specific row
    /// </summary>
    /// <param name="rowNumber">Row number to get errors for</param>
    /// <returns>Collection of errors for the specified row</returns>
    List<ParsingError> GetErrorsForRow(int rowNumber);

    /// <summary>
    /// Checks if any errors have been recorded
    /// </summary>
    /// <returns>True if errors exist, false otherwise</returns>
    bool HasErrors();

    /// <summary>
    /// Gets the total count of errors recorded
    /// </summary>
    /// <returns>Total number of errors</returns>
    int GetErrorCount();

    /// <summary>
    /// Clears all recorded errors
    /// </summary>
    void ClearErrors();

    /// <summary>
    /// Gets a summary of errors grouped by type
    /// </summary>
    /// <returns>Dictionary with error types as keys and counts as values</returns>
    Dictionary<ErrorType, int> GetErrorSummary();

    /// <summary>
    /// Gets errors grouped by row number
    /// </summary>
    /// <returns>Dictionary with row numbers as keys and error lists as values</returns>
    Dictionary<int, List<ParsingError>> GetErrorsByRow();

    /// <summary>
    /// Exports errors to a formatted string for reporting
    /// </summary>
    /// <returns>Formatted error report</returns>
    string ExportErrorReport();
}