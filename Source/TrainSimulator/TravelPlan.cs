using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace TrainSimulator
{
    class TravelPlan
    {
        List<Train> trains;
        List<Schedule> schedules;
        List<Station> stations;
        List<TrainTrack> trainTracks;
        List<Passenger> passenger;


        TimeSpan globalClock = new TimeSpan(10, 18, 00);
        TimeSpan zero = new TimeSpan(00, 00, 00);
        public TravelPlan(List<Train> trains, List<Schedule> schedules, List<Station> stations, List<TrainTrack> trainTracks, List<Passenger> passengers)
        {
            this.trains = trains;
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



        public TravelPlan Start()
        {

            Thread planerthread = new Thread(Depart);
            planerthread.IsBackground = true;
            planerthread.Start();


            void Depart()
            {

                while (true)
                {
                    Thread.Sleep(80);
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
            switch (train.trainState)
            {
                case TrainState.atStartStation:

                    train.PassengersInTrain.AddRange(stations[0].PassengersInStation);
                    stations[0].PassengersInStation.Clear();

                    var timeStartStation = trainTimeTableList.Where(x => x.StationId == track.StartStationId).FirstOrDefault();

                    Console.WriteLine("The train " + train.Name + " is at: " + stations.Where(x => x.ID == track.StartStationId).Single().StationName + " station");

                    if ((TimeSpan.Parse(timeStartStation.DepartureTime) - globalClock) >= zero)
                        Console.WriteLine(train.Name + " is waiting to depart. Departing in : " + (TimeSpan.Parse(timeStartStation.DepartureTime) - globalClock));

                    if (globalClock >= TimeSpan.Parse(timeStartStation.DepartureTime))
                    {
                        Console.WriteLine(train.Name + " Departing with " + train.PassengersInTrain.Count + " passengers in train");
                        train.Start();
                        train.trainState = TrainState.onWayToClosingCrossing;
                    }
                    break;

                case TrainState.onWayToClosingCrossing:

                    if (train.DistanceTravelled >= track.CrossingPosition - 1)
                    {
                        Console.WriteLine("Closing Crossing");
                        train.trainState = TrainState.onWayToOpenCrossing;
                    }
                    break;

                case TrainState.onWayToOpenCrossing:
                    if (train.DistanceTravelled >= track.CrossingPosition + 1)
                    {
                        Console.WriteLine("Opening Crossing");
                        train.trainState = TrainState.onWayToFirstSwitch;
                    }
                    break;

                case TrainState.onWayToFirstSwitch:

                    if (train.DistanceTravelled >= track.Switch1Position)
                    {
                        track.Switch1Left = true;
                        Console.WriteLine("First Switch is switched to it's left position");
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
                            stations[1].PassengersInStation.Add(train.PassengersInTrain.ElementAt(0));
                            train.PassengersInTrain.RemoveAt(0);
                        }

                        Console.WriteLine("The train " + train.Name + " has arrived at: " + stations.Where(x => x.ID == track.MiddleStationId).Single().StationName);
                        Console.WriteLine("Dropped of " + stations[1].PassengersInStation.Count + " passengers at " + stations[1].StationName);
                    }
                    break;

                case TrainState.atMiddleStation:
                    var timeMiddleStation = trainTimeTableList.Where(x => x.StationId == track.MiddleStationId).Single();

                    if ((TimeSpan.Parse(timeMiddleStation.DepartureTime) - globalClock) >= zero)
                        Console.WriteLine(train.Name + " waiting to depart. Departing in : " + (TimeSpan.Parse(timeMiddleStation.DepartureTime) - globalClock));

                    else
                    {
                        Console.WriteLine(train.Name + " Departing with " + train.PassengersInTrain.Count + " passengers");
                        train.Start();
                        train.trainState = TrainState.onWayToSecondSwitch;
                    }
                    break;

                case TrainState.onWayToSecondSwitch:
                    if (train.DistanceTravelled >= track.Switch2Position)
                    {
                        track.Switch2Left = false;
                        Console.WriteLine("Second Switch is switched to it's right position");
                        train.trainState = TrainState.onWayToEndStation;
                    }
                    break;

                case TrainState.onWayToEndStation:
                    if (train.DistanceTravelled >= track.EndStationPosition)
                    {
                        train.trainState = TrainState.atEndStation;
                        Console.WriteLine(train.Name + " has arrived at: " + stations.Where(x => x.ID == track.EndStationId).Single().StationName);

                        var passengersOnboard = train.PassengersInTrain.Count;
                        for (int i = 0; i < passengersOnboard; i++)
                        {
                            stations[2].PassengersInStation.Add(train.PassengersInTrain.ElementAt(0));
                            train.PassengersInTrain.RemoveAt(0);
                        }
                        Console.WriteLine("Dropped of " + stations[2].PassengersInStation.Count + " passengers at " + stations[2].StationName);


                        train.EndStop();
                    }

                    break;

                case TrainState.atEndStation:


                    break;
            }
        }
        private void Route2(Train train, List<Schedule> trainTimeTableList, TrainTrack track)
        {
            switch (train.trainState)
            {
                case TrainState.atStartStation:

                    train.PassengersInTrain.AddRange(stations[2].PassengersInStation);
                    stations[2].PassengersInStation.Clear();

                    var timeStartStation = trainTimeTableList.Where(x => x.StationId == track.StartStationId).FirstOrDefault();

                    Console.WriteLine("The train " + train.Name + " is at: " + stations.Where(x => x.ID == track.StartStationId).Single().StationName + " station");

                    if ((TimeSpan.Parse(timeStartStation.DepartureTime) - globalClock) >= zero)
                        Console.WriteLine(train.Name + " is waiting to depart. Departing in : " + (TimeSpan.Parse(timeStartStation.DepartureTime) - globalClock));

                    if (globalClock >= TimeSpan.Parse(timeStartStation.DepartureTime))
                    {
                        Console.WriteLine(train.Name + " Departing with " + train.PassengersInTrain.Count + " passengers in train");
                        train.Start();
                        train.trainState = TrainState.onWayToSecondSwitch;
                    }
                    break;

                case TrainState.onWayToSecondSwitch:

                    if (train.DistanceTravelled >= track.Switch2Position)
                    {
                        track.Switch2Left = true;
                        Console.WriteLine("Second Switch is switched to it's left position");
                        ControllerLog.LogInfo("Second switch is switched to it's left position");
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
                            stations[3].PassengersInStation.Add(train.PassengersInTrain.ElementAt(0));
                            train.PassengersInTrain.RemoveAt(0);
                        }

                        Console.WriteLine(train.Name + " has arrived at: " + stations.Where(x => x.ID == track.MiddleStationId).Single().StationName);
                        Console.WriteLine(train.Name + " dropped of " + stations[3].PassengersInStation.Count + " passengers at " + stations[3].StationName);
                    }
                    break;

                case TrainState.atMiddleStation:

                    var timeMiddleStation = trainTimeTableList.Where(x => x.StationId == track.MiddleStationId).Single();

                    if ((TimeSpan.Parse(timeMiddleStation.DepartureTime) - globalClock) >= zero)
                        Console.WriteLine(train.Name + " waiting to depart. Departing in : " + (TimeSpan.Parse(timeMiddleStation.DepartureTime) - globalClock));

                    else
                    {
                        Console.WriteLine(train.Name + " Departing with " + train.PassengersInTrain.Count + " passengers");
                        train.Start();
                        train.trainState = TrainState.onWayToFirstSwitch;
                    }
                    break;

                case TrainState.onWayToFirstSwitch:

                    if (train.DistanceTravelled >= track.Switch1Position)
                    {
                        track.Switch1Left = false;
                        Console.WriteLine("First Switch is switched to it's right position");
                        train.trainState = TrainState.onWayToClosingCrossing;
                    }
                    break;

                case TrainState.onWayToClosingCrossing:

                    if (train.DistanceTravelled >= track.CrossingPosition - 1)
                    {
                        Console.WriteLine("Closing Crossing");
                        train.trainState = TrainState.onWayToOpenCrossing;
                    }
                    break;

                case TrainState.onWayToOpenCrossing:
                    if (train.DistanceTravelled >= track.CrossingPosition + 1)
                    {
                        Console.WriteLine("Opening Crossing");
                        train.trainState = TrainState.onWayToEndStation;
                    }
                    break;


                case TrainState.onWayToEndStation:
                    if (train.DistanceTravelled >= track.EndStationPosition)
                    {
                        train.trainState = TrainState.atEndStation;
                        Console.WriteLine(train.Name + " has arrived at: " + stations.Where(x => x.ID == track.EndStationId).Single().StationName);

                        var passengersOnboard = train.PassengersInTrain.Count;

                        for (int i = 0; i < passengersOnboard; i++)
                        {
                            stations[0].PassengersInStation.Add(train.PassengersInTrain.ElementAt(0));
                            train.PassengersInTrain.RemoveAt(0);
                        }
                        Console.WriteLine(train.Name + " dropped of " + stations[0].PassengersInStation.Count + " passengers at " + stations[0].StationName);

                        train.EndStop();
                    }

                    break;

                case TrainState.atEndStation:


                    break;
            }
        }
    }
}
