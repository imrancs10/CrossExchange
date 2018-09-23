using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace CrossExchange.Controller
{
    [Route("api/Portfolio")]
    public class PortfolioController : ControllerBase
    {
        private IPortfolioRepository _portfolioRepository { get; set; }

        public PortfolioController(IShareRepository shareRepository, ITradeRepository tradeRepository, IPortfolioRepository portfolioRepository)
        {
            _portfolioRepository = portfolioRepository;
        }

        [HttpGet("{portFolioid}")]
        public async Task<IActionResult> GetPortfolioInfo([FromRoute]int portFolioid)
        {
            var portfolio = await _portfolioRepository.GetAll(portFolioid);
            if (!portfolio.Any())
            {
                return NotFound();
            }
            return Ok(portfolio);
        }


        [HttpPost]
        public async Task<IActionResult> Post([FromBody]PortfolioModel value)
        {
            if (!ModelState.IsValid || value == null)
            {
                return BadRequest(ModelState);
            }
            Portfolio data = new Portfolio
            {
                Name = value.Name
            };
            await _portfolioRepository.InsertAsync(data);

            return Created($"Portfolio/{data.Id}", data);
        }

    }
}
