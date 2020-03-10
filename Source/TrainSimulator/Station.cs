using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace TrainSimulator
{
    public class Station
    {
        public int ID { get; set; }
        public string StationName { get; set; }
        public bool EndStation { get; set; }

        public Station(int id, string stationName, bool endStation)
        {
            ID = id;
            StationName = stationName;
            EndStation = endStation;


        }

        public static void GetStation()
        {

            string line;
            List<object> listOfTrains = new List<object>();
            StreamReader file =
                new StreamReader(@"stations.txt");

            file.ReadLine();
            while ((line = file.ReadLine()) != null)
            {
                string[] words = line.Split('|');
                listOfTrains.Add(new Station(int.Parse(words[0]), words[1],  bool.Parse(words[2])));
            }

            file.Close();
        }
    }
}
