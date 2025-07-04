namespace CsvParser.Shared.Constants;

/// <summary>
/// Contains all error message constants used across the CSV parser application.
/// These constants ensure consistent error messaging throughout the application.
/// </summary>
public static class ErrorMessages
{
    // Validation error messages
    public const string IdEmpty = "ID cannot be empty";
    public const string IdTooShort = "ID must be at least 3 characters long";
    public const string NameEmpty = "Name cannot be empty";
    public const string NameTooShort = "Name must be at least 2 characters long";
    public const string InvalidEmailFormat = "Invalid email format: {0}";
    public const string InvalidPhoneFormat = "Invalid phone number format: {0}";
    public const string InvalidSalary = "Invalid salary value: {0}. Must be a positive number.";
    public const string InvalidNameCharacters = "Name contains invalid characters: {0}";
    public const string RequiredFieldEmpty = "Required field '{0}' cannot be empty (Row {1})";
    public const string EmailRequired = "At least one email address must be provided (Row {0})";
    public const string CustomerIdEmpty = "Customer ID cannot be empty (Row {0})";
    public const string FullNameEmpty = "Full name cannot be empty (Row {0})";
    public const string EmailEmpty = "Email cannot be empty (Row {0})";
    public const string InvalidEmailFormatRow = "Invalid email format: {0} (Row {1})";
    public const string SalaryMustBePositive = "Salary must be greater than zero (Row {0})";
    public const string FullNameFormat = "Full Name must contain at least first and last name (Row {0})";
    public const string NameAndSurnameRequired = "Both Name and Surname must be provided (Row {0})";
    public const string EmailRequiredTypeB = "At least one email (Corporate or Personal) must be provided (Row {0})";
    public const string FirstNameAndLastNameRequired = "Both FirstName and LastName must be provided (Row {0})";
    public const string DepartmentRequired = "Department must be provided (Row {0})";

    // Parsing error messages
    public const string FileStructureIncompatible = "File structure is not compatible with {0} parser";
    public const string FileTooShort = "CSV file must contain at least a header row and one data row";
    public const string ErrorParsingRow = "Error parsing row {0}";
    public const string ErrorDuringFileParsing = "Error during file parsing";
    public const string ErrorParsingDataRow = "Error parsing data row {0}";
    public const string ColumnValidationError = "Column '{0}': {1}";

    // Factory error messages
    public const string ParserTypeNullOrEmpty = "Parser type cannot be null or empty";
    public const string UnsupportedParserType = "Unsupported parser type '{0}'. Supported types: {1}";

    // Error report messages
    public const string NoErrors = "No errors recorded.";
    public const string ErrorReportTitle = "CSV Parsing Error Report - {0:yyyy-MM-dd HH:mm:ss} UTC";
    public const string TotalErrors = "Total Errors: {0}";
    public const string ErrorTypeSummary = "{0} Errors ({1}):";
    public const string ErrorDetail = "  {0}";
    public const string ExceptionMessage = "{0}: {1}";
}