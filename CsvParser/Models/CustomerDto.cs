namespace CsvParser.Models;

public record CustomerDto(
    string Id,
    string FullName,
    string Email,
    string? SecondaryEmail,
    string? Phone,
    decimal Salary,
    Dictionary<string, object> Metadata);