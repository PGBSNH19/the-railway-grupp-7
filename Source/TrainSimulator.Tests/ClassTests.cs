using System;
using Xunit;
using TrainSimulator;
using System.Collections.Generic;


namespace TrainSimulator.Tests
{
    public class ClassTests
    {
        [Fact]
        public void StationTest_ReadStations_GetStation()
        {
            var x = Station.GetStation();

            Assert.Equal(4, x.Count);

        }

        [Fact]
        public void TrainTest_ReadTrains_GetTrain()
        {
            var x = Train.GetTrains();

            Assert.NotNull(x);
        }
    }
}
