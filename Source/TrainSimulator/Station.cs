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
        public List<Passenger> PassengersInStation { get; set; }

        public Station(int id, string stationName, bool endStation)
        {
            ID = id;
            StationName = stationName;
            EndStation = endStation;
            PassengersInStation = new List<Passenger>();
            

        }

        //public List<Passenger> LoadPassengersToStation(List<Passenger> passengers)
        //{
        //    for (int i = 0; i < 34; i++)
        //    {
        //        PassengersInStation.Add(passengers[i]);
        //    }

        //    return PassengersInStation;
        //}

        public static List<Station> GetStation()
        {
            string line;
            List<Station> listOfStations = new List<Station>();
            StreamReader file =
                new StreamReader(@"./Data/stations.txt");

            file.ReadLine();
            while ((line = file.ReadLine()) != null)
            {
                string[] words = line.Split('|');
                listOfStations.Add(new Station(int.Parse(words[0]), words[1],  bool.Parse(words[2])));
            }

            file.Close();
            return listOfStations;
        }
    }
}
