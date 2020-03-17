using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace TrainSimulator
{
    public class TravelPlan
    {
        List<Train> trains;
        List<Schedule> schedules;
        List<Station> stations;
        List<TrainTrack> trainTracks;
        List<Passenger> passenger;


        TimeSpan globalClock = new TimeSpan(10, 18, 00);
        TimeSpan zero = new TimeSpan(00, 00, 00);
        public TravelPlan(List<Schedule> schedules, List<Station> stations, List<TrainTrack> trainTracks, List<Passenger> passengers)
        {

            this.schedules = schedules;
            this.stations = stations;
            this.trainTracks = trainTracks;
            this.passenger = passengers;

            var passengerHalfCount = (passengers.Count / 2);

            for (int i = 0; i < passengerHalfCount; i++)
            {
                stations[0].PassengersInStation.Add(passengers.ElementAt(i));
            }

            for (int i = passengerHalfCount; i < passengers.Count; i++)
            {
                stations[2].PassengersInStation.Add(passengers.ElementAt(i));
            }
        }

        public TravelPlan SetTrain(List<Train> trains)
        {
            this.trains = trains;
            return this;
        }

        public TravelPlan Start()
        {

            Thread planerthread = new Thread(Depart);
            planerthread.IsBackground = true;
            planerthread.Start();


            void Depart()
            {

                while (true)
                {

                    Thread.Sleep(200);
                    globalClock = globalClock.Add(new TimeSpan(00, 01, 00));

                    List<Schedule> trainTimeTableList = new List<Schedule>();

                    foreach (var train in trains)
                    {

                        trainTimeTableList = (schedules.Where(x => x.TrainId == train.ID).ToList());

                        var track = trainTracks.Where(x => x.Id == train.TrainTrackId).Single();

                        try
                        {
                            if (train.ID == 2)
                                Route1(train, trainTimeTableList, track);
                            else if (train.ID == 3)
                                Route2(train, trainTimeTableList, track);
                        }
                        catch
                        {
                        }
                    }
                }
            }
            return this;
        }

        private void Route1(Train train, List<Schedule> trainTimeTableList, TrainTrack track)
        {

            var timeStartStation = trainTimeTableList.Where(x => x.StationId == track.StartStationId).FirstOrDefault();
            var startStation = stations.Where(x => x.ID == track.StartStationId).Single();
            var middleStation = stations.Where(x => x.ID == track.MiddleStationId).Single();
            var endStation = stations.Where(x => x.ID == track.EndStationId).Single();
            switch (train.trainState)
            {
                case TrainState.atStartStation:

                    train.PassengersInTrain.AddRange(startStation.PassengersInStation);
                    startStation.PassengersInStation.Clear();



                    if ((TimeSpan.Parse(timeStartStation.DepartureTime) - globalClock) >= zero)
                    {
                        Console.WriteLine(train.Name + " is waiting to depart at: " + startStation.StationName +  " Station. Departing in: " + (TimeSpan.Parse(timeStartStation.DepartureTime) - globalClock));
                        Console.WriteLine();
                        ControllerLog.LogInfo(train.Name + " is waiting to depart at: " + startStation.StationName + " Station. Departing in: " + (TimeSpan.Parse(timeStartStation.DepartureTime) - globalClock));
                        ControllerLog.LogInfo("");
                    }


                    if (globalClock >= TimeSpan.Parse(timeStartStation.DepartureTime))
                    {
                        Console.WriteLine(train.Name + ": departing with " + train.PassengersInTrain.Count + " passengers in the train");
                        Console.WriteLine();
                        ControllerLog.LogInfo(train.Name + ": departing with " + train.PassengersInTrain.Count + " passengers in the train");
                        ControllerLog.LogInfo("");

                        train.Start();
                        train.trainState = TrainState.onWayToClosingCrossing;
                    }
                    break;

                case TrainState.onWayToClosingCrossing:

                    if (train.DistanceTravelled >= track.CrossingPosition - 1)
                    {
                        Console.WriteLine("Closing the Crossing for: " + train.Name);
                        Console.WriteLine();
                        ControllerLog.LogInfo("Closing the Crossing for: " + train.Name);
                        ControllerLog.LogInfo("");


                        train.trainState = TrainState.onWayToOpenCrossing;
                    }
                    break;

                case TrainState.onWayToOpenCrossing:
                    if (train.DistanceTravelled >= track.CrossingPosition + 1)
                    {
                        Console.WriteLine("Opening the Crossing for: " + train.Name);
                        Console.WriteLine();
                        ControllerLog.LogInfo("Opening the Crossing for: " + train.Name);
                        ControllerLog.LogInfo("");

                        train.trainState = TrainState.onWayToFirstSwitch;
                    }
                    break;

                case TrainState.onWayToFirstSwitch:

                    if (train.DistanceTravelled >= track.Switch1Position)
                    {
                        track.Switch1Left = true;
                        Console.WriteLine("The First Switch is switched to it's left position for: " + train.Name);
                        Console.WriteLine();
                        ControllerLog.LogInfo("The First Switch is switched to it's left position for: " + train.Name);
                        ControllerLog.LogInfo("");

                        train.trainState = TrainState.onWayToMiddleStation;
                    }
                    break;

                case TrainState.onWayToMiddleStation:

                    if (train.DistanceTravelled >= track.MiddleStationPosition)
                    {
                        train.trainState = TrainState.atMiddleStation;
                        train.Stop();

                        var passengersOnboard = (train.PassengersInTrain.Count/2);

                        for (int i = 0; i < passengersOnboard; i++)
                        {
                            middleStation.PassengersInStation.Add(train.PassengersInTrain.ElementAt(0));
                            train.PassengersInTrain.RemoveAt(0);
                        }

                        Console.WriteLine(train.Name + " has arrived at: " + middleStation.StationName + " Station");
                        ControllerLog.LogInfo(train.Name + " has arrived at: " + middleStation.StationName + " Station");

                        Console.WriteLine(train.Name + ": dropped of " + middleStation.PassengersInStation.Count + " passengers at " + middleStation.StationName + " Station");
                        ControllerLog.LogInfo(train.Name + ": dropped of " + middleStation.PassengersInStation.Count + " passengers at " + middleStation.StationName + " Station");
                        
                    }
                    break;

                case TrainState.atMiddleStation:
                    var timeMiddleStation = trainTimeTableList.Where(x => x.StationId == track.MiddleStationId).Single();

                    if ((TimeSpan.Parse(timeMiddleStation.DepartureTime) - globalClock) >= zero)
                    {
                        Console.WriteLine(train.Name + ": waiting to depart. Departing in: " + (TimeSpan.Parse(timeMiddleStation.DepartureTime) - globalClock));
                        Console.WriteLine();
                        ControllerLog.LogInfo(train.Name + ": waiting to depart. Departing in: " + (TimeSpan.Parse(timeMiddleStation.DepartureTime) - globalClock));
                        ControllerLog.LogInfo("");
                    }

                    else
                    {
                        Console.WriteLine(train.Name + ": departing with " + train.PassengersInTrain.Count + " passengers");
                        Console.WriteLine();
                        ControllerLog.LogInfo(train.Name + ": departing with " + train.PassengersInTrain.Count + " passengers");
                        ControllerLog.LogInfo("");

                        train.Start();
                        train.trainState = TrainState.onWayToSecondSwitch;
                    }
                    break;

                case TrainState.onWayToSecondSwitch:
                    if (train.DistanceTravelled >= track.Switch2Position)
                    {
                        track.Switch2Left = false;
                        Console.WriteLine("Second Switch is switched to it's right position for: " + train.Name);
                        Console.WriteLine();
                        ControllerLog.LogInfo("Second Switch is switched to it's right position for: " + train.Name);
                        ControllerLog.LogInfo("");

                        train.trainState = TrainState.onWayToEndStation;
                    }
                    break;

                case TrainState.onWayToEndStation:
                    if (train.DistanceTravelled >= track.EndStationPosition)
                    {
                        train.trainState = TrainState.atEndStation;
                        Console.WriteLine(train.Name + ": has arrived at: " + endStation.StationName + " Station");
                        ControllerLog.LogInfo(train.Name + ": has arrived at: " + endStation.StationName + " Station");


                        var passengersOnboard = train.PassengersInTrain.Count;
                        for (int i = 0; i < passengersOnboard; i++)
                        {
                            endStation.PassengersInStation.Add(train.PassengersInTrain.ElementAt(0));
                            train.PassengersInTrain.RemoveAt(0);
                        }
                        Console.WriteLine(train.Name + ": dropped of " + endStation.PassengersInStation.Count + " passengers at " + endStation.StationName + " Station");
                        Console.WriteLine();
                        train.EndStop();
                        ControllerLog.LogInfo(train.Name + ": dropped of " + endStation.PassengersInStation.Count + " passengers at " + endStation.StationName + " Station");
                        ControllerLog.LogInfo("");
                    }

                    break;

                case TrainState.atEndStation:


                    break;
            }
        }
        private void Route2(Train train, List<Schedule> trainTimeTableList, TrainTrack track)
        {

            var timeStartStation = trainTimeTableList.Where(x => x.StationId == track.StartStationId).FirstOrDefault();
            var startStation = stations.Where(x => x.ID == track.StartStationId).Single();
            var middleStation = stations.Where(x => x.ID == track.MiddleStationId).Single();
            var endStation = stations.Where(x => x.ID == track.EndStationId).Single();

            switch (train.trainState)
            {
                case TrainState.atStartStation:

                    train.PassengersInTrain.AddRange(startStation.PassengersInStation);
                    startStation.PassengersInStation.Clear();


                    if ((TimeSpan.Parse(timeStartStation.DepartureTime) - globalClock) >= zero)
                    {
                        Console.WriteLine(train.Name + " is waiting to depart at: " + startStation.StationName + " Station. Departing in: " + (TimeSpan.Parse(timeStartStation.DepartureTime) - globalClock));
                        Console.WriteLine();
                        ControllerLog.LogInfo(train.Name + " is waiting to depart at: " + startStation.StationName + " Station. Departing in: " + (TimeSpan.Parse(timeStartStation.DepartureTime) - globalClock));
                        ControllerLog.LogInfo("");

                    }

                    if (globalClock >= TimeSpan.Parse(timeStartStation.DepartureTime))
                    {
                        Console.WriteLine(train.Name + ": departing with " + train.PassengersInTrain.Count + " passengers in train");
                        Console.WriteLine();
                        ControllerLog.LogInfo(train.Name + ": departing with " + train.PassengersInTrain.Count + " passengers in train");
                        ControllerLog.LogInfo("");

                        train.Start();
                        train.trainState = TrainState.onWayToSecondSwitch;
                    }
                    break;

                case TrainState.onWayToSecondSwitch:

                    if (train.DistanceTravelled >= track.Switch2Position)
                    {
                        track.Switch2Left = true;
                        Console.WriteLine("Second Switch is switched to it's left position for: " + train.Name);
                        Console.WriteLine();
                        ControllerLog.LogInfo("Second Switch is switched to it's left position for: " + train.Name);
                        ControllerLog.LogInfo("");

                        train.trainState = TrainState.onWayToMiddleStation;
                    }
                    break;

                case TrainState.onWayToMiddleStation:

                    if (train.DistanceTravelled >= track.MiddleStationPosition)
                    {
                        train.trainState = TrainState.atMiddleStation;
                        train.Stop();

                        var passengersOnboard = (train.PassengersInTrain.Count / 2);

                        for (int i = 0; i < passengersOnboard; i++)
                        {
                            middleStation.PassengersInStation.Add(train.PassengersInTrain.ElementAt(0));
                            train.PassengersInTrain.RemoveAt(0);
                        }

                        Console.WriteLine(train.Name + ": has arrived at: " + middleStation.StationName + " Station");
                        ControllerLog.LogInfo(train.Name + ": has arrived at: " + middleStation.StationName + " Station");

                        Console.WriteLine(train.Name + ": dropped of: " + middleStation.PassengersInStation.Count + " passengers at " + middleStation.StationName + " Station");
                        ControllerLog.LogInfo(train.Name + ": dropped of: " + middleStation.PassengersInStation.Count + " passengers at " + middleStation.StationName + " Station");
                    }
                    break;

                case TrainState.atMiddleStation:

                    var timeMiddleStation = trainTimeTableList.Where(x => x.StationId == track.MiddleStationId).Single();

                    if ((TimeSpan.Parse(timeMiddleStation.DepartureTime) - globalClock) >= zero)
                    {
                        Console.WriteLine(train.Name + " is waiting to depart at: " + middleStation.StationName + " Station. Departing in: " + (TimeSpan.Parse(timeMiddleStation.DepartureTime) - globalClock));
                        ControllerLog.LogInfo(train.Name + " is waiting to depart at: " + middleStation.StationName + " Station. Departing in: " + (TimeSpan.Parse(timeMiddleStation.DepartureTime) - globalClock));
                    }

                    else
                    {
                        Console.WriteLine(train.Name + ": departing with " + train.PassengersInTrain.Count + " passengers");
                        Console.WriteLine();
                        ControllerLog.LogInfo(train.Name + ": departing with " + train.PassengersInTrain.Count + " passengers");
                        ControllerLog.LogInfo("");

                        train.Start();
                        train.trainState = TrainState.onWayToFirstSwitch;
                    }
                    break;

                case TrainState.onWayToFirstSwitch:

                    if (train.DistanceTravelled >= track.Switch1Position)
                    {
                        track.Switch1Left = false;
                        Console.WriteLine("First Switch is switched to it's right position for: " + train.Name);
                        Console.WriteLine();
                        ControllerLog.LogInfo("First Switch is switched to it's right position for: " + train.Name);
                        ControllerLog.LogInfo("");

                        train.trainState = TrainState.onWayToClosingCrossing;
                    }
                    break;

                case TrainState.onWayToClosingCrossing:

                    if (train.DistanceTravelled >= track.CrossingPosition - 1)
                    {
                        Console.WriteLine("Closing the Crossing for: " + train.Name);
                        Console.WriteLine();
                        ControllerLog.LogInfo("Closing the Crossing for: " + train.Name);
                        ControllerLog.LogInfo("");

                        train.trainState = TrainState.onWayToOpenCrossing;
                    }
                    break;

                case TrainState.onWayToOpenCrossing:
                    if (train.DistanceTravelled >= track.CrossingPosition + 1)
                    {
                        Console.WriteLine("Opening the Crossing for: " + train.Name);
                        Console.WriteLine();
                        ControllerLog.LogInfo("Opening the Crossing for: " + train.Name);
                        ControllerLog.LogInfo("");

                        train.trainState = TrainState.onWayToEndStation;
                    }
                    break;


                case TrainState.onWayToEndStation:
                    if (train.DistanceTravelled >= track.EndStationPosition)
                    {
                        train.trainState = TrainState.atEndStation;
                        Console.WriteLine(train.Name + " has arrived at: " + endStation.StationName + " Station");
                        ControllerLog.LogInfo(train.Name + " has arrived at: " + endStation.StationName + " Station");


                        var passengersOnboard = train.PassengersInTrain.Count;

                        for (int i = 0; i < passengersOnboard; i++)
                        {
                            endStation.PassengersInStation.Add(train.PassengersInTrain.ElementAt(0));
                            train.PassengersInTrain.RemoveAt(0);
                        }
                        Console.WriteLine(train.Name + " dropped of " + endStation.PassengersInStation.Count + " passengers at " + endStation.StationName + " Station");
                        ControllerLog.LogInfo(train.Name + " dropped of " + endStation.PassengersInStation.Count + " passengers at " + endStation.StationName + " Station");

                        train.EndStop();
                    }

                    break;

                case TrainState.atEndStation:


                    break;
            }
        }
    }
}
