using CsvParser.ErrorHandling.Interfaces;
using CsvParser.Models;
using CsvParser.Parsing.Implementations;
using CsvParser.Parsing.Interfaces;
using CsvParser.Shared.Constants;
using CsvParser.Transformation.Interfaces;
using CsvParser.Validation.Interfaces;

namespace CsvParser.Parsing.Factories;

/// <summary>
/// Factory class for creating different types of CSV parsers based on the parser type.
/// Implements the Factory pattern to provide a centralized way to create parser instances.
/// </summary>
public class CsvParserFactory
{
    private readonly Dictionary<string, Func<IValidator<CustomerDto>, ITransformer<CustomerDto>, IErrorHandler, ICsvParser<CustomerDto>>> _parserCreators;

    public CsvParserFactory()
    {
        _parserCreators = new Dictionary<string, Func<IValidator<CustomerDto>, ITransformer<CustomerDto>, IErrorHandler, ICsvParser<CustomerDto>>>
        {
            { ParserTypes.TypeA, (validator, transformer, errorHandler) => new TypeAParser(validator, transformer, errorHandler) },
            { ParserTypes.TypeB, (validator, transformer, errorHandler) => new TypeBParser(validator, transformer, errorHandler) },
            { ParserTypes.TypeC, (validator, transformer, errorHandler) => new TypeCParser(validator, transformer, errorHandler) }
        };
    }

    /// <summary>
    /// Creates a CSV parser instance based on the specified parser type
    /// </summary>
    /// <param name="parserType">Type of parser to create (e.g., "TypeA", "TypeB")</param>
    /// <param name="validator">Validator instance to use for data validation</param>
    /// <param name="transformer">Transformer instance to use for data transformation</param>
    /// <param name="errorHandler">Error handler instance to use for error management</param>
    /// <returns>Configured CSV parser instance</returns>
    /// <exception cref="ArgumentException">Thrown when the parser type is not supported</exception>
    public ICsvParser<CustomerDto> CreateParser(string parserType, IValidator<CustomerDto> validator, ITransformer<CustomerDto> transformer, IErrorHandler errorHandler)
    {
        if (string.IsNullOrWhiteSpace(parserType))
        {
            throw new ArgumentException(ErrorMessages.ParserTypeNullOrEmpty, nameof(parserType));
        }

        if (!_parserCreators.TryGetValue(parserType, out var value))
        {
            var supportedTypes = string.Join(", ", _parserCreators.Keys);
            throw new ArgumentException(string.Format(ErrorMessages.UnsupportedParserType, parserType, supportedTypes), nameof(parserType));
        }

        return value(validator, transformer, errorHandler);
    }

    /// <summary>
    /// Gets all supported parser types
    /// </summary>
    /// <returns>Collection of supported parser type names</returns>
    public IEnumerable<string> GetSupportedParserTypes()
    {
        return _parserCreators.Keys;
    }

    /// <summary>
    /// Checks if a parser type is supported
    /// </summary>
    /// <param name="parserType">Parser type to check</param>
    /// <returns>True if the parser type is supported, false otherwise</returns>
    public bool IsParserTypeSupported(string parserType)
    {
        return _parserCreators.TryGetValue(parserType, out _);
    }

    /// <summary>
    /// Registers a new parser type with the factory
    /// </summary>
    /// <param name="parserType">Name of the parser type</param>
    /// <param name="creator">Function to create the parser instance</param>
    public void RegisterParserType(string parserType, Func<IValidator<CustomerDto>, ITransformer<CustomerDto>, IErrorHandler, ICsvParser<CustomerDto>> creator)
    {
        if (string.IsNullOrWhiteSpace(parserType))
        {
            throw new ArgumentException(ErrorMessages.ParserTypeNullOrEmpty, nameof(parserType));
        }

        ArgumentNullException.ThrowIfNull(creator);

        _parserCreators[parserType] = creator;
    }
}