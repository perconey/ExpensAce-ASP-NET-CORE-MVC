using ExpensAce.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Globalization;

namespace ExpensAce.Controllers
{
    public class DashboardController : Controller
    {
        private ApplicationDBContext _dbContext;
        public DashboardController(ApplicationDBContext dbc)
        {
            _dbContext = dbc;
        }

        public async Task<ActionResult> Index()
        {
            DateTime startDate = DateTime.Today.AddDays(-6);
            DateTime endDate = DateTime.Today;
            CultureInfo culture = CultureInfo.CreateSpecificCulture("en-US");

            List<Transaction> last7DaysTransaction = await _dbContext.Transactions
                .Include(t => t.Category)
                .Where(t => t.Date <= endDate && t.Date >= startDate)
                .ToListAsync();

            Int32 Income = last7DaysTransaction
                .Where(t => t.Category?.Type == "Income")
                .Sum(t => t.Amount);

            ViewBag.TotalIncome = String.Format(culture, "{0:C0}", Income);

            Int32 Expense = last7DaysTransaction
                .Where(t => t.Category?.Type == "Expense")
                .Sum(t => t.Amount);

            ViewBag.TotalExpense = String.Format(culture, "{0:C0}", Expense);

            culture.NumberFormat.CurrencyNegativePattern = 1;

            ViewBag.Balance = String.Format(culture, "{0:C0}", Income - Expense);

            ViewBag.DoughnutChartData = last7DaysTransaction
                .Where(i => i.Category?.Type == "Expense")
                .GroupBy(t => t.Category.CategoryId)
                .Select(t => new
                {
                    categoryTitleWithIcon = t.First().CategoryTitleWithIcon,
                    amount = t.Sum(tt => tt.Amount),
                    formattedAmount = String.Format(culture, "{0:C0}", t.Sum(tt => tt.Amount))
                });

            //Income
            List<SplineChartData> IncomeSummary = last7DaysTransaction
                .Where(i => i.Category.Type == "Income")
                .GroupBy(j => j.Date)
                .Select(k => new SplineChartData()
                {
                    day = k.First().Date.ToString("dd-MMM"),
                    income = k.Sum(l => l.Amount)
                })
                .ToList();

            //Expense
            List<SplineChartData> ExpenseSummary = last7DaysTransaction
                .Where(i => i.Category.Type == "Expense")
                .GroupBy(j => j.Date)
                .Select(k => new SplineChartData()
                {
                    day = k.First().Date.ToString("dd-MMM"),
                    expense = k.Sum(l => l.Amount)
                })
                .ToList();

            //Combine Income & Expense
            string[] Last7Days = Enumerable.Range(0, 7)
                .Select(i => startDate.AddDays(i).ToString("dd-MMM"))
                .ToArray();

            ViewBag.SplineChartData = from day in Last7Days
                                      join income in IncomeSummary on day equals income.day into dayIncomeJoined
                                      from income in dayIncomeJoined.DefaultIfEmpty()
                                      join expense in ExpenseSummary on day equals expense.day into expenseJoined
                                      from expense in expenseJoined.DefaultIfEmpty()
                                      select new
                                      {
                                          day = day,
                                          income = income == null ? 0 : income.income,
                                          expense = expense == null ? 0 : expense.expense,
                                      };
            //Recent Transactions
            ViewBag.RecentTransactions = await _dbContext.Transactions
                .Include(i => i.Category)
                .OrderByDescending(j => j.Date)
                .Take(5)
                .ToListAsync();

            return View();
        }

        public class SplineChartData
        {
            public string day;
            public int income;
            public int expense;

        }
    }
}
