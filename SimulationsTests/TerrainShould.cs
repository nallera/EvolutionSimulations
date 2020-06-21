using System;
using Xunit;
using EvolutionSimulations;
using Newtonsoft.Json.Linq;

namespace SimulationsTests
{
    public class TerrainShould
    {
        [Fact]
        public void HaveTheCorrectAmountOfFood()
        {
            int x = 25, y = 60;
            int foodAmount = 76;
            Terrain sut = new Terrain(x, y);
            sut.AddRandomFood(foodAmount);

            Assert.Equal(foodAmount, sut.FoodUnits.Count);
        }
    }
}
