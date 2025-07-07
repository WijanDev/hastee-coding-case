using CsvParser.Models;

namespace CsvParser.UnitTests.Models;

public class CustomerDtoTests
{
    [Fact]
    public void GivenCustomerDto_WhenCreated_ThenShouldBeImmutable()
    {
        var metadata = new Dictionary<string, object> { ["key"] = "value" };
        var customer = new CustomerDto(
            "CUST001",
            "John Doe",
            "john.doe@email.com",
            "john.personal@email.com",
            "+1234567890",
            75000m,
            metadata);
        Assert.Equal("CUST001", customer.Id);
        Assert.Equal("John Doe", customer.FullName);
        Assert.Equal("john.doe@email.com", customer.Email);
        Assert.Equal("john.personal@email.com", customer.SecondaryEmail);
        Assert.Equal("+1234567890", customer.Phone);
        Assert.Equal(75000m, customer.Salary);
        Assert.Same(metadata, customer.Metadata);
    }

    [Fact]
    public void GivenCustomerDto_WhenComparedWithSameValues_ThenShouldHaveValueBasedEquality()
    {
        var metadata = new Dictionary<string, object> { ["key"] = "value" };
        var customer1 = new CustomerDto(
            "CUST001",
            "John Doe",
            "john.doe@email.com",
            "john.personal@email.com",
            "+1234567890",
            75000m,
            metadata);
        var customer2 = new CustomerDto(
            "CUST001",
            "John Doe",
            "john.doe@email.com",
            "john.personal@email.com",
            "+1234567890",
            75000m,
            metadata);
        Assert.Equal(customer1, customer2);
        Assert.True(customer1.Equals(customer2));
        Assert.True(customer1 == customer2);
        Assert.False(customer1 != customer2);
    }

    [Fact]
    public void GivenCustomerDto_WhenDeconstructed_ThenShouldSupportDeconstruction()
    {
        var customer = new CustomerDto(
            "CUST001",
            "John Doe",
            "john.doe@email.com",
            "john.personal@email.com",
            "+1234567890",
            75000m,
            new Dictionary<string, object>());
        var (id, fullName, email, secondaryEmail, phone, salary, metadata) = customer;
        Assert.Equal("CUST001", id);
        Assert.Equal("John Doe", fullName);
        Assert.Equal("john.doe@email.com", email);
        Assert.Equal("john.personal@email.com", secondaryEmail);
        Assert.Equal("+1234567890", phone);
        Assert.Equal(75000m, salary);
        Assert.NotNull(metadata);
    }

    [Fact]
    public void GivenCustomerDto_WhenOptionalValuesAreNull_ThenShouldHandleNulls()
    {
        var customer = new CustomerDto(
            "CUST001",
            "John Doe",
            "john.doe@email.com",
            null,
            null,
            75000m,
            new Dictionary<string, object>());
        Assert.Null(customer.SecondaryEmail);
        Assert.Null(customer.Phone);
        Assert.Equal("CUST001", customer.Id);
        Assert.Equal("John Doe", customer.FullName);
        Assert.Equal("john.doe@email.com", customer.Email);
        Assert.Equal(75000m, customer.Salary);
    }

    [Fact]
    public void GivenCustomerDto_WhenWithExpressionUsed_ThenShouldSupportWithExpression()
    {
        var originalCustomer = new CustomerDto(
            "CUST001",
            "John Doe",
            "john.doe@email.com",
            null,
            null,
            75000m,
            new Dictionary<string, object>());
        var updatedCustomer = originalCustomer with
        {
            Salary = 80000m,
            SecondaryEmail = "john.new@email.com"
        };
        Assert.Equal("CUST001", updatedCustomer.Id);
        Assert.Equal("John Doe", updatedCustomer.FullName);
        Assert.Equal("john.doe@email.com", updatedCustomer.Email);
        Assert.Equal("john.new@email.com", updatedCustomer.SecondaryEmail);
        Assert.Null(updatedCustomer.Phone);
        Assert.Equal(80000m, updatedCustomer.Salary);
        Assert.Equal(75000m, originalCustomer.Salary);
        Assert.Null(originalCustomer.SecondaryEmail);
    }
}
