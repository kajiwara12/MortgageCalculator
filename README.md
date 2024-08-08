# MortgageCalculator-MSSA ffirst c# project

## Table of Contents

- [Project Description](#project-description)
- [Features](#features)
- [Classes](#classes)
- [Usage](#usage)
- [Validation](#validation)
- [Tests](#tests)
- [Requirements](#requirements)
- [Installation](#installation)
- [Acknowledgments](#acknowledgments)

## Project Description

This C# project is a console application that calculates monthly mortgage payments based on the loan amount, annual interest rate, and loan term in years. It also generates an amortization schedule detailing the breakdown of each payment into principal and interest components.

## Features

- Calculate monthly mortgage payments.
- Generate and print an amortization schedule.
- Validate loan input data to ensure correctness.

## Classes

### `LoanDetails`

Represents the details of the loan.

#### Properties

- `decimal LoanAmount`: The total amount of the loan.
- `decimal AnnualInterestRate`: The annual interest rate of the loan.
- `int LoanTermInYears`: The duration of the loan in years.

#### Constructor

- `LoanDetails(decimal loanAmount, decimal annualInterestRate, int loanTermInYears)`

### `MortgageCalculator`

Calculates mortgage details and generates an amortization schedule.

#### Methods

- `decimal CalculateMonthlyPayment()`: Calculates the monthly mortgage payment.
- `decimal CalculateInterestPayment(decimal remainingBalance)`: Calculates the interest portion of a payment.
- `decimal CalculatePrincipalPayment(decimal totalMonthlyPayment, decimal interestPayment)`: Calculates the principal portion of a payment.
- `List<AmortizationScheduleEntry> CalculateAmortizationSchedule()`: Generates the amortization schedule.
- `void PrintAmortizationSchedule()`: Prints the amortization schedule to the console.

### `AmortizationScheduleEntry`

Represents a single entry in the amortization schedule.

#### Properties

- `int Month`: The month of the payment.
- `decimal TotalPayment`: The total payment amount.
- `decimal InterestPayment`: The interest portion of the payment.
- `decimal PrincipalPayment`: The principal portion of the payment.
- `decimal RemainingBalance`: The remaining loan balance after the payment.

## Usage

To use this application, instantiate the `LoanDetails` and `MortgageCalculator` classes and call the relevant methods.

## Example:
```csharp
var loanDetails = new LoanDetails(300000m, 3.5m, 30);
var mortgageCalculator = new MortgageCalculator(loanDetails);

var monthlyPayment = mortgageCalculator.CalculateMonthlyPayment();
Console.WriteLine($"Monthly Payment: {monthlyPayment:C}");

mortgageCalculator.PrintAmortizationSchedule();
```

## Validation

The application includes validation to ensure that:

- The loan amount is greater than zero.
- The annual interest rate is not negative.
- The loan term in years is greater than zero.
- Remaining balance and payments are not negative.

## Tests

The application includes NUnit tests to verify the correctness of calculations and validations.

### Example test:

```csharp
[Test]
public void LoanDetails_ShouldThrowExceptionForInvalidInputs()
{
    Assert.Throws<ArgumentException>(() => new LoanDetails(-300000m, 3.5m, 30));
    Assert.Throws<ArgumentException>(() => new LoanDetails(300000m, -3.5m, 30));
    Assert.Throws<ArgumentException>(() => new LoanDetails(300000m, 3.5m, -30));
}
```
## Requirements

- .NET SDK
- NUnit for testing

## Installation

1. Clone the repository.
2. Open the solution in your preferred C# IDE (e.g., Visual Studio).
3. Build the solution.
4. Run the application.

## Acknowledgments

This project is part of the Microsoft Software & Systems Academy (MSSA) program. Special thanks to the MSSA instructors and mentors for their guidance and support.

