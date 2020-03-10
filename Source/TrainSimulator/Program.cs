using System;
using System.IO;
using System.Collections.Generic;

namespace TrainSimulator
{
    class Program
    {
        static void Main(string[] args)
        {

            List <Train> trains = Train.GetTrains();
            List <Schedule> schedules = Schedule.GetSchedule();
            List <Station> stations = Station.GetStation();
            //List <Passenger> passengers = Passenger.GetPassenger();


            //var tid = new Schedule(tidtabell);
            //var train = new Train(trains);
            //var station = new Station(stations);
            var time = new TimeSpan(10, 19, 00);
            time.ToString(@"hh\:mm");
            var addMinute = TimeSpan.FromMinutes(01);

            var tid = schedules[0].DepartureTime + ":00";

            for (int i = 0; i <= 60; i++)
            {
                if(time.ToString() == tid)
                {
                    Console.WriteLine(tid + " nu startar tåget");
                }
                else
                {
                    Console.WriteLine(time.ToString());
                }

                time += addMinute;
                System.Threading.Thread.Sleep(500);
            }



            //    for (int i = 0; i <= 40; i++)
            //    {
            //        if (time.ToString() == arrivalTime.ToString())
            //        {
            //            Console.WriteLine(time.ToString() + " The Train has arrived to the destination");
            //        }
            //        else if (time.ToString() == tid.departureTime.ToString())
            //        {
            //            Console.WriteLine(time.ToString() + " The Train has left the station");
            //        }
            //        else
            //        {
            //            Console.WriteLine(time.ToString());
            //        }

            //        time += addMinute;
            //        System.Threading.Thread.Sleep(500);

            //    }
        }

    }
    //public class Schedule
    //{


    //    public int traindId { get; }
    //    public int stationId { get; }
    //    public TimeSpan departureTime { get; }
    //    public TimeSpan arrivalTime { get; }



    //    public Schedule(int trainId, int stationId, TimeSpan departureTime, TimeSpan arrivalTime)
    //    {
    //        //string[] train2 = scheduleArray[1].Split(",");
    //        traindId = int.Parse(train2[0]);
    //        stationId = int.Parse(train2[1]);
    //        departureTime = TimeSpan.Parse(train2[2]);
    //        arrivalTime = TimeSpan.Parse(train2[3]);


    //    }
    //}

}
