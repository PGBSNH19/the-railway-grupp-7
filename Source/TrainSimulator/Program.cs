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
            List<TrainTrack> tracks = TrainTrack.GetTrainTracks();

            TravelPlan travelPlan = new TravelPlan(trains, schedules, stations, tracks, passengers).Start();


            Console.ReadKey();
        }

    }
}


