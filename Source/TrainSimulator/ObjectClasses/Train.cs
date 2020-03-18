using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;

namespace TrainSimulator
{
    public class Train
    {
        public int ID { get; private set; }
        public string Name { get; private set; }
        public float MaxSpeed { get; private set; }
        public bool Operated { get; private set; }
        public List<Passenger> PassengersInTrain { get; set; }
        public int TrainTrackId { get; private set; }
        public TrainState trainState { get; set; }
        private int timeTravelled = 0;
        private float distanceTravelled;

        public float DistanceTravelled
        {
            get
            {
                lock (this)
                {
                    return distanceTravelled;
                }
            }
            private set
            {
                lock (this)
                {
                    distanceTravelled = value;
                }
            }
        }


        public Train()
        {
            thread = new Thread(Drive);
            thread.IsBackground = true;
            thread.Start();
        }
        public Train(int id, string name, int maxSpeed, int trainTrackId)
        {
            ID = id;
            Name = name;
            MaxSpeed = maxSpeed;
            TrainTrackId = trainTrackId;
            PassengersInTrain = new List<Passenger>();

            thread = new Thread(Drive);
            thread.IsBackground = true;
            thread.Start();

        }
        private Thread thread;

        public void Drive()
        {

            while (true)
            {
                Thread.Sleep(200);
                try
                {
                   if(Operated == true)
                    {
                        
                        DistanceTravelled = (MaxSpeed/60) * timeTravelled;

                        Console.WriteLine(this.Name + ": has traveled " + this.DistanceTravelled + " km");
                        Console.WriteLine();

                        timeTravelled++;

                    }
                }
                catch (Exception)
                {

                    break;
                }
            }
        }

        internal void Start()
        {
            Operated = true;
        }

        internal void Stop()
        {
            Operated = false;
        }
        internal void EndStop()
        {
            Console.WriteLine("This thread will now terminate");
            ControllerLog.LogInfo("Thread will now terminate");
            Operated = false;
            thread.Abort();
            Console.WriteLine("This text should not show if the thread is terminated correctly");
            ControllerLog.LogInfo("This text should not show if the thread is terminated correctly");
        }

        public List<Passenger> LoadPassengers(List<Passenger> PassengersInStation)
        {
            foreach (var person in PassengersInStation)
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
                new StreamReader(@"./Data/trains.txt");
            file.ReadLine();
            while ((line = file.ReadLine()) != null)
            {
                string[] words = line.Split(',');
                listOfTrains.Add(new Train(int.Parse(words[0]), words[1], int.Parse(words[2]), int.Parse(words[3])));
            }

            file.Close();
            return listOfTrains;
        }
    }
}
