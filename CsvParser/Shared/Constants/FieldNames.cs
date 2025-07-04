namespace CsvParser.Shared.Constants;

/// <summary>
/// Contains all field name constants used across the CSV parser application.
/// These constants ensure consistency in field name handling across different parsers and components.
/// </summary>
public static class FieldNames
{
    /// <summary>
    /// ID field variations used across different CSV formats
    /// </summary>
    public static class Ids
    {
        public const string CustomerId = "CustomerID";
        public const string Id = "ID";
        public const string EmployeeId = "EmployeeID";
    }

    /// <summary>
    /// Name field variations used across different CSV formats
    /// </summary>
    public static class Names
    {
        public const string FullName = "Full Name";
        public const string Name = "Name";
        public const string Surname = "Surname";
        public const string FirstName = "FirstName";
        public const string LastName = "LastName";
    }

    /// <summary>
    /// Email field variations used across different CSV formats
    /// </summary>
    public static class Emails
    {
        public const string Email = "Email";
        public const string CorporateEmail = "CorporateEmail";
        public const string PersonalEmail = "PersonalEmail";
        public const string WorkEmail = "WorkEmail";
    }

    /// <summary>
    /// Phone field variations used across different CSV formats
    /// </summary>
    public static class Phones
    {
        public const string Phone = "Phone";
        public const string MobileNumber = "MobileNumber";
    }

    /// <summary>
    /// Salary field variations used across different CSV formats
    /// </summary>
    public static class Salaries
    {
        public const string Salary = "Salary";
        public const string AnnualSalary = "AnnualSalary";
    }

    /// <summary>
    /// Additional fields used across different CSV formats
    /// </summary>
    public static class Additional
    {
        public const string Department = "Department";
    }
}