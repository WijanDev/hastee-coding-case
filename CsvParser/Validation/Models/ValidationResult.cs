namespace CsvParser.Validation.Models;

/// <summary>
/// Represents the result of a validation operation
/// </summary>
public class ValidationResult(bool isValid, int rowNumber, int? columnNumber = null)
{
    public bool IsValid { get; private set; } = isValid;

    public int RowNumber { get; } = rowNumber;

    public int? ColumnNumber { get; } = columnNumber;

    public List<string> Errors { get; } = new();

    public void AddError(string error)
    {
        IsValid = false;
        Errors.Add(error);
    }
}