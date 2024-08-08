using MortgageCalculation;
using Spectre.Console;

namespace OkamiMortgageConsole
{
    internal class Program
    {
        //create a Table thatll display 25 entries of the amortization schdeule
        public void DisplayPage(Table table, List<AmortizationScheduleEntry> schedule, int pageIndex, int pageSize)
        {
            table.Rows.Clear(); 

            int startIndex = pageIndex * pageSize;
            int endIndex = Math.Min(startIndex + pageSize, schedule.Count);

            for (int i = startIndex; i < endIndex; i++)
            {
                var entry = schedule[i];
                table.AddRow(
                    entry.Month.ToString(),
                    entry.TotalPayment.ToString("C"),
                    entry.InterestPayment.ToString("C"),
                    entry.PrincipalPayment.ToString("C"),
                    entry.RemainingBalance.ToString("C"));
            }
            //clear the console before the table gets printed to keep the console clean for users
            AnsiConsole.Clear();
            AnsiConsole.Write(table);
        }

        //create a loop that calls DisplayPage that'll keep the table being displayed unless Exit is selected

        public void PaginateSchedule(List<AmortizationScheduleEntry> schedule)
        {
            int pageSize = 25;
            int pageIndex = 0;
            var table = new Table()
                .Border(TableBorder.Rounded)
                .AddColumn("Month")
                .AddColumn("Total Payment")
                .AddColumn("Interest Payment")
                .AddColumn("Principal Payment")
                .AddColumn("Remaining Balance");
            string choice;

            do
            {
            //Create a prompt that gives choices for navigating table
                DisplayPage(table, schedule, pageIndex, pageSize);

                var prompt = new SelectionPrompt<string>()
                    .AddChoices(new[] { "Next", "Back", "Exit" });

                choice = AnsiConsole.Prompt(prompt);
                //verify that you arent hitting next when you shouldnt ie on the last page
                if (choice == "Next" && (pageIndex + 1) * pageSize < schedule.Count)
                {
                    pageIndex++;
                }
                //verify you dont go back beyond the first page
                else if (choice == "Back" && pageIndex > 0)
                {
                    pageIndex--;
                }

            } while (choice != "Exit");
        }

        //created a loop to ensure that the user gives valid inputs, if invalid theyll be prompted to input correct inputs
        public void GetValidInput()
        {
            while (true)
            {
                try
                {
                    // Get user input
                    var loanAmount = AnsiConsole.Ask<decimal>("Enter the [green]Loan Amount[/]:");
                    var annualInterestRate = AnsiConsole.Ask<decimal>("Enter the [green]Annual Interest Rate[/](e.g., 3.5 for 3.5%):");
                    var loanTermInYears = AnsiConsole.Ask<int>("Enter the [green]Loan Term in Years[/]:");
                    //use input to create loanDetails instance
                    var loanDetails = new LoanDetails(loanAmount, annualInterestRate, loanTermInYears);

                    // Create MortgageCalculator instance with LoanDetails
                    var mortgageCalculator = new MortgageCalculator(loanDetails);

                    // Calculate/create Amortization Schedule
                    var schedule = mortgageCalculator.CalculateAmortizationSchedule();

                    // Paginate schedule
                    PaginateSchedule(schedule);

                    // Exit after successful processing
                    break;
                }
                catch (ArgumentException ex)
                {
                    AnsiConsole.MarkupLine($"[red]Error:[/] {ex.Message}");
                    AnsiConsole.MarkupLine("Please try again.");
                }
            }
        }
        static void Main(string[] args)
        {
            AnsiConsole.Write(
              new FigletText("Welcome To The Simple Mortgage Calculator")
              .Centered()
              .Color(Color.Green)
            );
             var program = new Program();
                program.GetValidInput();
        }
    }
}
