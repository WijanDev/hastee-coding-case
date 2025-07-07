using CsvParser.ErrorHandling.Models;

namespace CsvParser.UnitTests.ErrorHandling.Models;

public class ParsingErrorTests
{
    [Fact]
    public void GivenConstructor_WhenCalledWithAllParameters_ThenShouldSetRequiredProperties()
    {
        var error = new ParsingError(1, 2, "Test error message", ErrorType.Validation);
        Assert.Equal(1, error.RowNumber);
        Assert.Equal(2, error.ColumnNumber);
        Assert.Equal("Test error message", error.ErrorMessage);
        Assert.Equal(ErrorType.Validation, error.ErrorType);
    }

    [Fact]
    public void GivenConstructor_WhenCalledWithNullColumnNumber_ThenShouldHandleNullColumnNumber()
    {
        var error = new ParsingError(1, null, "Test error message", ErrorType.Parsing);
        Assert.Equal(1, error.RowNumber);
        Assert.Null(error.ColumnNumber);
        Assert.Equal("Test error message", error.ErrorMessage);
        Assert.Equal(ErrorType.Parsing, error.ErrorType);
    }

    [Fact]
    public void GivenOptionalProperties_WhenSet_ThenShouldBeInitializedCorrectly()
    {
        var error = new ParsingError(1, 2, "Test error", ErrorType.Validation)
        {
            ExpectedValue = "expected",
            ActualValue = "actual",
            FieldName = "field",
            Context = "context"
        };
        Assert.Equal("expected", error.ExpectedValue);
        Assert.Equal("actual", error.ActualValue);
        Assert.Equal("field", error.FieldName);
        Assert.Equal("context", error.Context);
        Assert.NotEqual(default(DateTime), error.Timestamp);
    }

    [Fact]
    public void GivenConstructor_WhenCalled_ThenTimestampShouldBeSetToUtcNow()
    {
        var beforeCreation = DateTime.UtcNow;
        var error = new ParsingError(1, 2, "Test error", ErrorType.Validation);
        var afterCreation = DateTime.UtcNow;
        Assert.True(error.Timestamp >= beforeCreation);
        Assert.True(error.Timestamp <= afterCreation);
    }

    [Fact]
    public void GivenToString_WhenCalled_ThenShouldFormatCorrectly()
    {
        var error = new ParsingError(5, 3, "Invalid email format", ErrorType.Validation);
        var result = error.ToString();
        Assert.Equal("[Validation] Row 5, Column 3: Invalid email format", result);
    }

    [Fact]
    public void GivenToString_WhenCalledWithNullColumnNumber_ThenShouldHandleNullColumnNumber()
    {
        var error = new ParsingError(5, null, "General error", ErrorType.Exception);
        var result = error.ToString();
        Assert.Equal("[Exception] Row 5: General error", result);
    }

    [Theory]
    [InlineData(ErrorType.Validation)]
    [InlineData(ErrorType.Parsing)]
    [InlineData(ErrorType.Transformation)]
    [InlineData(ErrorType.Exception)]
    public void GivenConstructor_WhenCalledWithAllErrorTypes_ThenShouldSupportAllErrorTypes(ErrorType errorType)
    {
        var error = new ParsingError(1, 1, "Test error", errorType);
        Assert.Equal(errorType, error.ErrorType);
    }
}