using System.Collections.Generic;
using System.Threading.Tasks;

namespace CrossExchange
{
    public interface IShareRepository : IGenericRepository<HourlyShareRate>
    {
        Task<HourlyShareRate> GetLatestPrice(string symbol);
        Task<List<HourlyShareRate>> Get(string symbol);
    }
}