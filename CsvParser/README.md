# Hastee CSV Parser Solution

## Overview

This solution implements a flexible and extensible CSV parser using the **Factory Pattern** with dependency injection of interfaces. The design allows for parsing different CSV file formats while maintaining a common output structure.

## Architecture

### Design Patterns Used

1. **Factory Pattern**: `CsvParserFactory` creates different parser types based on the provided parser type string
2. **Strategy Pattern**: Different parser implementations handle various CSV formats
3. **Dependency Injection**: Interfaces are injected into parsers for validation, transformation, and error handling

### Core Components

#### Interfaces
- **`IValidator`**: Handles data validation (single-cell and multi-cell)
- **`ITransformer`**: Transforms raw CSV data into common DTO format
- **`IErrorHandler`**: Records and manages parsing errors
- **`ICsvParser`**: Base interface for all CSV parsers

#### Models
- **`CustomerDto`**: Common Data Transfer Object for customer data

#### Parsers
- **`BaseCsvParser`**: Abstract base class with common parsing logic
- **`TypeAParser`**: Handles CSV format: `CustomerID, Full Name, Email, Phone, Salary`
- **`TypeBParser`**: Handles CSV format: `ID, Name, Surname, CorporateEmail, PersonalEmail, Salary`
- **`TypeCParser`**: Handles CSV format: `EmployeeID, FirstName, LastName, WorkEmail, MobileNumber, AnnualSalary, Department`

#### Implementations
- **`CustomerValidator`**: Concrete validation implementation
- **`CustomerTransformer`**: Concrete transformation implementation
- **`CsvErrorHandler`**: Concrete error handling implementation

#### Factory
- **`CsvParserFactory`**: Creates parser instances based on type

## Features

### ✅ Flexible and Extensible Design
- Easy to add new parser types by implementing `BaseCsvParser`
- Reusable validation, transformation, and error handling components
- Factory pattern allows dynamic parser selection

### ✅ Clean, Well-Structured Code
- Clear separation of concerns with interfaces
- Comprehensive error handling and reporting
- Well-documented code with XML comments

### ✅ Robust Error Handling
- Detailed error tracking with row/column information
- Error categorization (Validation, Parsing, Transformation, Exception)
- Comprehensive error reporting and summary

### ✅ Data Validation
- Single-cell validation for individual fields
- Multi-cell validation for related fields
- DTO-level validation after transformation

### ✅ Data Transformation
- Flexible mapping from different CSV formats to common DTO
- Support for external data enrichment (API calls, database queries)
- Priority logic for email fields (corporate vs personal)

## Usage Example

```csharp
// Create dependencies
IValidator validator = new CustomerValidator();
ITransformer transformer = new CustomerTransformer();
IErrorHandler errorHandler = new CsvErrorHandler();

// Create factory
var factory = new CsvParserFactory();

// Create parser for specific type
var parser = factory.CreateParser("TypeA", validator, transformer, errorHandler);

// Parse CSV file
var customers = await parser.ParseAsync("path/to/file.csv");

// Check for errors
if (errorHandler.HasErrors())
{
    var errorReport = errorHandler.ExportErrorReport();
    Console.WriteLine(errorReport);
}
```

## Supported CSV Formats

### TypeA Format
```
CustomerID, Full Name, Email, Phone, Salary
CUST001, John Doe, john.doe@email.com, +1234567890, 75000
```

### TypeB Format
```
ID, Name, Surname, CorporateEmail, PersonalEmail, Salary
EMP001, Alice, Brown, alice.brown@company.com, alice.personal@email.com, 90000
```

### TypeC Format
```
EmployeeID, FirstName, LastName, WorkEmail, MobileNumber, AnnualSalary, Department
E001, Frank, Miller, frank.miller@work.com, +1234567893, 95000, Engineering
```

## Key Design Decisions

### 1. Interface-Based Design
- **Why**: Enables easy testing, mocking, and swapping implementations
- **Benefit**: Loose coupling between components

### 2. Factory Pattern
- **Why**: Centralized parser creation with type safety
- **Benefit**: Easy to extend with new parser types

### 3. Abstract Base Class
- **Why**: Common parsing logic shared across all parsers
- **Benefit**: Reduces code duplication and ensures consistent behavior

### 4. Comprehensive Error Handling
- **Why**: Detailed error tracking is crucial for data processing
- **Benefit**: Helps identify and fix data quality issues

### 5. Async Support
- **Why**: External data enrichment may require API calls
- **Benefit**: Non-blocking operations for better performance

## Extending the Solution

### Adding a New Parser Type

1. Create a new parser class inheriting from `BaseCsvParser`:
```csharp
public class NewTypeParser : BaseCsvParser
{
    public NewTypeParser(IValidator validator, ITransformer transformer, IErrorHandler errorHandler) 
        : base(validator, transformer, errorHandler) { }
    
    public override string GetParserType() => "NewType";
    
    public override async Task<bool> ValidateFileStructureAsync(string filePath)
    {
        // Implement structure validation
    }
}
```

2. Register the new parser in the factory:
```csharp
factory.RegisterParserType("NewType", (v, t, e) => new NewTypeParser(v, t, e));
```

### Adding Custom Validation Rules

Extend `CustomerValidator` or create a new validator implementing `IValidator`:
```csharp
public class CustomValidator : IValidator
{
    // Implement validation methods
}
```

### Adding Custom Transformations

Extend `CustomerTransformer` or create a new transformer implementing `ITransformer`:
```csharp
public class CustomTransformer : ITransformer
{
    // Implement transformation methods
}
```

## Running the Demo

1. Build the project:
```bash
dotnet build
```

2. Run the demo:
```bash
dotnet run
```

The demo will:
- Create sample CSV files
- Parse them using different parser types
- Demonstrate error handling with invalid data
- Show error reports and summaries

## Benefits of This Design

1. **Maintainability**: Clear separation of concerns makes code easy to maintain
2. **Testability**: Interface-based design enables easy unit testing
3. **Extensibility**: Easy to add new parser types and validation rules
4. **Reusability**: Components can be reused across different parser types
5. **Error Handling**: Comprehensive error tracking and reporting
6. **Flexibility**: Supports various CSV formats with different field structures 