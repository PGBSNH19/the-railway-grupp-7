using System;
using System.IO;
using System.Collections.Generic;
using System.Threading;

namespace TrainSimulator
{
    class Program
    {
        static void Main(string[] args)
        {

            List<Train> trains = Train.GetTrains();
            List<Schedule> schedules = Schedule.GetSchedule();
            List<Station> stations = Station.GetStation();
            List<Passenger> passengers = Passenger.GetPassenger();
            TravelPlan.Start();

            //var tid = new Schedule(tidtabell);
            //var train = new Train(trains);
            //var station = new Station(stations);

            //stations[0].LoadPassengersToStation(passengers);



            //var time = new timespan(10, 19, 00);
            //var addminute = timespan.fromminutes(01);

            //var tid = schedules[0].departuretime + ":00";

            //for (int i = 0; i <= 60; i++)
            //{
            //    if(time.tostring() == tid)
            //    {
            //        console.writeline(tid + " nu startar tåget");
            //    }
            //    else
            //    {
            //        console.writeline(time.tostring());
            //    }

            //    time += addminute;
            //    system.threading.thread.sleep(500);
            //}



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
}


