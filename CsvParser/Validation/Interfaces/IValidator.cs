using CsvParser.Validation.Models;

namespace CsvParser.Validation.Interfaces;

/// <summary>
/// Interface for validating data during CSV parsing operations.
/// Supports both single-cell and multi-cell validation scenarios.
/// </summary>
/// <typeparam name="T">The type of DTO to validate</typeparam>
public interface IValidator<T> where T : class
{
    /// <summary>
    /// Validates a single cell value against specified rules
    /// </summary>
    /// <param name="value">The cell value to validate</param>
    /// <param name="fieldName">Name of the field being validated</param>
    /// <param name="rowNumber">Row number for error reporting</param>
    /// <param name="columnNumber">Column number for error reporting</param>
    /// <returns>Validation result indicating success/failure and any error messages</returns>
    ValidationResult ValidateSingleCell(string value, string fieldName, int rowNumber, int columnNumber);

    /// <summary>
    /// Validates multiple related cell values together
    /// </summary>
    /// <param name="values">Dictionary of field names and their corresponding values</param>
    /// <param name="rowNumber">Row number for error reporting</param>
    /// <returns>Validation result indicating success/failure and any error messages</returns>
    ValidationResult ValidateMultiCell(Dictionary<string, string> values, int rowNumber);

    /// <summary>
    /// Validates a complete DTO object after transformation
    /// </summary>
    /// <param name="dto">The DTO to validate</param>
    /// <param name="rowNumber">Row number for error reporting</param>
    /// <returns>Validation result indicating success/failure and any error messages</returns>
    ValidationResult ValidateDto(T dto, int rowNumber);
}