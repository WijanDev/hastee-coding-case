using CsvParser.ErrorHandling.Implementations;
using CsvParser.ErrorHandling.Models;
using CsvParser.Shared.Constants;
using CsvParser.Validation.Models;

namespace CsvParser.UnitTests.ErrorHandling.Implementations;

public class CsvErrorHandlerTests
{
    private readonly CsvErrorHandler _errorHandler;

    public CsvErrorHandlerTests()
    {
        _errorHandler = new CsvErrorHandler();
    }

    [Fact]
    public void GivenConstructor_WhenCalled_ThenShouldInitializeEmptyErrorList()
    {
        Assert.False(_errorHandler.HasErrors());
        Assert.Equal(0, _errorHandler.GetErrorCount());
        Assert.Empty(_errorHandler.GetAllErrors());
    }

    [Fact]
    public void GivenRecordValidationError_WhenValidationErrorsAdded_ThenShouldAddValidationErrors()
    {
        var validationResult = new ValidationResult(true, 1, 2);
        validationResult.AddError("Validation error 1");
        validationResult.AddError("Validation error 2");
        _errorHandler.RecordValidationError(validationResult);
        Assert.True(_errorHandler.HasErrors());
        Assert.Equal(2, _errorHandler.GetErrorCount());
        var errors = _errorHandler.GetAllErrors();
        Assert.Equal(2, errors.Count);
        Assert.All(errors, error => Assert.Equal(ErrorType.Validation, error.ErrorType));
        Assert.All(errors, error => Assert.Equal(1, error.RowNumber));
        Assert.All(errors, error => Assert.Equal(2, error.ColumnNumber));
    }

    [Fact]
    public void GivenRecordParsingError_WhenCalled_ThenShouldAddParsingError()
    {
        _errorHandler.RecordParsingError(5, 3, "Parsing error", "expected", "actual");
        Assert.True(_errorHandler.HasErrors());
        Assert.Equal(1, _errorHandler.GetErrorCount());
        var errors = _errorHandler.GetAllErrors();
        var error = Assert.Single(errors);
        Assert.Equal(ErrorType.Parsing, error.ErrorType);
        Assert.Equal(5, error.RowNumber);
        Assert.Equal(3, error.ColumnNumber);
        Assert.Equal("Parsing error", error.ErrorMessage);
        Assert.Equal("expected", error.ExpectedValue);
        Assert.Equal("actual", error.ActualValue);
    }

    [Fact]
    public void GivenRecordException_WhenCalled_ThenShouldAddExceptionError()
    {
        var exception = new InvalidOperationException("Test exception");
        _errorHandler.RecordException(10, exception, "Test context");
        Assert.True(_errorHandler.HasErrors());
        Assert.Equal(1, _errorHandler.GetErrorCount());
        var errors = _errorHandler.GetAllErrors();
        var error = Assert.Single(errors);
        Assert.Equal(ErrorType.Exception, error.ErrorType);
        Assert.Equal(10, error.RowNumber);
        Assert.Null(error.ColumnNumber);
        Assert.Contains("Test exception", error.ErrorMessage);
        Assert.Equal("Test context", error.Context);
    }

    [Fact]
    public void GivenRecordTransformationError_WhenCalled_ThenShouldAddTransformationError()
    {
        _errorHandler.RecordTransformationError(7, "Email", "invalid-email", "Invalid email format");
        Assert.True(_errorHandler.HasErrors());
        Assert.Equal(1, _errorHandler.GetErrorCount());
        var errors = _errorHandler.GetAllErrors();
        var error = Assert.Single(errors);
        Assert.Equal(ErrorType.Transformation, error.ErrorType);
        Assert.Equal(7, error.RowNumber);
        Assert.Equal("Email", error.FieldName);
        Assert.Equal("invalid-email", error.ActualValue);
        Assert.Equal("Invalid email format", error.ErrorMessage);
    }

    [Fact]
    public void GivenGetErrorsForRow_WhenCalled_ThenShouldReturnOnlyErrorsForSpecificRow()
    {
        _errorHandler.RecordParsingError(1, 1, "Error in row 1");
        _errorHandler.RecordParsingError(2, 1, "Error in row 2");
        _errorHandler.RecordParsingError(1, 2, "Another error in row 1");
        var row1Errors = _errorHandler.GetErrorsForRow(1);
        var row2Errors = _errorHandler.GetErrorsForRow(2);
        Assert.Equal(2, row1Errors.Count);
        Assert.Single(row2Errors);
        Assert.All(row1Errors, error => Assert.Equal(1, error.RowNumber));
        Assert.All(row2Errors, error => Assert.Equal(2, error.RowNumber));
    }

    [Fact]
    public void GivenClearErrors_WhenCalled_ThenShouldRemoveAllErrors()
    {
        _errorHandler.RecordParsingError(1, 1, "Error 1");
        _errorHandler.RecordParsingError(2, 1, "Error 2");
        Assert.True(_errorHandler.HasErrors());
        _errorHandler.ClearErrors();
        Assert.False(_errorHandler.HasErrors());
        Assert.Equal(0, _errorHandler.GetErrorCount());
        Assert.Empty(_errorHandler.GetAllErrors());
    }

    [Fact]
    public void GivenGetErrorSummary_WhenCalled_ThenShouldGroupErrorsByType()
    {
        var validationResult = new ValidationResult(false, 1);
        validationResult.AddError("Error 1");
        validationResult.AddError("Error 2");
        _errorHandler.RecordValidationError(validationResult);
        _errorHandler.RecordParsingError(2, 1, "Parsing error");
        _errorHandler.RecordException(3, new Exception("Test"), "Context");
        var summary = _errorHandler.GetErrorSummary();
        Assert.Equal(3, summary.Count);
        Assert.Equal(2, summary[ErrorType.Validation]);
        Assert.Equal(1, summary[ErrorType.Parsing]);
        Assert.Equal(1, summary[ErrorType.Exception]);
    }

    [Fact]
    public void GivenGetErrorsByRow_WhenCalled_ThenShouldGroupErrorsByRowNumber()
    {
        _errorHandler.RecordParsingError(1, 1, "Error 1");
        _errorHandler.RecordParsingError(1, 2, "Error 2");
        _errorHandler.RecordParsingError(2, 1, "Error 3");
        var errorsByRow = _errorHandler.GetErrorsByRow();
        Assert.Equal(2, errorsByRow.Count);
        Assert.Equal(2, errorsByRow[1].Count);
        Assert.Single(errorsByRow[2]);
    }

    [Fact]
    public void GivenExportErrorReport_WhenErrorsExist_ThenShouldReturnFormattedReport()
    {
        _errorHandler.RecordParsingError(1, 1, "Test error");
        var report = _errorHandler.ExportErrorReport();
        Assert.Contains("CSV Parsing Error Report", report);
        Assert.Contains("Total Errors: 1", report);
        Assert.Contains("Test error", report);
    }

    [Fact]
    public void GivenExportErrorReport_WhenNoErrorsExist_ThenShouldReturnNoErrorsMessage()
    {
        var report = _errorHandler.ExportErrorReport();
        Assert.Equal(ErrorMessages.NoErrors, report);
    }

    [Fact]
    public void GivenRecordValidationError_WhenNullValidationResult_ThenShouldThrowArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => _errorHandler.RecordValidationError(null!));
    }

    [Fact]
    public void GivenRecordException_WhenNullException_ThenShouldThrowArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => _errorHandler.RecordException(1, null!, "context"));
    }

    [Fact]
    public void GivenRecordParsingError_WhenEmptyErrorMessage_ThenShouldIgnoreError()
    {
        _errorHandler.RecordParsingError(1, 1, string.Empty);
        Assert.False(_errorHandler.HasErrors());
        Assert.Equal(0, _errorHandler.GetErrorCount());
    }

    [Fact]
    public void GivenRecordTransformationError_WhenEmptyErrorMessage_ThenShouldIgnoreError()
    {
        _errorHandler.RecordTransformationError(1, "field", "value", string.Empty);
        Assert.False(_errorHandler.HasErrors());
        Assert.Equal(0, _errorHandler.GetErrorCount());
    }
}