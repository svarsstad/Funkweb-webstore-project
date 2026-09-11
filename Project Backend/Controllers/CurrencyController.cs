using Microsoft.AspNetCore.Mvc;
using Project_Backend.Services;

namespace Project_Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CurrencyController : ControllerBase
    {
        private readonly CurrencyService _currencyService;

        public CurrencyController(CurrencyService currencyService)
        {
            _currencyService = currencyService;
        }

        [HttpGet("symbol/{id}")]
        public ActionResult<string> GetSym(string id)
        {
            string symbol = _currencyService.GetCreditSymbol(id);
            return Ok(symbol);
        }

        [HttpGet("exchange/{id}")]
        public ActionResult<double> GetExchange(string id)
        {
            double rate = _currencyService.GetExchangeRate(id);
            return Ok(rate);
        }
    }
}