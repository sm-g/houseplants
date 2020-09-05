using Houseplants.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Houseplants.Data
{
    public class DataService : IDataService
    {
        public async Task<IEnumerable<Pot>> GetPots()
        {
            var pots = new Pot[] {
                new Pot("pot1"),
                new Pot("pot1") {WithTray = true},
                new Pot("pot1")
            };
            return pots;
        }


        public async Task<IEnumerable<Flower>> GetFlowers()
        {
            var result = new Flower[] {
                new Flower(new PlantSource(Plants.Hedera, SeedType.Seedling)),
                new Flower(new PlantSource(Plants.Hedera, SeedType.Seedling)),

            };
            return result;
        }
    }
}