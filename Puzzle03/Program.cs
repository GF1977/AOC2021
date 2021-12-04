using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace MyApp // Note: actual namespace depends on the project name.
{
    public class Program
    {
        // Answers for Data_p   : 2250414, 6085575
        static readonly string filePath = @".\..\..\..\Data_p.txt";
        static int nLenOfDiagnosticString;

        public static void Main(string[] args)
        {
            // parsing the input file
            List<string> InputData = new();
            using (StreamReader file = new(filePath))
                while (!file.EndOfStream)
                {
                    string? line = file.ReadLine();
                    if (line != null)
                        InputData.Add(line);
                }

            nLenOfDiagnosticString = InputData.First().Length;

            // Part One
            GetGammaEpsilonRate(InputData, out string sGammaRate, out string sEpsilonRate);

            int nGammaRate   = Convert.ToInt32(sGammaRate,2);
            int nEpsilonRate = Convert.ToInt32(sEpsilonRate, 2);

            Console.WriteLine("Gamma rate: {0}    Epsilon rate: {1}    Result: {2}", nGammaRate, nEpsilonRate, nGammaRate * nEpsilonRate);

            // Part Two
            int nOxygenRate      = GetOxygenCO2Rating(InputData, "Oxygen");
            int nCO2ScrubberRate = GetOxygenCO2Rating(InputData, "CO2");

            Console.WriteLine("Oxygen rate: {0}    CO2 scrubber rate: {1}    Result: {2}", nOxygenRate, nCO2ScrubberRate, nOxygenRate * nCO2ScrubberRate);
        }

        static void GetGammaEpsilonRate(List<string> InputData, out string sGammaRate, out string sEpsilonRate)
        {
            sGammaRate = "";
            sEpsilonRate = "";
            int[] RateArray = new int[nLenOfDiagnosticString];

            foreach (string s in InputData)
                for (int i = 0; i<nLenOfDiagnosticString; i++)
                {
                    if (s[i] == '0')
                        RateArray[i] -= 1;
                    else
                        RateArray[i] += 1;
                }

            for (int i = 0; i < nLenOfDiagnosticString; i++)
            {
                if (RateArray[i] >= 0)
                {
                    sGammaRate += "1";
                    sEpsilonRate += "0";
                }
                if (RateArray[i] < 0)
                { 
                    sGammaRate += "0";
                    sEpsilonRate += "1";
                }
            }
        }

        static int  GetOxygenCO2Rating(List<string> InputData, string sIdentificator)
        {
            List<string> FilteredInput = new List<string>();
            List<string> InputDataCopy = new List<string>();

            foreach(string s in InputData)
                InputDataCopy.Add(s);

            for (int i = 0; i < nLenOfDiagnosticString; i++)
            {
                GetGammaEpsilonRate(InputDataCopy, out string sGammaRate, out string sEpsilonRate);

                FilteredInput.Clear();
                foreach (string s in InputDataCopy)
                {
                    if (sIdentificator == "Oxygen" && s[i] == sGammaRate[i])
                        FilteredInput.Add(s);
                    if (sIdentificator == "CO2"    && s[i] == sEpsilonRate[i])
                        FilteredInput.Add(s);
                }

                InputDataCopy.Clear();
                foreach (string s in FilteredInput)
                    InputDataCopy.Add(s);

                if (InputDataCopy.Count == 1)
                    break;
            }
            return Convert.ToInt32(InputDataCopy.First(),2);
        }
    }
}
