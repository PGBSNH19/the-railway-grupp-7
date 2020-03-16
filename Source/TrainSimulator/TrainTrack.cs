using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace TrainSimulator
{
    class TrainTrack
    {
        public int Id { get; set; }
        public int Length { get; set; }
        public int StationID { get; set; }



        public TrainTrack(int id, int length, int stationID)
        {
            Id = id;
            Length = length;
            StationID = stationID;
        }
        public static List<TrainTrack> GetTrainTracks()
        {

            string line;
            List<TrainTrack> listOfTrainTracks = new List<TrainTrack>();
            StreamReader file =
                new StreamReader(@"traintrack.txt");
            file.ReadLine();
            while ((line = file.ReadLine()) != null)
            {
                string[] words = line.Split(',');
                listOfTrainTracks.Add(new TrainTrack(int.Parse(words[0]), int.Parse(words[1]), int.Parse(words[2])));
            }

            file.Close();
            return listOfTrainTracks;
        }

    }
}
