using CsvParser.ErrorHandling.Interfaces;
using CsvParser.Models;
using CsvParser.Shared.Constants;
using CsvParser.Transformation.Interfaces;
using CsvParser.Validation.Interfaces;
using CsvParser.Validation.Models;

namespace CsvParser.Parsing.Implementations;

/// <summary>
/// Parser for Type B CSV format:
/// ID, Name, Surname, CorporateEmail, PersonalEmail, Salary
/// </summary>
public class TypeBParser(
    IValidator<CustomerDto> validator,
    ITransformer<CustomerDto> transformer,
    IErrorHandler errorHandler)
    : BaseCsvParser<CustomerDto>(
        validator,
        transformer,
        errorHandler)
{
    private readonly string[] _expectedHeaders = [FieldNames.Ids.Id, FieldNames.Names.Name, FieldNames.Names.Surname, FieldNames.Emails.CorporateEmail, FieldNames.Emails.PersonalEmail, FieldNames.Salaries.Salary];

    public override string GetParserType() => ParserTypes.TypeB;

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
    /// Override to provide TypeB-specific validation logic
    /// </summary>
    protected override async Task<ValidationResult> ValidateRowDataAsync(Dictionary<string, string> rowData, int rowNumber)
    {
        var result = await base.ValidateRowDataAsync(rowData, rowNumber);

        // TypeB-specific validations
        if (
            !rowData.TryGetValue(FieldNames.Names.Name, out string? name)
            || !rowData.TryGetValue(FieldNames.Names.Surname, out string? surname)
            || string.IsNullOrEmpty(name)
            || string.IsNullOrEmpty(surname))
        {
            result.AddError(string.Format(ErrorMessages.NameAndSurnameRequired, rowNumber));
        }

        // Validate that at least one email is provided
        if (
            !rowData.TryGetValue(FieldNames.Emails.CorporateEmail, out string? corporateEmail)
            || !rowData.TryGetValue(FieldNames.Emails.PersonalEmail, out string? personalEmail)
            || string.IsNullOrEmpty(corporateEmail)
            || string.IsNullOrEmpty(personalEmail))
        {
            result.AddError(string.Format(ErrorMessages.EmailRequiredTypeB, rowNumber));
        }

        return result;
    }
}