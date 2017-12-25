using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Configuration;

namespace Common
{
    public class Log
    {
        public static void LogToFile(string Text)
        {
            try
            {
                if (ConfigurationSettings.AppSettings["LogToFile"].ToLower() == "true")
                {
                    FileStream fs = new FileStream(@"c:\logs\BtrStyleIntegration.txt", FileMode.OpenOrCreate, FileAccess.ReadWrite);
                    StreamWriter sw = new StreamWriter(fs);
                    sw.BaseStream.Seek(0, SeekOrigin.End);
                    sw.WriteLine("{0} :{1}", DateTime.Now, Text);
                    sw.Close();
                }
            }
            catch (Exception exc)
            {
                // Catch and stop any exceptions from logging
            }
        }
    }
}
