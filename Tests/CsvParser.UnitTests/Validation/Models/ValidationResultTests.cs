using CsvParser.Validation.Models;

namespace CsvParser.UnitTests.Validation.Models;

public class ValidationResultTests
{
    [Fact]
    public void GivenConstructor_WhenCalledWithAllParameters_ThenShouldSetInitialValues()
    {
        var result = new ValidationResult(true, 1, 2);
        Assert.True(result.IsValid);
        Assert.Equal(1, result.RowNumber);
        Assert.Equal(2, result.ColumnNumber);
        Assert.Empty(result.Errors);
    }

    [Fact]
    public void GivenConstructor_WhenCalledWithNullColumnNumber_ThenShouldHandleNullColumnNumber()
    {
        var result = new ValidationResult(false, 1);
        Assert.False(result.IsValid);
        Assert.Equal(1, result.RowNumber);
        Assert.Null(result.ColumnNumber);
        Assert.Empty(result.Errors);
    }

    [Fact]
    public void GivenAddError_WhenCalled_ThenShouldAddErrorAndSetIsValidToFalse()
    {
        var result = new ValidationResult(true, 1);
        result.AddError("Test error message");
        Assert.False(result.IsValid);
        Assert.Single(result.Errors);
        Assert.Contains("Test error message", result.Errors);
    }

    [Fact]
    public void GivenAddError_WhenCalledMultipleTimes_ThenShouldAddMultipleErrors()
    {
        var result = new ValidationResult(true, 1);
        result.AddError("First error");
        result.AddError("Second error");
        result.AddError("Third error");
        Assert.False(result.IsValid);
        Assert.Equal(3, result.Errors.Count);
        Assert.Contains("First error", result.Errors);
        Assert.Contains("Second error", result.Errors);
        Assert.Contains("Third error", result.Errors);
    }

    [Fact]
    public void GivenAddError_WhenCalled_ThenShouldNotChangeRowAndColumnNumbers()
    {
        var result = new ValidationResult(true, 5, 3);
        result.AddError("Test error");
        Assert.Equal(5, result.RowNumber);
        Assert.Equal(3, result.ColumnNumber);
    }

    [Fact]
    public void GivenConstructor_WhenCalledWithIsValidTrue_ThenIsValidShouldStartAsTrue()
    {
        var result = new ValidationResult(true, 1);
        Assert.True(result.IsValid);
    }

    [Fact]
    public void GivenConstructor_WhenCalledWithIsValidFalse_ThenIsValidShouldStartAsFalse()
    {
        var result = new ValidationResult(false, 1);
        Assert.False(result.IsValid);
    }
}