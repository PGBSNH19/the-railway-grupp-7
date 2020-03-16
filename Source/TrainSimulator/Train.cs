using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;

namespace TrainSimulator
{
    public class Train
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int MaxSpeed { get; set; }
        public bool Operated { get; set; }
        public List<Passenger> PassengersInTrain { get; set; }
        public int Position { get; set; }
        public int DistanceTravelled { get; set; }
        private int timeTravelled = 0;
        public Train(int id, string name, int maxSpeed, bool operated)
        {
            ID = id;
            Name = name;
            MaxSpeed = maxSpeed;
            Operated = operated;
            PassengersInTrain = new List<Passenger>();
            thread = new Thread(Drive);

        }
        private Thread thread;

        public void Drive()
        {
 
            while (true)
            {
                Thread.Sleep(200);
                timeTravelled += 1;

                DistanceTravelled = MaxSpeed * timeTravelled;

                if (DistanceTravelled == 100)
                    break;
            }
        }

        internal void Start()
        {
            thread.Start();
            Console.WriteLine("Is the thread alive? " + thread.IsAlive);

        }

        internal void Stop()
        {
            Console.WriteLine("Is the thread alive? " + thread.IsAlive);

            thread.Abort();
            DistanceTravelled = 0;
            timeTravelled = 0;
            Operated = false;
        }

        /*
* thrad(drive);
* 
* */

        public List<Passenger> LoadPassengers(List<Passenger> PassengersInStation)
        {
            foreach (var person in PassengersInStation )
            {
                PassengersInTrain.Add(person);
                
            }

            return PassengersInTrain;
        }

        public static List<Train> GetTrains()
        {

            string line;
            List<Train> listOfTrains = new List<Train>();
            StreamReader file =
                new StreamReader(@"trains.txt");
            file.ReadLine();
            while ((line = file.ReadLine()) != null)
            {
                string[] words = line.Split(',');
                listOfTrains.Add(new Train(int.Parse(words[0]), words[1], int.Parse(words[2]), bool.Parse(words[3])));
            }

            file.Close();
            return listOfTrains;
        }
    }
}
