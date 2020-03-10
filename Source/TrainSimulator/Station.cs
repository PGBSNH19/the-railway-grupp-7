using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace TrainSimulator
{
   public class Station
    {

        string[] stations = File.ReadAllLines("stations.txt");

        public Station(string[] stations)
        {
            this.stations = stations;
        }
    }
}
