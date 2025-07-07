# Hastee CSV Parser

A flexible, extensible CSV parser built in C# that can handle different CSV file structures and transform them into a common Data Transfer Object (DTO).

## 🚀 Quick Start

### Prerequisites
- .NET 9.0 SDK or later
- Windows, macOS, or Linux

### Running the Program

1. **Clone the repository**
   ```bash
   git clone <repository-url>
   cd hastee-coding-case/CsvParser
   ```

2. **Build the project**
   ```bash
   dotnet build
   ```

3. **Run the program**
   ```bash
   dotnet run
   ```

The program will automatically demonstrate parsing different CSV file types and show error handling capabilities.

### Sample CSV Files

The project includes sample CSV files for testing:
- `sample_typea.csv` - Simple customer data format
- `sample_typeb.csv` - Corporate employee data with multiple emails
- `sample_typec.csv` - Employee data with department information
- `sample_with_errors.csv` - File containing various validation errors

## 🏗️ Architecture Overview

### Core Design Principles

1. **Extensibility**: Easy to add new parser types without modifying existing code
2. **Separation of Concerns**: Validation, transformation, and parsing are separate responsibilities
3. **Error Handling**: Comprehensive error tracking and reporting
4. **Type Safety**: Full nullable reference type support
5. **Modern C#**: Uses latest C# features (records, primary constructors, pattern matching)

### Project Structure

```
CsvParser/
├── Models/
│   └── CustomerDto.cs                 # Common DTO for all parser types
├── Parsing/
│   ├── Interfaces/
│   │   └── ICsvParser.cs              # Parser interface
│   ├── Implementations/
│   │   ├── BaseCsvParser.cs           # Abstract base parser
│   │   ├── TypeAParser.cs             # Type A CSV parser
│   │   ├── TypeBParser.cs             # Type B CSV parser
│   │   └── TypeCParser.cs             # Type C CSV parser
│   └── Factories/
│       └── CsvParserFactory.cs        # Factory for creating parsers
├── Validation/
│   ├── Interfaces/
│   │   └── IValidator.cs              # Validation interface
│   ├── Implementations/
│   │   └── CustomerValidator.cs       # Customer data validation
│   └── Models/
│       └── ValidationResult.cs        # Validation result model
├── Transformation/
│   ├── Interfaces/
│   │   └── ITransformer.cs            # Transformation interface
│   └── Implementations/
│       └── CustomerTransformer.cs     # Data transformation logic
├── ErrorHandling/
│   ├── Interfaces/
│   │   ├── IErrorHandler.cs           # Error handling interface
│   │   ├── ErrorType.cs               # Error type enumeration
│   │   └── ParsingError.cs            # Error model
│   └── Implementations/
│       └── CsvErrorHandler.cs         # Error handling implementation
└── Shared/
    └── Constants/                      # Shared constants organized by category
        ├── FieldNames.cs              # Field name constants
        ├── ValidationConstants.cs     # Validation rules
        ├── ErrorMessages.cs           # Error message templates
        ├── ParserTypes.cs             # Parser type constants
        └── ...                        # Other constant categories
```

## 🎯 Design Patterns Used

### 1. **Factory Pattern**
- `CsvParserFactory` creates appropriate parser instances based on type
- Allows easy registration of new parser types
- Centralized parser creation logic

### 2. **Strategy Pattern**
- Different parsers implement the same interface
- Runtime selection of parsing strategy
- Easy to add new parsing strategies

### 3. **Template Method Pattern**
- `BaseCsvParser` defines the parsing algorithm structure
- Subclasses override specific steps (validation, transformation)
- Common parsing logic shared across all parsers

### 4. **Builder Pattern**
- `CustomerTransformer` builds `CustomerDto` objects
- Handles different input formats and field mappings
- Supports external data enrichment

## 🔧 Key Components

### Parser Types

#### Type A Parser
- **Format**: `CustomerID, Full Name, Email, Phone, Salary`
- **Features**: Simple validation, basic transformation
- **Use Case**: Standard customer data

#### Type B Parser
- **Format**: `ID, Name, Surname, CorporateEmail, PersonalEmail, Salary`
- **Features**: Email priority logic, external data enrichment
- **Use Case**: Corporate employee data

#### Type C Parser
- **Format**: `EmployeeId, FirstName, LastName, WorkEmail, Phone, AnnualSalary, Department`
- **Features**: Department metadata, salary validation
- **Use Case**: Detailed employee records

### Validation System

- **Single Cell Validation**: Validates individual field values
- **Multi-Cell Validation**: Validates relationships between fields
- **DTO Validation**: Validates the final transformed object
- **Customizable Rules**: Easy to add new validation rules

### Transformation System

- **Field Mapping**: Maps different field names to common DTO
- **Data Normalization**: Standardizes formats (phone, email, names)
- **External Enrichment**: Fetches missing data from external sources
- **Metadata Collection**: Preserves original field information

### Error Handling

- **Comprehensive Tracking**: Records all errors with context
- **Error Categorization**: Validation, parsing, transformation, exception types
- **Detailed Reporting**: Provides formatted error reports
- **Graceful Degradation**: Continues processing despite individual errors

## 🚀 Code Quality Features

### Modern C# Features
- **Records**: Immutable DTOs with value-based equality
- **Primary Constructors**: Reduced boilerplate code
- **Nullable Reference Types**: Compile-time null safety
- **Pattern Matching**: Expressive conditional logic
- **Collection Expressions**: Concise collection initialization

### Performance Optimizations
- **TryGetValue**: Single dictionary lookup instead of ContainsKey + indexer
- **Readonly Properties**: Immutability where appropriate
- **Efficient Collections**: Appropriate collection types for use cases

### Maintainability
- **Modular Design**: Clear separation of concerns
- **Shared Constants**: No magic strings or numbers
- **Consistent Naming**: Follows C# conventions
- **Comprehensive Error Handling**: Robust error management

## 🔄 Extension Points

### Adding a New Parser Type

1. **Create Parser Implementation**
   ```csharp
   public class TypeDParser : BaseCsvParser<CustomerDto>
   {
       public override string GetParserType() => ParserTypes.TypeD;
       
       protected override async Task<ValidationResult> ValidateRowDataAsync(
           Dictionary<string, string> rowData, int rowNumber)
       {
           // Custom validation logic
       }
   }
   ```

2. **Register with Factory**
   ```csharp
   factory.RegisterParserType(ParserTypes.TypeD, 
       (validator, transformer, errorHandler) => 
           new TypeDParser(validator, transformer, errorHandler));
   ```

3. **Add Constants**
   ```csharp
   public static class ParserTypes
   {
       public const string TypeD = "TypeD";
   }
   ```

### Adding New Validation Rules

1. **Extend Validator**
   ```csharp
   private void ValidateCustomField(string value, ValidationResult result)
   {
       // Custom validation logic
   }
   ```

2. **Add to Field Groups**
   ```csharp
   private static readonly HashSet<string> CustomFields = new([
       "CustomField1", "CustomField2"
   ]);
   ```

## 📊 Current State

### ✅ Completed Features
- [x] Modular architecture with clear separation of concerns
- [x] Three parser types (TypeA, TypeB, TypeC)
- [x] Comprehensive validation system
- [x] Flexible transformation system
- [x] Robust error handling and reporting
- [x] Modern C# features and best practices
- [x] Shared constants (no magic strings)
- [x] Performance optimizations
- [x] Nullable reference types
- [x] Immutable DTOs using records
- [x] Primary constructors for reduced boilerplate

### 🎯 Design Decisions Made

1. **Generic Interfaces**: `IValidator<T>` and `ITransformer<T>` for reusability
2. **Factory Pattern**: Centralized parser creation with registration support
3. **Abstract Base Class**: Shared parsing logic with template method pattern
4. **Immutable DTOs**: Records provide value-based equality and immutability
5. **Comprehensive Error Handling**: Detailed error tracking with categorization
6. **Shared Constants**: Organized constants prevent magic strings/numbers
7. **Modern C# Features**: Leverages latest language features for cleaner code

### 🔮 Future Enhancements

- [ ] Unit and integration tests
- [ ] Configuration externalization
- [ ] Structured logging
- [ ] Stream processing for large files
- [ ] Parallel processing capabilities
- [ ] Web API interface
- [ ] Plugin architecture for dynamic parser loading
- [ ] Performance benchmarking
- [ ] Documentation generation

## 🤝 Contributing

This project demonstrates clean architecture principles and modern C# development practices. The modular design makes it easy to extend and maintain, while the comprehensive error handling ensures robustness in production environments.

For questions or contributions, please refer to the project structure and extension points documented above.