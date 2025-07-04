namespace CsvParser.Shared.Constants;

/// <summary>
/// Contains all phone number-related constants used across the CSV parser application.
/// These constants define phone number formatting and generation rules.
/// </summary>
public static class PhoneNumberConstants
{
    // Phone number formatting constants
    public const string Prefix = "+1-555-";
    public const char PaddingChar = '0';
    public const int PaddingLength = 4;
}