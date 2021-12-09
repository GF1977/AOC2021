
namespace MyApp
{
    public class Program
    {
        // Answers for Data_p.txt  Part 1:      Part 2: 
        static readonly string filePath = @".\..\..\..\Data_p.txt";
        static List<string> InputData = new List<string>();

        public static void Main(string[] args)
        {
            ParsingInputData();

            int nResOne = 0;
            foreach (string input in InputData)
                nResOne += SolveThePuzzle(input, 1);


            int nResTwo = 0;
            foreach (string input in InputData)
                nResTwo += SolveThePuzzle(input, 2);

            Console.WriteLine("Part One: {0, 10:0}", nResOne);
            Console.WriteLine("Part Two: {0, 10:0}", nResTwo);
        }

        private static int SolveThePuzzle(string Input, int nPuzzlePart)
        {
            string[] parts = Input.Split(" | ");

            string[] USP = parts[0].Split(" ");
            string[] DOV = parts[1].Split(" ");

            string[] sN = new string[10];

            sN[1] = USP.First(s => s.Length == 2);
            sN[3] = USP.First(s => s.Length == 5 && GetStringDiff(sN[1], s).Length == 3);
            sN[4] = USP.First(s => s.Length == 4);
            sN[6] = USP.First(s => s.Length == 6 && GetStringDiff(sN[1], s).Length == 6);
            sN[5] = USP.First(s => s.Length == 5 && GetStringDiff(sN[6], s).Length == 1);
            sN[7] = USP.First(s => s.Length == 3);
            sN[8] = USP.First(s => s.Length == 7);


            string sA = GetStringDiff(sN[7], sN[1]);
            string sC = GetStringDiff(sN[8], sN[6]);
            string sE = GetStringDiff(GetStringDiff(sN[8], sN[5]), sC);
            string sB = GetStringDiff(GetStringDiff(sN[3], sN[8]), sE);
            string sF = GetStringDiff(sN[1], sC);
            string sD = GetStringDiff(GetStringDiff(sN[4], sN[1]), sB);
            string sG = GetStringDiff(GetStringDiff(sN[8], sN[7]), sB+sD+sE);

            string sDecode = sA + sB + sC + sD + sE + sF + sG;


            int nResOne = 0;
            int nResTwo = 0;
            foreach (string s in DOV)
            {
                int n = GetDigital(s, sDecode);
                if (n == 1 || n == 4 || n == 7 || n == 8)
                    nResOne++;

                nResTwo = nResTwo * 10 + n;
            }

            return (nPuzzlePart == 1) ? nResOne : nResTwo;
        }

        private static string GetStringDiff(string a, string b)
        {
            string sRes = "";

            foreach (char c in a)
                if (!b.Contains(c))
                    sRes += c;

            foreach (char c in b)
                if (!a.Contains(c))
                    sRes += c;

            return sRes;
        }
        private static int GetDigital(string s, string encoder)
        {
            string sPlain = "abcdefg";
            string sNew = "";
            foreach(char c in s)
                for(int i= 0; i < 7 ; i++)
                {
                    if (c == encoder[i])
                        sNew += sPlain[i];
                }

            string sSortedS = String.Concat(sNew.OrderBy(c => c));
            switch (sSortedS)
            {
                case "abcefg":  return 0;
                case "cf":      return 1;
                case "acdeg":   return 2;
                case "acdfg":   return 3;
                case "bcdf":    return 4;
                case "abdfg":   return 5;
                case "abdefg":  return 6;
                case "acf":     return 7;
                case "abcdefg": return 8;
                case "abcdfg":  return 9;

                default:
                    return -1;
            }
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
