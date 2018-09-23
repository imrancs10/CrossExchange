using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace CrossExchange
{
    public class PortfolioRepository : GenericRepository<Portfolio>, IPortfolioRepository
    {
        public PortfolioRepository(ExchangeContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Portfolio>> GetAll(int portFolioid)
        {
            return await _dbContext.Portfolios.Include(x => x.Trade).AsQueryable().Where(x => x.Id.Equals(portFolioid)).ToListAsync();
        }
    }
}