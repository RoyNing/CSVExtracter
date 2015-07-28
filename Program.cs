using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Diagnostics;
using Microsoft.VisualBasic.FileIO;
using System.Configuration;


namespace CSVExtracter
{
    class Program
    {
        static void Main(string[] args)
        {
            string SourceFilePath = args[0];
            string TargetFilePath = args[1];

            List<string> ResultList = TextFieldParserMethod(SourceFilePath);
            ExportCSV(TargetFilePath, ResultList);
        }

        // TextFieldParser Method 
        private static List<string> TextFieldParserMethod(string SourceFilePath)
        {
            List<string> ResultList = new List<string>();

            // Parser
            
            using (TextFieldParser parser = new TextFieldParser(SourceFilePath))
            {
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(",");

                // Find Index
                string[] header = parser.ReadFields();
                int physicalDeliveryOfficeNameIndex = Array.IndexOf(header, "physicalDeliveryOfficeName");
                int nameIndex = Array.IndexOf(header, "name");
                int titleIndex = Array.IndexOf(header, "title");

                string[] CurrentRow;
               
                while (!parser.EndOfData)
                {
                    //Processing row
                    string[] fields = parser.ReadFields();
                    CurrentRow = new string[3] { fields[physicalDeliveryOfficeNameIndex], fields[nameIndex], fields[titleIndex] };
                    if (!string.IsNullOrEmpty(CurrentRow[0]))
                    ResultList.Add(string.Join(",", CurrentRow));
                }
            }
            return ResultList;
        }
        
        // Export CSV Method
        private static void ExportCSV(string TargetFilePath, List<string> ResultList)
        {
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(TargetFilePath))
            {
                foreach (string line in ResultList)
                {
                    file.WriteLine(line);
                }
            }
        }
    }
}
