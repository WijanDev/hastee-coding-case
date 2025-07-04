using System.Text.RegularExpressions;
using CsvParser.Models;
using CsvParser.Shared.Constants;
using CsvParser.Validation.Interfaces;
using CsvParser.Validation.Models;

namespace CsvParser.Validation.Implementations;

/// <summary>
/// Concrete implementation of IValidator for customer data validation.
/// Provides reusable validation methods for customer information.
/// </summary>
public class CustomerValidator : IValidator<CustomerDto>
{
    private static readonly HashSet<string> IdFields = new([
        FieldNamePatterns.Ids.CustomerId,
        FieldNamePatterns.Ids.Id,
        FieldNamePatterns.Ids.EmployeeId
    ]);

    private static readonly HashSet<string> EmailFields = new([
        FieldNamePatterns.Emails.Email,
        FieldNamePatterns.Emails.CorporateEmail,
        FieldNamePatterns.Emails.PersonalEmail,
        FieldNamePatterns.Emails.WorkEmail
    ]);

    private static readonly HashSet<string> PhoneFields = new([
        FieldNamePatterns.Phones.Phone,
        FieldNamePatterns.Phones.MobileNumber
    ]);

    private static readonly HashSet<string> SalaryFields = new([
        FieldNamePatterns.Salaries.Salary,
        FieldNamePatterns.Salaries.AnnualSalary
    ]);

    private static readonly HashSet<string> NameFields = new([
        FieldNamePatterns.Names.FullName,
        FieldNamePatterns.Names.Name,
        FieldNamePatterns.Names.Surname,
        FieldNamePatterns.Names.FirstName,
        FieldNamePatterns.Names.LastName
    ]);

    private readonly Regex _emailRegex = new(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.Compiled);
    private readonly Regex _phoneRegex = new(@"^[\+]?[1-9][\d]{0,15}$", RegexOptions.Compiled);

    public ValidationResult ValidateSingleCell(string value, string fieldName, int rowNumber, int columnNumber)
    {
        var result = new ValidationResult(true, rowNumber, columnNumber);

        // Skip validation for empty values (handled by multi-cell validation)
        if (string.IsNullOrWhiteSpace(value))
        {
            return result;
        }

        var fieldNameLower = fieldName.ToLowerInvariant();

        if (IdFields.Contains(fieldNameLower))
        {
            ValidateId(value, result);
            return result;
        }

        if (EmailFields.Contains(fieldNameLower))
        {
            ValidateEmail(value, result);
            return result;
        }

        if (PhoneFields.Contains(fieldNameLower))
        {
            ValidatePhone(value, result);
            return result;
        }

        if (SalaryFields.Contains(fieldNameLower))
        {
            ValidateSalary(value, result);
            return result;
        }

        if (NameFields.Contains(fieldNameLower))
        {
            ValidateName(value, result);
            return result;
        }

        return result;
    }

    public ValidationResult ValidateMultiCell(Dictionary<string, string> values, int rowNumber)
    {
        var result = new ValidationResult(true, rowNumber);

        // Validate that required fields are not empty
        var requiredFields = GetRequiredFields(values);
        foreach (var field in requiredFields)
        {
            if (values.TryGetValue(field, out var fieldValue) && string.IsNullOrWhiteSpace(fieldValue))
            {
                result.AddError(string.Format(ErrorMessages.RequiredFieldEmpty, field, rowNumber));
            }
        }

        // Validate email priority logic for TypeB
        if (values.TryGetValue(FieldNames.Emails.CorporateEmail, out var corporateEmail) && values.TryGetValue(FieldNames.Emails.PersonalEmail, out var personalEmail))
        {
            if (string.IsNullOrWhiteSpace(corporateEmail) && string.IsNullOrWhiteSpace(personalEmail))
            {
                result.AddError(string.Format(ErrorMessages.EmailRequired, rowNumber));
            }
        }

        return result;
    }

    public ValidationResult ValidateDto(CustomerDto customer, int rowNumber)
    {
        var result = new ValidationResult(true, rowNumber);

        // Validate required DTO properties
        if (string.IsNullOrWhiteSpace(customer.Id))
        {
            result.AddError(string.Format(ErrorMessages.CustomerIdEmpty, rowNumber));
        }

        if (string.IsNullOrWhiteSpace(customer.FullName))
        {
            result.AddError(string.Format(ErrorMessages.FullNameEmpty, rowNumber));
        }

        if (string.IsNullOrWhiteSpace(customer.Email))
        {
            result.AddError(string.Format(ErrorMessages.EmailEmpty, rowNumber));
        }
        else if (!_emailRegex.IsMatch(customer.Email))
        {
            result.AddError(string.Format(ErrorMessages.InvalidEmailFormatRow, customer.Email, rowNumber));
        }

        if (customer.Salary <= Shared.Constants.ValidationConstants.MinimumSalary)
        {
            result.AddError(string.Format(ErrorMessages.SalaryMustBePositive, rowNumber));
        }

        return result;
    }

    private void ValidateId(string value, ValidationResult result)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            result.AddError(ErrorMessages.IdEmpty);
        }
        else if (value.Length < Shared.Constants.ValidationConstants.MinimumIdLength)
        {
            result.AddError(ErrorMessages.IdTooShort);
        }
    }

    private void ValidateEmail(string value, ValidationResult result)
    {
        if (!_emailRegex.IsMatch(value))
        {
            result.AddError(string.Format(ErrorMessages.InvalidEmailFormat, value));
        }
    }

    private void ValidatePhone(string value, ValidationResult result)
    {
        if (!_phoneRegex.IsMatch(value))
        {
            result.AddError(string.Format(ErrorMessages.InvalidPhoneFormat, value));
        }
    }

    private void ValidateSalary(string value, ValidationResult result)
    {
        if (!decimal.TryParse(value, out var salary) || salary <= Shared.Constants.ValidationConstants.MinimumSalary)
        {
            result.AddError(string.Format(ErrorMessages.InvalidSalary, value));
        }
    }

    private void ValidateName(string value, ValidationResult result)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            result.AddError(ErrorMessages.NameEmpty);
        }
        else if (value.Length < Shared.Constants.ValidationConstants.MinimumNameLength)
        {
            result.AddError(ErrorMessages.NameTooShort);
        }
        else if (!value.All(c => char.IsLetter(c) || char.IsWhiteSpace(c) || c == '-' || c == '\''))
        {
            result.AddError(string.Format(ErrorMessages.InvalidNameCharacters, value));
        }
    }

    private IEnumerable<string> GetRequiredFields(Dictionary<string, string> values)
    {
        var requiredFields = new List<string>();

        // Determine required fields based on available columns
        if (values.ContainsKey(FieldNames.Ids.CustomerId) || values.ContainsKey(FieldNames.Ids.Id) || values.ContainsKey(FieldNames.Ids.EmployeeId))
        {
            requiredFields.Add(values.Keys.First(k => k.Contains("ID", StringComparison.OrdinalIgnoreCase)));
        }

        if (values.TryGetValue(FieldNames.Names.FullName, out _))
        {
            requiredFields.Add(FieldNames.Names.FullName);
        }
        else if (values.TryGetValue(FieldNames.Names.Name, out _) && values.TryGetValue(FieldNames.Names.Surname, out _))
        {
            requiredFields.AddRange(new[] { FieldNames.Names.Name, FieldNames.Names.Surname });
        }
        else if (values.TryGetValue(FieldNames.Names.FirstName, out _) && values.TryGetValue(FieldNames.Names.LastName, out _))
        {
            requiredFields.AddRange(new[] { FieldNames.Names.FirstName, FieldNames.Names.LastName });
        }

        if (values.TryGetValue(FieldNames.Emails.Email, out _))
        {
            requiredFields.Add(FieldNames.Emails.Email);
        }
        else if (values.TryGetValue(FieldNames.Emails.CorporateEmail, out _) || values.TryGetValue(FieldNames.Emails.PersonalEmail, out _))
        {
            // At least one email is required, but we handle this in multi-cell validation
        }
        else if (values.TryGetValue(FieldNames.Emails.WorkEmail, out _))
        {
            requiredFields.Add(FieldNames.Emails.WorkEmail);
        }

        if (values.TryGetValue(FieldNames.Salaries.Salary, out _) || values.TryGetValue(FieldNames.Salaries.AnnualSalary, out _))
        {
            requiredFields.Add(values.Keys.First(k => k.Contains("Salary", StringComparison.OrdinalIgnoreCase)));
        }

        return requiredFields;
    }
}