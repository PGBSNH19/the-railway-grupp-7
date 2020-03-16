using System;
using System.Collections.Generic;
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

        public TravelPlan(List<Train> trains, List<Schedule> schedules, List<Station> stations, List<TrainTrack> trainTracks)
        {
            this.trains = trains;
            this.schedules = schedules;
            this.stations = stations;
            this.trainTracks = trainTracks;
        }



        public void Start()
        {

            Thread planerthread = new Thread(Depart);
            planerthread.Start();


            void Depart()
            {
                //logik för att starta och stoppa en tråd istället för thread.Abort();
                //bool stopped = false;
                //while (!stopped)
                //{

                //}


                foreach (var train in trains)
                {
                    if (train.Operated == true)
                    {
                        train.Start();
                    }
                }


                while (true)
                {
                    Thread.Sleep(500);

                    foreach (var train in trains)
                    {

                        if (train.DistanceTravelled == 100)
                        {
                            train.Stop();
                        }
                    }

                    // check distance of all trains
                    // var traindistance = trains[0].Distance;
                    //if train disance == destination stop train
                    //if all trains are stoped, break
                    //Console.WriteLine(traindistance);
                }

            }
        }
    }
}
