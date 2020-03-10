using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace TrainSimulator
{
    public class Train
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int MaxSpeed { get; set; }
        public bool Operated { get; set; }

        public Train(int id, string name, int maxSpeed, bool operated)
        {
            ID = id;
            Name = name;
            MaxSpeed = maxSpeed;
            Operated = operated;


        }

       public static void GetTrains()
        {

            string line;
            List<object> listOfTrains = new List<object>();
            StreamReader file =
                new StreamReader(@"trains.txt");
            while ((line = file.ReadLine()) != null)
            {
                string[] words = line.Split(',');
                listOfTrains.Add(new Train(int.Parse(words[0]), words[1], int.Parse(words[2]), bool.Parse(words[3])));
            }

            file.Close();
        }
    }
}
