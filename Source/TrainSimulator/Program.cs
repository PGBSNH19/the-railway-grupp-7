﻿using System;
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
            var time = new TimeSpan(10, 19, 00);
            var addMinute = TimeSpan.FromMinutes(01);


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

                time += addMinute;
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
            string[] train2 = scheduleArray[1].Split(",");
            traindId = int.Parse(train2[0]);
            stationId = int.Parse(train2[1]);
            departureTime = TimeSpan.Parse(train2[2]);
            arrivalTime = TimeSpan.Parse(train2[3]);


        }
    }

}
