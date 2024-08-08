namespace MortgageCalculation
{

    public class LoanDetails
    {
        private decimal _loanAmount;
        private decimal _annualInterestRate;
        private int _loanTermInYears;

        public decimal LoanAmount => _loanAmount;
        public decimal AnnualInterestRate => _annualInterestRate;
        public int LoanTermInYears => _loanTermInYears;

        public LoanDetails(decimal loanAmount, decimal annualInterestRate, int loanTermInYears)
        {
            if (loanAmount <= 0)
                throw new ArgumentException("Loan amount must be greater than zero.");
            if (annualInterestRate < 0)
                throw new ArgumentException("Annual interest rate cannot be negative.");
            if (loanTermInYears <= 0)
                throw new ArgumentException("Loan term in years must be greater than zero.");

            _loanAmount = loanAmount;
            _annualInterestRate = annualInterestRate;
            _loanTermInYears = loanTermInYears;
        }
    }
    public class MortgageCalculator
    {
        private readonly LoanDetails _loanDetails;

        public MortgageCalculator(LoanDetails loanDetails)
        {
            _loanDetails = loanDetails;
        }

        public decimal CalculateMonthlyPayment()
        {
            decimal monthlyRate = _loanDetails.AnnualInterestRate / 1200m;
            int numberOfMonths = _loanDetails.LoanTermInYears * 12;
            return _loanDetails.LoanAmount * (monthlyRate / (1 - (decimal)Math.Pow(1 + (double)monthlyRate, -numberOfMonths)));
        }

        public decimal CalculateInterestPayment(decimal remainingBalance)
        {
            if (remainingBalance < 0)
                throw new ArgumentException("Remaining balance cannot be negative.");

            decimal monthlyRate = _loanDetails.AnnualInterestRate / 1200m;
            return remainingBalance * monthlyRate;
        }

        public decimal CalculatePrincipalPayment(decimal totalMonthlyPayment, decimal interestPayment)
        {
            if (totalMonthlyPayment < 0 || interestPayment < 0)
                throw new ArgumentException("Total monthly payment and interest payment cannot be negative.");

            return totalMonthlyPayment - interestPayment;
        }

        public List<AmortizationScheduleEntry> CalculateAmortizationSchedule()
        {
            var schedule = new List<AmortizationScheduleEntry>();
            decimal remainingBalance = _loanDetails.LoanAmount;
            decimal totalMonthlyPayment = CalculateMonthlyPayment();

            for (int month = 1; month <= _loanDetails.LoanTermInYears * 12; month++)
            {
                decimal interestPayment = CalculateInterestPayment(remainingBalance);
                decimal principalPayment = CalculatePrincipalPayment(totalMonthlyPayment, interestPayment);

                if (remainingBalance < 0) remainingBalance = 0;

                remainingBalance -= principalPayment;

                schedule.Add(new AmortizationScheduleEntry(month, totalMonthlyPayment, interestPayment, principalPayment, remainingBalance));
            }

            return schedule;
        }

        public void PrintAmortizationSchedule()
        {
            var schedule = CalculateAmortizationSchedule();

            Console.WriteLine("Month\tTotal Payment\tInterest Payment\tPrincipal Payment\tRemaining Balance");
            foreach (var entry in schedule)
            {
                Console.WriteLine($"{entry.Month}\t{entry.TotalPayment:C}\t{entry.InterestPayment:C}\t{entry.PrincipalPayment:C}\t{entry.RemainingBalance:C}");
            }
        }
    }
    public class AmortizationScheduleEntry
    {
        public int Month { get; }
        public decimal TotalPayment { get; }
        public decimal InterestPayment { get; }
        public decimal PrincipalPayment { get; }
        public decimal RemainingBalance { get; }
        public AmortizationScheduleEntry(int month, decimal totalPayment, decimal interestPayment, decimal principalPayment, decimal remainingBalance)
        {
            Month = month;
            TotalPayment = totalPayment;
            InterestPayment = interestPayment;
            PrincipalPayment = principalPayment;
            RemainingBalance = remainingBalance;
        }
    }


}
