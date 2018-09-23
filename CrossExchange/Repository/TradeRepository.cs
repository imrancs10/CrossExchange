using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CrossExchange
{
    public class TradeRepository : GenericRepository<Trade>, ITradeRepository
    {
        public TradeRepository(ExchangeContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Trade>> GetAllTradings(int portFolioid)
        {
            return Query().Where(x => x.PortfolioId.Equals(portFolioid)).ToList();
        }
    }
}