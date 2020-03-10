using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
namespace TrainSimulator
{
    public class Schedule
    {


        public int TrainId { get; set; }
        public int StationId { get; set; }
        public TimeSpan DepartureTime { get; set; }
        public TimeSpan ArrivalTime { get; set; }



        public Schedule(int trainId, int stationId, TimeSpan departureTime, TimeSpan arrivalTime)
        {
            TrainId = trainId;
            StationId = stationId;
            DepartureTime = departureTime;
            ArrivalTime = arrivalTime;


        }

        public static void GetSchedule()
        {
            string line;
            List<object> listOfSchedules = new List<object>();
            StreamReader file =
                new StreamReader(@"timetable.txt");
            while ((line = file.ReadLine()) != null)
            {
                string[] words = line.Split(',');
                listOfSchedules.Add(new Schedule(int.Parse(words[0]), int.Parse(words[1]), TimeSpan.Parse(words[2]), TimeSpan.Parse(words[3])));
            }

            file.Close();

        }
    }
}
