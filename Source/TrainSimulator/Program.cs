using System;
using System.IO;

namespace TrainSimulator
{
    class Program
    {
        static void Main(string[] args)
        {


            string[] trains = File.ReadAllLines("trains.txt");
            string[] stations = File.ReadAllLines("stations.txt");
            string[] tidtabell = File.ReadAllLines("timetable.txt");

            var tid = new Schedule(tidtabell);
            var train = new Train(trains);
            var station = new Station(stations);
            var time = new TimeSpan(10, 20, 00);
            var addMin = TimeSpan.FromMinutes(01);


            for (int i = 0; i <= 40; i++)
            {
                if (time.ToString() == tid.arrivalTime.ToString())
                {
                    Console.WriteLine(time.ToString() + " The Train has arrived to the destination");
                }
                else if (time.ToString() == tid.departureTime.ToString())
                {
                    Console.WriteLine(time.ToString() + " The Train has left the station");
                }
                else
                {
                    Console.WriteLine(time.ToString());
                }

                time += addMin;
                System.Threading.Thread.Sleep(500);

            }
        }

    }
    public class Schedule
    {
        public int traindId { get; }
        public int stationId { get; }
        public TimeSpan departureTime { get; }
        public TimeSpan arrivalTime { get; }

        public Schedule(string[] scheduleArray)
        {
            string[] convert = scheduleArray[1].Split(",");
            traindId = int.Parse(convert[0]);
            stationId = int.Parse(convert[1]);
            departureTime = TimeSpan.Parse(convert[2]);
            arrivalTime = TimeSpan.Parse(convert[3]);

        }
    }

}
