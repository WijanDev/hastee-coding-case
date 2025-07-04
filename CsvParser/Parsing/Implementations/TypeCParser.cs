using CsvParser.ErrorHandling.Interfaces;
using CsvParser.Models;
using CsvParser.Shared.Constants;
using CsvParser.Transformation.Interfaces;
using CsvParser.Validation.Interfaces;
using CsvParser.Validation.Models;

namespace CsvParser.Parsing.Implementations;

/// <summary>
/// Parser for Type C CSV format (example of extensibility):
/// EmployeeID, FirstName, LastName, WorkEmail, MobileNumber, AnnualSalary, Department
/// </summary>
public class TypeCParser(
    IValidator<CustomerDto> validator,
    ITransformer<CustomerDto> transformer,
    IErrorHandler errorHandler)
    : BaseCsvParser<CustomerDto>(
        validator,
        transformer,
        errorHandler)
{
    private readonly string[] _expectedHeaders = [FieldNames.Ids.EmployeeId, FieldNames.Names.FirstName, FieldNames.Names.LastName, FieldNames.Emails.WorkEmail, FieldNames.Phones.MobileNumber, FieldNames.Salaries.AnnualSalary, FieldNames.Additional.Department];

    public override string GetParserType() => ParserTypes.TypeC;

    public override async Task<bool> ValidateFileStructureAsync(string filePath)
    {
        try
        {
            var lines = await File.ReadAllLinesAsync(filePath);
            if (lines.Length == 0)
            {
                return false;
            }

            var headers = ParseHeaderRow(lines[0]);

            return ValidateHeaders(headers, _expectedHeaders);
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// Override to provide TypeC-specific validation logic
    /// </summary>
    protected override async Task<ValidationResult> ValidateRowDataAsync(Dictionary<string, string> rowData, int rowNumber)
    {
        var result = await base.ValidateRowDataAsync(rowData, rowNumber);

        // TypeC-specific validations
        if (
            !rowData.TryGetValue(FieldNames.Names.FirstName, out string? firstName)
            || !rowData.TryGetValue(FieldNames.Names.LastName, out string? lastName)
            || string.IsNullOrEmpty(firstName)
            || string.IsNullOrEmpty(lastName))
        {
            result.AddError(string.Format(ErrorMessages.FirstNameAndLastNameRequired, rowNumber));
        }

        // Validate department is provided
        if (
            !rowData.TryGetValue(FieldNames.Additional.Department, out string? department)
            || string.IsNullOrEmpty(department))
        {
            result.AddError(string.Format(ErrorMessages.DepartmentRequired, rowNumber));
        }

        return result;
    }
}