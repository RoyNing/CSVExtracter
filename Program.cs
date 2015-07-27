using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Diagnostics;
using Microsoft.VisualBasic.FileIO;

namespace CSVExtracter
{
    class Program
    {
        static void Main(string[] args)
        {
            string SourceFilePath = args[0];
            string TargetFilePath = args[1];



            Stopwatch sw = Stopwatch.StartNew();
            //using (TextFieldParser parser = new TextFieldParser(SourceFilePath))
            //{
            //    parser.TextFieldType = FieldType.Delimited;
            //    parser.SetDelimiters(",");           

            //    string[] header = parser.ReadFields();
            //    int physicalDeliveryOfficeNameIndex = Array.IndexOf(header, "physicalDeliveryOfficeName");
            //    int nameIndex = Array.IndexOf(header, "name");
            //    int titleIndex = Array.IndexOf(header, "title");
                
            //    string[] CurrentRow;
            //    List<string> ResultList = new List<string>();

            //    while (!parser.EndOfData)
            //    {
            //        //Processing row
            //        string[] fields = parser.ReadFields();
            //        CurrentRow = new string[3] { fields[physicalDeliveryOfficeNameIndex], fields[nameIndex], fields[titleIndex] };
            //        ResultList.Add(string.Join(",", CurrentRow));
            //    }
            //}

            // Read csv
            List<string> SourceFileLines = File.ReadLines(SourceFilePath).ToList();
            sw.Stop();
            TimeSpan elapsedTime = sw.Elapsed;
            Console.WriteLine("Time to load source lines: " + elapsedTime);




            



            sw = Stopwatch.StartNew();

            // Find Index
            string[] header = SourceFileLines[0].Split(',').ToArray();
            int physicalDeliveryOfficeNameIndex = Array.IndexOf(header, "physicalDeliveryOfficeName");
            int nameIndex = Array.IndexOf(header, "name");
            int titleIndex = Array.IndexOf(header, "title");

            sw.Stop();
            elapsedTime = sw.Elapsed;
            Console.WriteLine("Time to find index: " + elapsedTime);

            sw = Stopwatch.StartNew();

            // Extract Columns
            List<string> ResultList = new List<string>();
            string[] CurrentRow;
            foreach (string line in SourceFileLines)
            {
                CurrentRow = Regex.Split(line, ",(?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)");
                if (!string.IsNullOrEmpty(CurrentRow[physicalDeliveryOfficeNameIndex]))
                {
                    CurrentRow = new string[3] { CurrentRow[physicalDeliveryOfficeNameIndex], CurrentRow[nameIndex], CurrentRow[titleIndex] };
                    ResultList.Add(string.Join(",", CurrentRow));
                }
            }

            sw.Stop();
            elapsedTime = sw.Elapsed;
            Console.WriteLine("Time to extract columns: " + elapsedTime);


            sw = Stopwatch.StartNew();

            // Export CSV
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(TargetFilePath))
            {
                foreach (string line in ResultList)
                {
                    file.WriteLine(line);
                }
            }

            sw.Stop();
            elapsedTime = sw.Elapsed;
            Console.WriteLine("Time to exporet csv: " + elapsedTime);
            Console.ReadKey();
        }
    }
}
