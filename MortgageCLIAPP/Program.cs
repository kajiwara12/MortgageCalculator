using MortgageCalculation;
using System;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.CommandLine.NamingConventionBinder;
namespace MortgageCLIApp
{
    public class Program
    {
        public static int Main(string[] args)
        {
            // Define the command line options
            var loanAmountOption = new Option<decimal>(
                new[] { "-l", "--loan-amount" },
                "The loan amount");

            var annualInterestRateOption = new Option<decimal>(
                new[] { "-i", "--annual-interest-rate" },
                "The annual interest rate");

            var loanTermInYearsOption = new Option<int>(
                new[] { "-t", "--loan-term-in-years" },
                "The loan term in years");

            // Create the root command
            var rootCommand = new RootCommand
            {
                loanAmountOption,
                annualInterestRateOption,
                loanTermInYearsOption
            };

            rootCommand.Description = "Mortgage Calculator CLI";

            // Set the handler for the command
            rootCommand.Handler = CommandHandler.Create<decimal, decimal, int>((loanAmount, annualInterestRate, loanTermInYears) =>
            {
                try
                {
                    // Create LoanDetails instance
                    var loanDetails = new LoanDetails(loanAmount, annualInterestRate, loanTermInYears);

                    // Create MortgageCalculator instance
                    var mortgageCalculator = new MortgageCalculator(loanDetails);

                    // Calculate Monthly Payment
                    var monthlyPayment = mortgageCalculator.CalculateMonthlyPayment();
                    Console.WriteLine($"Monthly Payment: {monthlyPayment:C}");

                    return 0; 
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                    return 1;
                }
            });

            return rootCommand.Invoke(args);
        }
    }
}
