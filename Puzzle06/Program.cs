using System.Numerics;

namespace MyApp // Note: actual namespace depends on the project name.
{
    public class Program
    {
        // Answers for Data_p.txt  Part 1: 372300     Part 2: 1675781200288
        static readonly string filePath = @".\..\..\..\Data_p.txt";
        static List<int> Fish = new List<int>();
        public static void Main(string[] args)
        {
            ParsingInputData();
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

        // Key of the puzzle - the growth of population is not expanential but  binomial expansion growth 
        private static BigInteger PascalTriangle(int nRow, int nPosition)
        {
            // according to the theorem:
            // n! / (k! * ( n - k)!)
            return Factorial(nRow)/(Factorial(nPosition)*Factorial(nRow-nPosition));
        }

        private static BigInteger GetFishNumbers(int nIterations, int nInitialN)
        {
            BigInteger nRes = 0;
            int nTRow = 0;
            int nTPos = 0;

            int nCurrentN = nInitialN;
            nIterations--;
            nInitialN +=1;
            
            int nCount = 0;

            while(nCurrentN <= nIterations)
            {
                while(nCurrentN <= nIterations)
                {
                    nRes += PascalTriangle(nTRow, nTPos);
                    nTRow++;
                    nCurrentN += 7;
                }
                nCount++;
                nTRow = nCount;
                nTPos++;
                nCurrentN = (nInitialN - 1) + 9 * nCount;
            }

            return nRes + 1;
        }
        // Associative: Total population of fishes = sum of every fish population
        // Population(a,b) = Population(a) + Population(b)
        // Solution get the count of each fish and summarize the population multiplied by count
        // Population(a,a,a,a,b,b) = 4*Population(a) + 2 * Population(b)
        private static BigInteger SolveThePuzzle(int nIterations)
        {
            Dictionary<int, int> PopulationByGroups = new Dictionary<int, int>();
            foreach(int nAge in Fish)
            {
                if (PopulationByGroups.TryAdd(nAge, 1) == false)
                    PopulationByGroups[nAge]++;
            }

            BigInteger res = 0;
            foreach(int nAge in PopulationByGroups.Keys)
            {
                res += PopulationByGroups[nAge] * GetFishNumbers(nIterations, nAge);
            }

            return res;
        }

        private static void ParsingInputData()
        {
            List<string> InputData = new();
            using (StreamReader file = new(filePath))
                { 
                    string line = file.ReadLine();
                    string[] parts = line.Split(",");
                    
                    foreach(string s in parts)
                        Fish.Add(int.Parse(s));
                }
        }
    }
}