using CsvParser.Models;
using CsvParser.Shared.Constants;
using CsvParser.Transformation.Interfaces;

namespace CsvParser.Transformation.Implementations;

/// <summary>
/// Concrete implementation of ITransformer for transforming CSV data into CustomerDto objects.
/// Provides reusable transformation methods for customer information.
/// </summary>
public class CustomerTransformer : ITransformer<CustomerDto>
{
    private static readonly HashSet<string> NameFields = new([
        FieldNamePatterns.Names.FullName,
        FieldNamePatterns.Names.Name,
        FieldNamePatterns.Names.Surname,
        FieldNamePatterns.Names.FirstName,
        FieldNamePatterns.Names.LastName
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

    public string TransformSingleCell(string value, string fieldName)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return value;
        }

        var fieldNameLower = fieldName.ToLowerInvariant();
        if (NameFields.Contains(fieldNameLower))
        {
            return TransformName(value);
        }

        if (EmailFields.Contains(fieldNameLower))
        {
            return TransformEmail(value);
        }

        if (PhoneFields.Contains(fieldNameLower))
        {
            return TransformPhone(value);
        }

        if (SalaryFields.Contains(fieldNameLower))
        {
            return TransformSalary(value);
        }

        return value.Trim();
    }

    public Dictionary<string, string> TransformMultiCell(Dictionary<string, string> values)
    {
        var transformed = new Dictionary<string, string>();

        foreach (var kvp in values)
        {
            transformed[kvp.Key] = TransformSingleCell(kvp.Value, kvp.Key);
        }

        return transformed;
    }

    public CustomerDto TransformToDto(Dictionary<string, string> rowData)
    {
        var (primaryEmail, secondaryEmail) = GetEmailValues(rowData);
        return new CustomerDto(
            GetIdValue(rowData),
            GetFullNameValue(rowData),
            primaryEmail,
            secondaryEmail,
            GetPhoneValue(rowData),
            GetSalaryValue(rowData),
            GetMetadata(rowData));
    }

    public async Task<CustomerDto> EnrichWithExternalDataAsync(CustomerDto customer)
    {
        // Example: Enrich with external data (e.g., from API or database)
        // This is where you would make API calls or database queries to get additional information

        // For TypeB parser, if phone is missing, we might fetch it from an external service
        if (string.IsNullOrWhiteSpace(customer.Phone) && !string.IsNullOrWhiteSpace(customer.Id))
        {
            try
            {
                // Simulate API call to get phone number
                var phone = await GetPhoneFromExternalServiceAsync(customer.Id);
                var updatedMetadata = new Dictionary<string, object>(customer.Metadata)
                {
                    [MetadataConstants.PhoneSource] = MetadataConstants.ExternalApi
                };

                return customer with { Phone = phone, Metadata = updatedMetadata };
            }
            catch (Exception)
            {
                // Log the error but don't fail the entire process
                var updatedMetadata = new Dictionary<string, object>(customer.Metadata)
                {
                    [MetadataConstants.PhoneSource] = MetadataConstants.NotAvailable
                };

                return customer with { Metadata = updatedMetadata };
            }
        }

        return customer;
    }

    private static string TransformName(string value)
    {
        // Normalize name formatting
        return value.Trim()
                   .ToLowerInvariant()
                   .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                   .Select(word => char.ToUpperInvariant(word[0]) + word[1..])
                   .Aggregate((current, next) => current + " " + next);
    }

    private static string TransformEmail(string value)
    {
        return value.Trim().ToLowerInvariant();
    }

    private static string TransformPhone(string value)
    {
        // Remove all non-digit characters except + for international numbers
        var cleaned = new string(value.Where(c => char.IsDigit(c) || c == '+').ToArray());

        // Ensure it starts with + for international format
        if (!cleaned.StartsWith("+") && cleaned.Length > 0)
        {
            cleaned = "+" + cleaned;
        }

        return cleaned;
    }

    private static string TransformSalary(string value)
    {
        // Remove currency symbols and formatting
        var cleaned = new string(value.Where(c => char.IsDigit(c) || c == '.' || c == ',').ToArray());

        // Handle comma as decimal separator
        if (cleaned.Contains(','))
        {
            cleaned = cleaned.Replace(",", ".");
        }

        return cleaned;
    }

    private static string GetIdValue(Dictionary<string, string> rowData)
    {
        var keys = new[] { FieldNames.Ids.CustomerId, FieldNames.Ids.Id, FieldNames.Ids.EmployeeId };
        foreach (var key in keys)
        {
            if (rowData.TryGetValue(key, out var value))
            {
                return value;
            }
        }

        return string.Empty;
    }

    private static string GetFullNameValue(Dictionary<string, string> rowData)
    {
        // Handle different name formats
        if (rowData.TryGetValue(FieldNames.Names.FullName, out var fullName))
        {
            return fullName;
        }

        if (rowData.TryGetValue(FieldNames.Names.Name, out var name) && rowData.TryGetValue(FieldNames.Names.Surname, out var surname))
        {
            return $"{name} {surname}";
        }

        if (rowData.TryGetValue(FieldNames.Names.FirstName, out var firstName) && rowData.TryGetValue(FieldNames.Names.LastName, out var lastName))
        {
            return $"{firstName} {lastName}";
        }

        return string.Empty;
    }

    private (string PrimaryEmail, string? SecondaryEmail) GetEmailValues(Dictionary<string, string> rowData)
    {
        // Handle different email formats with priority logic
        if (rowData.TryGetValue(FieldNames.Emails.Email, out var email))
        {
            return (email, null);
        }

        if (rowData.TryGetValue(FieldNames.Emails.CorporateEmail, out var corporateEmail) && rowData.TryGetValue(FieldNames.Emails.PersonalEmail, out var personalEmail))
        {
            // Corporate email takes priority if both are available
            if (!string.IsNullOrWhiteSpace(corporateEmail))
            {
                return (corporateEmail, string.IsNullOrWhiteSpace(personalEmail) ? null : personalEmail);
            }

            return (personalEmail, null);
        }

        if (rowData.TryGetValue(FieldNames.Emails.WorkEmail, out var workEmail))
        {
            return (workEmail, null);
        }

        return (string.Empty, null);
    }

    private string? GetPhoneValue(Dictionary<string, string> rowData)
    {
        var phoneKeys = new[] { FieldNames.Phones.Phone, FieldNames.Phones.MobileNumber };
        foreach (var key in phoneKeys)
        {
            if (rowData.TryGetValue(key, out var value) && !string.IsNullOrWhiteSpace(value))
            {
                return value;
            }
        }

        return null;
    }

    private decimal GetSalaryValue(Dictionary<string, string> rowData)
    {
        var salaryKeys = new[] { FieldNames.Salaries.Salary, FieldNames.Salaries.AnnualSalary };
        foreach (var key in salaryKeys)
        {
            if (rowData.TryGetValue(key, out var value) && decimal.TryParse(value, out var salary))
            {
                return salary;
            }
        }

        return Defaults.Salary;
    }

    private Dictionary<string, object> GetMetadata(Dictionary<string, string> rowData)
    {
        var metadata = new Dictionary<string, object>
        {
            // Store original field names for debugging
            [MetadataConstants.OriginalFields] = string.Join(", ", rowData.Keys)
        };

        // Store parser-specific information
        if (rowData.TryGetValue(FieldNames.Additional.Department, out var department))
        {
            metadata[MetadataConstants.Department] = department;
        }

        return metadata;
    }

    private async Task<string?> GetPhoneFromExternalServiceAsync(string customerId)
    {
        // Simulate external API call
        await Task.Delay(NetworkConstants.DelayMilliseconds); // Simulate network delay

        // In a real implementation, this would make an HTTP request to an external service
        // For now, we'll return a mock phone number
        return $"{PhoneNumberConstants.Prefix}{customerId.PadLeft(PhoneNumberConstants.PaddingLength, PhoneNumberConstants.PaddingChar)}";
    }
}