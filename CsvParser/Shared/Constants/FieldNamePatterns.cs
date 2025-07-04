namespace CsvParser.Shared.Constants;

/// <summary>
/// Contains field name patterns used in switch statements for validation and transformation.
/// These are lowercase patterns used for case-insensitive field name matching.
/// </summary>
public static class FieldNamePatterns
{
    /// <summary>
    /// ID field patterns used in switch statements
    /// </summary>
    public static class Ids
    {
        public const string CustomerId = "customerid";
        public const string Id = "id";
        public const string EmployeeId = "employeeid";
    }

    /// <summary>
    /// Name field patterns used in switch statements
    /// </summary>
    public static class Names
    {
        public const string FullName = "full name";
        public const string Name = "name";
        public const string Surname = "surname";
        public const string FirstName = "firstname";
        public const string LastName = "lastname";
    }

    /// <summary>
    /// Email field patterns used in switch statements
    /// </summary>
    public static class Emails
    {
        public const string Email = "email";
        public const string CorporateEmail = "corporateemail";
        public const string PersonalEmail = "personalemail";
        public const string WorkEmail = "workemail";
    }

    /// <summary>
    /// Phone field patterns used in switch statements
    /// </summary>
    public static class Phones
    {
        public const string Phone = "phone";
        public const string MobileNumber = "mobilenumber";
    }

    /// <summary>
    /// Salary field patterns used in switch statements
    /// </summary>
    public static class Salaries
    {
        public const string Salary = "salary";
        public const string AnnualSalary = "annualsalary";
    }
}