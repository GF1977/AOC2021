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
            Console.WriteLine("Part one: {0, 6:0}", SolveThePuzzle(2560));
        }
        private static BigInteger Factorial(Int64 n)
        {
            BigInteger res = 1;
            for (int i = 1; i <= n; i++) 
                res *= i;
            
            return res;
        }

        // Key of the puzzle - the growth of population is not expanential but  binomial expansion growth 
        // Pascal's triangle is the best way to represent that
        private static BigInteger PascalTriangle(int nRow, int nPosition)
        {
            // according to the theorem:
            // n! / (k! * ( n - k)!)
            return Factorial(nRow)/(Factorial(nPosition)*Factorial(nRow-nPosition));
        }

        // We create the triangle of generations, each element in triangle is associated with the value on thes same position within Pascal's triangle
        // sum of the elements is the answer
        private static BigInteger GetFishNumbers(int nIterations, int nInitialN)
        {
            BigInteger nRes = 0; 
            int nTRow = 0; // row in the triangle
            int nTPos = 0; // position in the row

            int N = nInitialN; // N value in the triangle

            while(N < nIterations)
            {
                while (N < nIterations)
                {
                    nRes += PascalTriangle(nTRow, nTPos);
                    nTRow++;
                    N += 7;
                }
                nTPos++;
                nTRow = nTPos;

                N = (nInitialN) + 9 * nTPos;
            }

            return nRes + 1;
        }

        // Associative: Total population of fishes = sum of every fish population
        // Population(a,b) = Population(a) + Population(b)
        // Solution: 
        // Population(a,a,a,a,b,b) = 4 * Population(a) + 2 * Population(b)
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