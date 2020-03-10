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
        public string DepartureTime { get; set; }
        public string ArrivalTime { get; set; }



        public Schedule(int trainId, int stationId, string departureTime, string arrivalTime)
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
            file.ReadLine();
            while ((line = file.ReadLine()) != null)
            {
                string[] words = line.Split(',');
                listOfSchedules.Add(new Schedule(int.Parse(words[0]), int.Parse(words[1]), words[2], words[3]));
            }

            file.Close();

        }
    }
}
