
using MortgageCalculation;

namespace MortgageCalculationTests
{
    [TestFixture]
    public class MortgageCalculatorTests
    {
        private LoanDetails _loanDetails;
        private MortgageCalculator _mortgageCalculator;

        [SetUp]
        public void Setup()
        {
            _loanDetails = new LoanDetails(300000m, 3.5m, 30);
            _mortgageCalculator = new MortgageCalculator(_loanDetails);
        }

        [Test]
        public void LoanDetailsPropertiesTest()
        {
            decimal loanAmount = 100000m;
            decimal annualInterestRate = 3.75m;
            int loanTermInYears = 30;

            var loanDetails = new LoanDetails(loanAmount, annualInterestRate, loanTermInYears);

            Assert.AreEqual((double)loanAmount, (double)loanDetails.LoanAmount);
            Assert.AreEqual((double)annualInterestRate, (double)loanDetails.AnnualInterestRate);
            Assert.AreEqual(loanTermInYears, loanDetails.LoanTermInYears);
        }

        [Test]
        public void CalculateMonthlyPayment()
        {
            decimal expectedMonthlyPayment = 1347.13m;
            decimal actualMonthlyPayment = _mortgageCalculator.CalculateMonthlyPayment();
            Assert.AreEqual((double)expectedMonthlyPayment, (double)actualMonthlyPayment, 0.01);
        }

        [Test]
        public void CalculateInterestPayment()
        {
            decimal remainingBalance = 300000m;
            decimal expectedInterestPayment = 875.00m;
            decimal actualInterestPayment = _mortgageCalculator.CalculateInterestPayment(remainingBalance);
            Assert.AreEqual((double)expectedInterestPayment, (double)actualInterestPayment, 0.01);
        }

        [Test]
        public void CalculatePrincipalPayment()
        {
            decimal totalMonthlyPayment = 1347.13m;
            decimal interestPayment = 875.00m;
            decimal expectedPrincipalPayment = 472.13m;
            decimal actualPrincipalPayment = _mortgageCalculator.CalculatePrincipalPayment(totalMonthlyPayment, interestPayment);
            Assert.AreEqual((double)expectedPrincipalPayment, (double)actualPrincipalPayment, 0.01);
        }

        [Test]
        public void CalculateAmortizationSchedule()
        {
            List<AmortizationScheduleEntry> schedule = _mortgageCalculator.CalculateAmortizationSchedule();
            Console.WriteLine("Month\tTotal Payment\tInterest Payment\tPrincipal Payment\tRemaining Balance");
            foreach (var entry in schedule)
            {
                Console.WriteLine($"{entry.Month}\t{entry.TotalPayment:C}\t{entry.InterestPayment:C}\t{entry.PrincipalPayment:C}\t{entry.RemainingBalance:C}");
            }
            Assert.AreEqual(360, schedule.Count);

            var firstMonth = schedule[0];
            Assert.AreEqual(1, firstMonth.Month);
            Assert.AreEqual((double)1347.13m, (double)firstMonth.TotalPayment, 0.01);
            Assert.AreEqual((double)875.00m, (double)firstMonth.InterestPayment, 0.01);
            Assert.AreEqual((double)472.13m, (double)firstMonth.PrincipalPayment, 0.01);
            Assert.AreEqual((double)299527.87m, (double)firstMonth.RemainingBalance, 0.01);

            var lastMonth = schedule[359];
            Assert.AreEqual(360, lastMonth.Month);
            Assert.AreEqual((double)1347.13m, (double)lastMonth.TotalPayment, 0.01);
            Assert.AreEqual((double)3.92m, (double)lastMonth.InterestPayment, 0.01);
            Assert.AreEqual((double)1343.21m, (double)lastMonth.PrincipalPayment, 0.01);
            Assert.AreEqual((double)0.00m, (double)lastMonth.RemainingBalance, 0.01);
        }

        [Test]
        public void LoanDetailsInvalidInputs()
        {

            Assert.Throws<ArgumentException>(() => new LoanDetails(-300000m, 3.5m, 30));

            Assert.Throws<ArgumentException>(() => new LoanDetails(300000m, -3.5m, 30));

            Assert.Throws<ArgumentException>(() => new LoanDetails(300000m, 3.5m, -30));
        }

        [Test]
        public void CalculateInterestPaymentInvalidInput()
        {
            var validLoanDetails = new LoanDetails(300000m, 3.5m, 30);
            var mortgageCalculator = new MortgageCalculator(validLoanDetails);
            Assert.Throws<ArgumentException>(() => mortgageCalculator.CalculateInterestPayment(-300000m));
        }
    }
}
