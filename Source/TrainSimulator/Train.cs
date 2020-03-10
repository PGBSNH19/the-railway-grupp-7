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

        string[] trains = File.ReadAllLines("trains.txt");

        public Train(string[] trains)
        {
            this.trains = trains;
        }

        public Train LoadTrain()
        {
            foreach (string line in trains)
            {
                string[] parts = line.Split(',');
                        
                
            }
            
            return this;
        }
       
    }
}
