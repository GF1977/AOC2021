// Susing System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using static System.Math;

namespace MyApp // Note: actual namespace depends on the project name.
{
    public class Program
    {
        // Answers for Data_p.txt  Part 1: 372300     Part 2: 1675781200288
        static readonly string filePath = @".\..\..\..\Data_p.txt";
        static List<int> Fish = new List<int>();
        static int nInitialFishCount;
        public static void Main(string[] args)
        {
            // 1675781200245 too low
            // 1675781200288
            // 1675781200295 too high

            nInitialFishCount = ParsingInputData();
            Console.WriteLine("Part one: {0, 6:0}", SolveThePuzzle(80));
            Console.WriteLine("Part one: {0, 6:0}", SolveThePuzzle(256));
        }
        private static BigInteger Factorial(Int64 n)
        {
            BigInteger res = 1;
            for (int i = 1; i <= n; i++)
                res *= i;
            return res;
        }

        private static BigInteger PascalTriangle(int nRow, int nPosition)
        {
            return Factorial(nRow)/(Factorial(nPosition)*Factorial(nRow-nPosition));
        }

        private static BigInteger GetFishNumbersMath(int nIterations, int nInitialN)
        {
            int nTRow = 0;
            int nTPos = 0;

            int nCurrentN = nInitialN;
            nIterations--;
            nInitialN +=1;

            BigInteger nRes = 0;
            
            int nCount = 0;

            while(nCurrentN <= nIterations)
            {
                while(nCurrentN <= nIterations)
                {
                    //Console.WriteLine("Row: {0}    Pos: {1}    Value: {2}    Triangle: {3}", nTRow, nTPos, nCurrentN, PascalTriangle(nTRow, nTPos));
                    nRes += PascalTriangle(nTRow, nTPos);
                    nTRow++;
                    nCurrentN += 7;
                }
                nCount++;
                nTRow = nCount;
                nTPos++;
                nCurrentN = (nInitialN - 1) + 9 * nCount;
            }


            //if (nInitialN == 2)
              //  nRes--;

            return nRes + 1;
        }
        private static BigInteger SolveThePuzzle(int nIterations)
        {
            ParsingInputData();
            Dictionary<int, int> PopulationByGroups = new Dictionary<int, int>();
            foreach(int group in Fish)
            {
                if (PopulationByGroups.TryAdd(group, 1) == false)
                    PopulationByGroups[group]++;
            }


            BigInteger res = 0;
            foreach(int nAge in PopulationByGroups.Keys)
            {
                ParsingInputData();
                res += PopulationByGroups[nAge] * GetFishNumbersMath(nIterations, nAge);
            }

            return res;
        }

        // nAge - by default this method return the result for all fishes in the input.
        // if nAge specified ( >= 0) it will return the result for single parent fish
        private static int GetFishNumbersX(int nIteration, int nAge = -1)
        {
            List<int> FishTemp = new List<int>();
            List<int> FishNewPopulation = new List<int>();

            if (nAge >= 0)
            {
                Fish.Clear();
                Fish.Add(nAge);
            }

            for (int i = 0; i < nIteration; i++)
            {
                FishTemp.Clear();
                FishNewPopulation.Clear();
                
                foreach(int f in Fish)
                {
                    if(f == 0)
                    {
                        FishTemp.Add(6);
                        FishNewPopulation.Add(8);
                    }
                    else
                        FishTemp.Add(f-1);
                }
                Fish.Clear();
                Fish.AddRange(FishTemp);
                Fish.AddRange(FishNewPopulation);
            }

            int res = Fish.Count;
            return res;
        }

        private static int ParsingInputData()
        {
            Fish.Clear();
            List<string> InputData = new();
            using (StreamReader file = new(filePath))
                { 
                    string line = file.ReadLine();
                    string[] parts = line.Split(",");
                    
                    foreach(string s in parts)
                        Fish.Add(int.Parse(s));
                }

            return Fish.Count;
        }
    }
}