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
                    if (line != null)
                        inputData.Add(line);
                }

            int nLenOfDiagnosticString = inputData.First().Length;

            int nGammaRate = GetGammaRate(inputData, nLenOfDiagnosticString);
            int nEpsilonRate = GetEpsilonRate(nGammaRate, nLenOfDiagnosticString);

            Console.WriteLine("Gamma rate: {0}    Epsilon rate: {1}    Result: {2}", nGammaRate, nEpsilonRate, nGammaRate * nEpsilonRate);
        }
        static int GetEpsilonRate(int nGammaRate, int nLenOfDiagnosticString)
        {
            int nEpsilonRate = (int)(Math.Pow(2,nLenOfDiagnosticString) - nGammaRate - 1);
            return nEpsilonRate;
        }

        static int GetGammaRate(List<string> InputData, int nLenOfDiagnosticString)
        {
            
            int[] GammaRateArray = new int[nLenOfDiagnosticString];

            foreach (string s in InputData)
                for (int i = 0; i < nLenOfDiagnosticString; i++)
                {
                    if (s[i]=='0')
                        GammaRateArray[i] -= 1;
                    else
                        GammaRateArray[i] += 1;
                }

            string sGammaRay = "";
            for (int i = 0; i < nLenOfDiagnosticString; i++)
            {
                if (GammaRateArray[i] > 0)
                    sGammaRay += "1";
                else
                    sGammaRay += "0";
            }

            return Convert.ToInt32(sGammaRay,2);
        }
    }
}
