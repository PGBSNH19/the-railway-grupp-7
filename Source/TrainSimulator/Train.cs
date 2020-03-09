using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace TrainSimulator
{
    class Train
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int MaxSpeed { get; set; }
        public bool Operated { get; set; }

        public Train LoadTrain()
        {
         string[] TrainFilePath = File.ReadAllLines("trains.txt");

            foreach (string line in TrainFilePath)
            {
                string[] parts = line.Split(',');
                int id = int.Parse(parts[0]);
                
            }
            return this;
        }
    }
}
