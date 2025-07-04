using CsvParser.ErrorHandling.Implementations;
using CsvParser.ErrorHandling.Interfaces;
using CsvParser.Models;
using CsvParser.Parsing.Factories;
using CsvParser.Shared.Constants;
using CsvParser.Transformation.Implementations;
using CsvParser.Transformation.Interfaces;
using CsvParser.Validation.Implementations;
using CsvParser.Validation.Interfaces;

/// <summary>
/// Main program demonstrating the CSV parser implementation.
/// Shows how to use the factory pattern with dependency injection of interfaces.
/// Uses modern .NET 9 top-level statements for cleaner, more concise code.
/// </summary>
Console.WriteLine($"{UiMessages.DemoTitle}\n");

// Create the dependencies (interfaces)
IValidator<CustomerDto> validator = new CustomerValidator();
ITransformer<CustomerDto> transformer = new CustomerTransformer();
IErrorHandler errorHandler = new CsvErrorHandler();

// Create the factory
var factory = new CsvParserFactory();

// Display supported parser types
Console.WriteLine(UiMessages.SupportedParserTypesTitle);
foreach (var parserType in factory.GetSupportedParserTypes())
{
    Console.WriteLine($"  - {parserType}");
}

Console.WriteLine();

// Example usage with different parser types
await DemonstrateParserUsage(factory, validator, transformer, errorHandler);

// Example of error handling
await DemonstrateErrorHandling(factory, validator, transformer, errorHandler);

Console.WriteLine($"\n{UiMessages.DemoCompleteMessage}");

/// <summary>
/// Demonstrates how to use different parser types
/// </summary>
static async Task DemonstrateParserUsage(
    CsvParserFactory factory,
    IValidator<CustomerDto> validator,
    ITransformer<CustomerDto> transformer,
    IErrorHandler errorHandler)
{
    Console.WriteLine(UiMessages.ParserUsageDemoTitle);

    // Parse TypeA CSV
    Console.WriteLine(UiMessages.ParsingTypeA);
    await ParseCsvFile(factory, ParserTypes.TypeA, FileNames.SampleTypeA, validator, transformer, errorHandler);

    // Parse TypeB CSV
    Console.WriteLine(UiMessages.ParsingTypeB);
    await ParseCsvFile(factory, ParserTypes.TypeB, FileNames.SampleTypeB, validator, transformer, errorHandler);

    // Parse TypeC CSV
    Console.WriteLine(UiMessages.ParsingTypeC);
    await ParseCsvFile(factory, ParserTypes.TypeC, FileNames.SampleTypeC, validator, transformer, errorHandler);
}

/// <summary>
/// Demonstrates error handling capabilities
/// </summary>
static async Task DemonstrateErrorHandling(
    CsvParserFactory factory,
    IValidator<CustomerDto> validator,
    ITransformer<CustomerDto> transformer,
    IErrorHandler errorHandler)
{
    Console.WriteLine(UiMessages.ErrorHandlingDemoTitle);

    // Clear previous errors
    errorHandler.ClearErrors();

    // Try to parse the file with errors
    Console.WriteLine(UiMessages.ParsingWithErrors);
    await ParseCsvFile(factory, ParserTypes.TypeA, FileNames.SampleWithErrors, validator, transformer, errorHandler);

    // Display error report
    Console.WriteLine(UiMessages.ErrorReportTitle);
    Console.WriteLine(errorHandler.ExportErrorReport());

    // Show error summary
    var errorSummary = errorHandler.GetErrorSummary();
    Console.WriteLine(UiMessages.ErrorSummaryTitle);
    foreach (var kvp in errorSummary)
    {
        Console.WriteLine(string.Format(UiMessages.ErrorSummaryFormat, kvp.Key, kvp.Value));
    }
}

/// <summary>
/// Parses a CSV file using the specified parser type
/// </summary>
static async Task ParseCsvFile(
    CsvParserFactory factory,
    string parserType,
    string filePath,
    IValidator<CustomerDto> validator,
    ITransformer<CustomerDto> transformer,
    IErrorHandler errorHandler)
{
    try
    {
        // Create parser using factory
        var parser = factory.CreateParser(parserType, validator, transformer, errorHandler);

        Console.WriteLine(string.Format(UiMessages.UsingParser, parser.GetParserType()));

        // Validate file structure
        var isValidStructure = await parser.ValidateFileStructureAsync(filePath);
        Console.WriteLine(string.Format(UiMessages.FileStructureValid, isValidStructure));

        if (!isValidStructure)
        {
            Console.WriteLine(UiMessages.SkippingParsing);
            return;
        }

        // Parse the file
        var customers = await parser.ParseAsync(filePath);

        Console.WriteLine(string.Format(UiMessages.SuccessfullyParsed, customers.Count));
        foreach (var customer in customers)
        {
            Console.WriteLine(string.Format(UiMessages.CustomerFormat, customer));
        }

        // Check for errors
        if (errorHandler.HasErrors())
        {
            Console.WriteLine(string.Format(UiMessages.Warnings, errorHandler.GetErrorCount()));
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine(string.Format(UiMessages.ErrorFormat, ex.Message));
    }
}



