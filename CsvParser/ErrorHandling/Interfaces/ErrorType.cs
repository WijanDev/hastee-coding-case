namespace CsvParser.ErrorHandling.Interfaces;

/// <summary>
/// Types of errors that can occur during parsing
/// </summary>
public enum ErrorType
{
    Validation,
    Parsing,
    Transformation,
    Exception
}