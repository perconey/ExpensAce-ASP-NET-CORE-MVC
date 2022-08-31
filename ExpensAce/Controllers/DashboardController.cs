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

            Int32 income = last7DaysTransaction
                .Where(t => t.Category?.Type == "Income")
                .Sum(t => t.Amount);

            ViewBag.TotalIncome = String.Format(culture, "{0:C0}", income);

            Int32 Expense = last7DaysTransaction
                .Where(t => t.Category?.Type == "Expense")
                .Sum(t => t.Amount);

            ViewBag.TotalExpense = String.Format(culture, "{0:C0}", Expense);

            culture.NumberFormat.CurrencyNegativePattern = 1;

            ViewBag.Balance = String.Format(culture, "{0:C0}", income - Expense);


            return View();
        }
    }
}
