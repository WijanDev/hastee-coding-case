namespace CsvParser.Transformation.Interfaces;

/// <summary>
/// Interface for transforming raw CSV data into the common DTO format.
/// Supports both single-cell and multi-cell transformation scenarios.
/// </summary>
/// <typeparam name="T">The type of DTO to transform into</typeparam>
public interface ITransformer<T> where T : class
{
    /// <summary>
    /// Transforms a single cell value according to specific rules
    /// </summary>
    /// <param name="value">The raw cell value to transform</param>
    /// <param name="fieldName">Name of the field being transformed</param>
    /// <returns>Transformed value</returns>
    string TransformSingleCell(string value, string fieldName);

    /// <summary>
    /// Transforms multiple related cell values together
    /// </summary>
    /// <param name="values">Dictionary of field names and their corresponding raw values</param>
    /// <returns>Dictionary of transformed values</returns>
    Dictionary<string, string> TransformMultiCell(Dictionary<string, string> values);

    /// <summary>
    /// Transforms raw CSV row data into a DTO object
    /// </summary>
    /// <param name="rowData">Dictionary of column headers and their corresponding values</param>
    /// <returns>Transformed DTO object</returns>
    T TransformToDto(Dictionary<string, string> rowData);

    /// <summary>
    /// Enriches the DTO with external data (e.g., from API or database)
    /// </summary>
    /// <param name="dto">The DTO to enrich</param>
    /// <returns>Enriched DTO object</returns>
    Task<T> EnrichWithExternalDataAsync(T dto);
}