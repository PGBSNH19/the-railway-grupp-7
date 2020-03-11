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

        public TravelPlan(List<Train> trains, List<Schedule> schedules, List<Station> stations)
        {
            this.trains = trains;
            this.schedules = schedules;
        }



        public void Start()
        {
            


            Thread planerthread = new Thread(Depart);
            planerthread.Start();


            void Depart()
            {

                //int distanceTravelled = 0;
                //for (int i = 0; i < 10; i++)
                // start all trains
                trains[0].Start();
                while (true)
                {
                    Thread.Sleep(500);
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
