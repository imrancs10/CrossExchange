using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CrossExchange
{
    public class ShareRepository : GenericRepository<HourlyShareRate>, IShareRepository
    {
        public ShareRepository(ExchangeContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<HourlyShareRate> GetLatestPrice(string symbol)
        {
            return await Query().Where(x => x.Symbol.Equals(symbol)).OrderByDescending(x => x.TimeStamp).FirstOrDefaultAsync();
        }
        public async Task<List<HourlyShareRate>> Get(string symbol)
        {
            return Query().Where(x => x.Symbol.Equals(symbol)).ToList();
        }
    }
}