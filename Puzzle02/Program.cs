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
            List<string> inputData = new();

            using (StreamReader file = new(filePath))
                while (!file.EndOfStream)
                {
                    string? line = file.ReadLine();
                    if(line != null)
                        inputData.Add(line);
                }

            int nRes = ProcessCourse(inputData, 1);
            Console.WriteLine("Puzzle 1: Answer = {0}", nRes);

                nRes = ProcessCourse(inputData, 2);
            Console.WriteLine("Puzzle 2: Answer = {0}", nRes);
        }

        static  int  ProcessCourse(List<string> inputData, int nPuzzlePart)
        {
            int nHorizontalPosition = 0;
            int nDepthPartOne = 0;
            int nDepthPartTwo = 0;
            int nAim = 0;

            foreach (string s in inputData)
            {
                string[] sParts = s.Split(" ");
                int nUnits = int.Parse(sParts[1]);
                switch (sParts[0])
                {
                    case "forward":
                        nHorizontalPosition += nUnits;
                        nDepthPartTwo += nAim * nUnits;
                        break;
                    case "up":
                        nDepthPartOne -= nUnits;
                        nAim -= nUnits;
                        break;
                    case "down":
                        nDepthPartOne += nUnits;
                        nAim += nUnits;
                        break;

                    default:
                        break;
                }
            }
            int res = nHorizontalPosition * nDepthPartOne;

            if (nPuzzlePart == 2)
                res = nHorizontalPosition * nDepthPartTwo;

            return res; ;
        }

    }
}