using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CrossExchange
{
    public interface IPortfolioRepository : IGenericRepository<Portfolio>
    {
        Task<List<Portfolio>> GetAll(int portFolioid);
    }
}