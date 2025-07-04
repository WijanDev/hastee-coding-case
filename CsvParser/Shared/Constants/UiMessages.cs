namespace CsvParser.Shared.Constants;

/// <summary>
/// Contains all UI message constants used across the CSV parser application.
/// These constants define user interface messages and console output formatting.
/// </summary>
public static class UiMessages
{
    // Demo titles and headers
    public const string DemoTitle = "=== Hastee CSV Parser Demo ===";
    public const string SupportedParserTypesTitle = "Supported Parser Types:";
    public const string ParserUsageDemoTitle = "=== Parser Usage Demo ===";
    public const string ErrorHandlingDemoTitle = "=== Error Handling Demo ===";
    public const string DemoCompleteMessage = "=== Demo Complete ===";

    // Parsing messages
    public const string ParsingTypeA = "\n1. Parsing TypeA CSV file:";
    public const string ParsingTypeB = "\n2. Parsing TypeB CSV file:";
    public const string ParsingTypeC = "\n3. Parsing TypeC CSV file:";
    public const string ParsingWithErrors = "\nParsing CSV file with errors:";

    // Status messages
    public const string UsingParser = "  Using parser: {0}";
    public const string FileStructureValid = "  File structure valid: {0}";
    public const string SkippingParsing = "  Skipping parsing due to invalid file structure.";
    public const string SuccessfullyParsed = "  Successfully parsed {0} customers:";
    public const string CustomerFormat = "    {0}";
    public const string Warnings = "  Warnings: {0} errors recorded during parsing";
    public const string ErrorFormat = "  Error: {0}";

    // Error handling messages
    public const string ErrorReportTitle = "\nError Report:";
    public const string ErrorSummaryTitle = "\nError Summary:";
    public const string ErrorSummaryFormat = "  {0}: {1} errors";
}