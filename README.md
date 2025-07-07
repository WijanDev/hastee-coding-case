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

#### 1. **Generic Interfaces**: `IValidator<T>` and `ITransformer<T>`
**Decision**: Use generic interfaces instead of concrete types
**Arguments**:
- **Reusability**: Can be implemented for different DTOs (CustomerDto, ProductDto, etc.)
- **Type Safety**: Compile-time type checking prevents runtime errors
- **Extensibility**: Easy to add new entity types without changing interface contracts
- **Testability**: Can mock interfaces for unit testing
- **Dependency Inversion**: Depends on abstractions, not concrete implementations

#### 2. **Factory Pattern**: Centralized parser creation
**Decision**: Use factory pattern for parser instantiation
**Arguments**:
- **Encapsulation**: Hides complex object creation logic
- **Flexibility**: Easy to add new parser types without modifying existing code
- **Registration System**: Runtime registration of new parsers
- **Dependency Injection**: Centralized dependency management
- **Configuration**: Can configure parsers based on external settings

#### 3. **Abstract Base Class**: Template method pattern
**Decision**: Use abstract base class instead of pure interface implementation
**Arguments**:
- **Code Reuse**: Common parsing logic shared across all parsers
- **Consistency**: Enforces consistent parsing workflow
- **Reduced Duplication**: Template method eliminates repetitive code
- **Extensibility**: Subclasses can override specific steps while keeping structure
- **Maintainability**: Changes to common logic only need to be made in one place

#### 4. **Immutable DTOs**: Records instead of classes
**Decision**: Use C# records for CustomerDto
**Arguments**:
- **Immutability**: Prevents accidental data modification after creation
- **Value Semantics**: Natural equality comparison without custom implementation
- **Thread Safety**: Immutable objects are inherently thread-safe
- **Functional Programming**: Aligns with functional programming principles
- **Performance**: Optimized for value-based scenarios
- **Conciseness**: Less boilerplate code compared to traditional classes

#### 5. **Comprehensive Error Handling**: Detailed error tracking
**Decision**: Implement extensive error handling with categorization
**Arguments**:
- **Debugging**: Detailed error information helps identify issues quickly
- **User Experience**: Clear error messages improve usability
- **Graceful Degradation**: System continues processing despite individual errors
- **Audit Trail**: Complete error history for compliance and monitoring
- **Error Recovery**: Categorized errors enable targeted error handling strategies
- **Reporting**: Structured error data supports automated reporting

#### 6. **Shared Constants**: Organized constant management
**Decision**: Extract all magic strings/numbers into organized constant files
**Arguments**:
- **Maintainability**: Single source of truth for all constants
- **Refactoring Safety**: IDE can safely rename constants across the codebase
- **Consistency**: Ensures consistent values across the application
- **Documentation**: Constants serve as self-documenting code
- **Localization**: Easy to replace constants with localized versions
- **Type Safety**: Compile-time checking prevents typos

#### 7. **Modern C# Features**: Latest language features
**Decision**: Leverage C# 9+ features throughout the codebase
**Arguments**:
- **Performance**: Features like records and primary constructors are optimized
- **Readability**: More expressive and concise code
- **Safety**: Nullable reference types prevent null reference exceptions
- **Productivity**: Less boilerplate code means faster development
- **Future-Proof**: Aligns with Microsoft's direction for the language
- **Best Practices**: Demonstrates modern C# development patterns

#### 8. **Modular Architecture**: Separation by responsibility
**Decision**: Organize code into modules (Parsing, Validation, Transformation, ErrorHandling)
**Arguments**:
- **Single Responsibility**: Each module has a clear, focused purpose
- **Loose Coupling**: Modules can evolve independently
- **High Cohesion**: Related functionality is grouped together
- **Testability**: Each module can be tested in isolation
- **Team Development**: Different developers can work on different modules
- **Reusability**: Modules can be reused in other projects

#### 9. **TryGetValue Pattern**: Dictionary access optimization
**Decision**: Use TryGetValue instead of ContainsKey + indexer
**Arguments**:
- **Performance**: Single dictionary lookup instead of two
- **Atomicity**: Eliminates race conditions in concurrent scenarios
- **Cleaner Code**: More idiomatic and readable
- **Error Prevention**: Avoids potential KeyNotFoundException
- **Best Practice**: Recommended approach in C# documentation

#### 10. **Primary Constructors**: Reduced boilerplate
**Decision**: Use primary constructors for simple data containers
**Arguments**:
- **Conciseness**: Significantly reduces boilerplate code
- **Readability**: Constructor parameters are immediately visible
- **Immutability**: Natural fit for immutable objects
- **Performance**: Optimized by the compiler
- **Modern Approach**: Aligns with C# evolution toward more functional patterns

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