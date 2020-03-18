using System;
using System.Collections.Generic;
using System.IO;

namespace TrainSimulator
{
    public class ControllerLog
    {

        public static void LogInfo(string info)
        {

            string controllerLogFilePath = @"./Data/controllerlog.txt";

            StreamWriter sw = File.AppendText(controllerLogFilePath);

            sw.WriteLine(info);

            sw.Close();

        }


    }


}
