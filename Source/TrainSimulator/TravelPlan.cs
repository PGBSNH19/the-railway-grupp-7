using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace TrainSimulator
{
    class TravelPlan
    {

        public static void Start()
        {
            
            
            Thread train1 = new Thread(Depart);
            train1.Start();


            void Depart()
            {

                int distanceTravelled = 0;
                for (int i = 0; i < 10; i++)
                {
                    distanceTravelled += 1;
                    Thread.Sleep(500);
                    Console.WriteLine(distanceTravelled);
                }

                if (distanceTravelled == 10)
                {
                    Console.WriteLine();
                    Console.ReadLine();
                }

            }
        }
    }
}
