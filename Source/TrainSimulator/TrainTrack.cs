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
        public bool Switch { get; set; }
        public int StationID { get; set; }
        public bool CrossingOpen { get; set; }

    }
}
