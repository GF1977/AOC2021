
namespace MyApp
{
    public class Program
    {
        // Answers for Data_p.txt  Part 1: 548     Part 2: 1074888 
        static readonly string filePath = @".\..\..\..\Data_p.txt";
        static List<string> InputData = new List<string>();

        public static void Main(string[] args)
        {
            ParsingInputData();

            int nResOne = 0, nTempOne;
            int nResTwo = 0, nTempTwo;
            foreach (string input in InputData)
            {
                SolveThePuzzle(input, out nTempOne, out nTempTwo);
                nResOne += nTempOne;
                nResTwo += nTempTwo;
            }

            Console.WriteLine("Part One: {0, 10:0}", nResOne);
            Console.WriteLine("Part Two: {0, 10:0}", nResTwo);
        }

        private static int GetDigital(string s, string[] sDigitals)
        {
            string  sSorted = String.Concat(s.OrderBy(c => c));
            for (int i = 0; i < sDigitals.Length; i++)
            {
                string sDigitalSorted = String.Concat(sDigitals[i].OrderBy(c => c));
                if (sDigitalSorted == sSorted)
                    return i;
            }
            return -1;
        }
        private static void SolveThePuzzle(string Input, out int nResOne, out int nResTwo)
        {
            string[] USP = Input.Split(" | ")[0].Split(" ");
            string[] DOV = Input.Split(" | ")[1].Split(" ");

            string[] sN = new string[10];

            sN[1] = USP.First(s => s.Length == 2);
            sN[3] = USP.First(s => s.Length == 5 && GetStringDiff(sN[1], s).Length == 3);
            sN[4] = USP.First(s => s.Length == 4);
            sN[2] = USP.First(s => s.Length == 5 && GetStringDiff(sN[4], s).Length == 5);
            sN[6] = USP.First(s => s.Length == 6 && GetStringDiff(sN[1], s).Length == 6);
            sN[5] = USP.First(s => s.Length == 5 && s != sN[3] && s != sN[2]);
            sN[7] = USP.First(s => s.Length == 3);
            sN[8] = USP.First(s => s.Length == 7);
            sN[9] = USP.First(s => s.Length == 6 && GetStringDiff(sN[4], s).Length == 2);
            sN[0] = USP.First(s => s.Length == 6 && s != sN[6] && s != sN[9]);

            nResOne = 0;
            nResTwo = 0;
            foreach (string s in DOV)
            {
                int  n = GetDigital(s, sN);
                if (n == 1 || n == 4 || n == 7 || n == 8)
                    nResOne++;

                nResTwo = nResTwo * 10 + n;
            }
        }

        private static string GetStringDiff(string a, string b)
        {
            string sRes = "";
            foreach (char c in (a + b))
                if ((a + b).Count(v=>v == c) == 1) 
                    sRes+=c;

            return sRes;
        }

        private static void ParsingInputData()
        {
            using (StreamReader file = new(filePath))
                while(!file.EndOfStream)
                {
                    string line = file.ReadLine();
                    InputData.Add(line);
                }
        }
    }
}
