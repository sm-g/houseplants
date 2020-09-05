using Houseplants.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Houseplants.Data
{
    public interface IDataService
    {
        Task<IEnumerable<Pot>> GetPots();
        Task<IEnumerable<Flower>> GetFlowers();
    }
}