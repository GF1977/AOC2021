using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace MyApp // Note: actual namespace depends on the project name.
{
    public class Program
    {
        static string filePath = @".\..\..\..\Data_p.txt";
        
        public static void Main(string[] args)
        {
            List<int> sonarData = new();

            using (StreamReader file = new(filePath))
                while (!file.EndOfStream)
                {
                    string? line = file.ReadLine();
                    if (line != null)
                        sonarData.Add(int.Parse(line));
                }

            int nLargerMeasurment = GetDepthIncreases(sonarData);
            Console.WriteLine("Puzzle 1: {0}", nLargerMeasurment);

            List<int> sonarSlicedData = new();
            sonarSlicedData = GetDepthSlices(sonarData);
            nLargerMeasurment = GetDepthIncreases(sonarSlicedData);
            Console.WriteLine("Puzzle 2: {0}", nLargerMeasurment);


        }
        public static List<int> GetDepthSlices(List<int> sonarData)
        {
            List<int> res = new();
            int[] sonarDataArray = sonarData.ToArray();

            for(int i=0; i<sonarDataArray.Length; i++)
            {
                if(i + 2 < sonarDataArray.Length)
                    res.Add(sonarDataArray[i]+sonarDataArray[i+1]+sonarDataArray[i+2]);
            }

            return res;
        }
        public static int GetDepthIncreases(List<int> sonarData)
        {
            int res = 0;
            int prevMeasurement = int.MaxValue;

            foreach (int line in sonarData)
            {
                if (line > prevMeasurement)
                    res++;

                prevMeasurement = line;
            }
            
            return res;
        }
    }
}