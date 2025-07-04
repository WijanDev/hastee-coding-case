namespace CsvParser.Parsing.Interfaces;

/// <summary>
/// Base interface for CSV parsers that defines the contract for parsing CSV files
/// into a common DTO format.
/// </summary>
/// <typeparam name="T">The type of DTO to parse into</typeparam>
public interface ICsvParser<T> where T : class
{
    /// <summary>
    /// Parses a CSV file and returns a collection of DTO objects
    /// </summary>
    /// <param name="filePath">Path to the CSV file to parse</param>
    /// <returns>Collection of parsed DTO objects</returns>
    Task<List<T>> ParseAsync(string filePath);

    /// <summary>
    /// Gets the parser type identifier
    /// </summary>
    /// <returns>String identifier for this parser type</returns>
    string GetParserType();

    /// <summary>
    /// Validates that the CSV file structure is compatible with this parser
    /// </summary>
    /// <param name="filePath">Path to the CSV file to validate</param>
    /// <returns>True if the file structure is compatible, false otherwise</returns>
    Task<bool> ValidateFileStructureAsync(string filePath);
}