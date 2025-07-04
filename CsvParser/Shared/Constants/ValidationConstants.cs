namespace CsvParser.Shared.Constants;

/// <summary>
/// Contains all validation-related constants used across the CSV parser application.
/// These constants define validation rules and thresholds for data validation.
/// </summary>
public static class ValidationConstants
{
    // Length validation constants
    public const int MinimumIdLength = 3;
    public const int MinimumNameLength = 2;
    public const int MinimumNameParts = 2;

    // Value validation constants
    public const decimal MinimumSalary = 0;

    // File structure constants
    public const int MinimumFileLines = 2; // Header + at least 1 data row
    public const int HeaderRowIndex = 0;
    public const int DataRowStartIndex = 1;
}