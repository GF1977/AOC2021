using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace MyApp // Note: actual namespace depends on the project name.
{
    public class Program
    {
        static string filePath = @".\..\..\..\Data_p.txt";
        static int nLenOfDiagnosticString;

        public static void Main(string[] args)
        {
            List<string> inputData = new();

            using (StreamReader file = new(filePath))
                while (!file.EndOfStream)
                {
                    string? line = file.ReadLine();
                    if (line != null)
                        inputData.Add(line);
                }

            nLenOfDiagnosticString = inputData.First().Length;

            // Part One
            string sGammaRate = GetGammaEpsilonRate(inputData, "Gamma");
            int nGammaRate = Convert.ToInt32(sGammaRate,2);

            string sEpsilonRate = GetGammaEpsilonRate(inputData, "Epsilon");
            int nEpsilonRate = Convert.ToInt32(sEpsilonRate, 2);

            Console.WriteLine("Gamma rate: {0}    Epsilon rate: {1}    Result: {2}", nGammaRate, nEpsilonRate, nGammaRate * nEpsilonRate);

            // Part Two
            string sOxygenRate = GetOxygenCO2Rating(inputData, sGammaRate, "Oxygen");
            int nOxygenRate = Convert.ToInt32(sOxygenRate, 2);

            string sCO2ScrubberRate = GetOxygenCO2Rating(inputData, sEpsilonRate, "CO2");
            int nCO2ScrubberRate = Convert.ToInt32(sCO2ScrubberRate, 2);

            Console.WriteLine("Oxygen rate: {0}    CO2 scrubber rate: {1}    Result: {2}", nOxygenRate, nCO2ScrubberRate, nOxygenRate * nCO2ScrubberRate);
        }
        static string GetGammaEpsilonRate(List<string> InputData, string sIdentificator)
        {
            int[] GammaRateArray = new int[nLenOfDiagnosticString];

            foreach (string s in InputData)
                for (int i = 0; i<nLenOfDiagnosticString; i++)
                {
                    if (s[i]=='0')
                        GammaRateArray[i] -= 1;
                    else
                        GammaRateArray[i] += 1;
                }

            string sResult = "";
            for (int i = 0; i<nLenOfDiagnosticString; i++)
            {
                if (sIdentificator == "Gamma")
                {
                    if (GammaRateArray[i] >= 0)
                        sResult += "1";
                    else
                        sResult += "0";
                }
                if (sIdentificator == "Epsilon")
                {
                    if (GammaRateArray[i] >= 0)
                        sResult += "0";
                    else
                        sResult += "1";
                }
            }
            return sResult;
        }

        static string GetOxygenCO2Rating(List<string> InputData, string sConsideredRate, string sIdentificator)
        {
            List<string> FilteredInput = new List<string>();
            List<string> InputDataCopy = new List<string>();

            foreach(string s in InputData)
                InputDataCopy.Add(s);

            for (int i = 0; i < nLenOfDiagnosticString; i++)
            {
                foreach (string s in InputDataCopy)
                {
                    if (s[i] == sConsideredRate[i])
                    {
                        FilteredInput.Add(s);
                    }
                }

                InputDataCopy.Clear();
                foreach (string s in FilteredInput)
                    InputDataCopy.Add(s);

                if (InputDataCopy.Count == 1)
                    break;

                if (sIdentificator == "Oxygen")
                    sConsideredRate = GetGammaEpsilonRate(FilteredInput, "Gamma");

                if (sIdentificator == "CO2")
                    sConsideredRate = GetGammaEpsilonRate(FilteredInput, "Epsilon");


                FilteredInput.Clear();

            }

            return InputDataCopy.First();
        }
    }
}
