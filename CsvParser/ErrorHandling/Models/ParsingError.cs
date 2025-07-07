namespace CsvParser.ErrorHandling.Models;

public class ParsingError(int rowNumber, int? columnNumber, string errorMessage, ErrorType errorType)
{
    public int RowNumber { get; } = rowNumber;

    public int? ColumnNumber { get; } = columnNumber;

    public string ErrorMessage { get; } = errorMessage;

    public string? ExpectedValue { get; init; }

    public string? ActualValue { get; init; }

    public string? FieldName { get; init; }

    public ErrorType ErrorType { get; } = errorType;

    public DateTime Timestamp { get; init; } = DateTime.UtcNow;

    public string? Context { get; init; }

    public override string ToString()
    {
        var location = ColumnNumber.HasValue
            ? $"Row {RowNumber}, Column {ColumnNumber}"
            : $"Row {RowNumber}";

        return $"[{ErrorType}] {location}: {ErrorMessage}";
    }
}
