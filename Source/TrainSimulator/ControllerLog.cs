using System;
using System.Collections.Generic;
using System.IO;
namespace TrainSimulator
{
    class ControllerLog
    {


        public static void LogInfo(string info)
        {

            string controllerLogFilePath = @".\Data\controllerlog.txt";

            StreamWriter sw = File.CreateText(controllerLogFilePath);

            sw.WriteLine(info);

            sw.Close();

        }


    }


}
