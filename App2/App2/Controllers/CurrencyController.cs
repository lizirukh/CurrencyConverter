using App2.Interfaces;
using App2.Models.Json;
using Microsoft.AspNetCore.Mvc;

namespace App2.Controllers
{
    public class CurrencyController : Controller
    {
        private readonly IExchange _exchangeRepo;
        public CurrencyController(IExchange exchangeRepo)
        {
            _exchangeRepo = exchangeRepo;
            //  _exchangeRepo.SaveCurrency();
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Converter(string originalCode, string destinationCode, double amount)
        {
            //System.Diagnostics.Debug.WriteLine("originalCode = " + originalCode);
            //System.Diagnostics.Debug.WriteLine("destinationCode = " + destinationCode);
            //System.Diagnostics.Debug.WriteLine("amount = " + amount);

            Info obj = _exchangeRepo.Calculate(originalCode, destinationCode, amount);
            return Json(new {message = "OK", data = obj});
        }

        [HttpPost]
        public IActionResult SaveCurrencyInterval()
        {
            _exchangeRepo.SaveCurrency();
            return Ok();
        }

    }
}
