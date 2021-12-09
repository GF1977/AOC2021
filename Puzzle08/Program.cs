
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
            int nRes = 0;
            foreach (string input in InputData)
            {
                string[] sParts = input.Split(" | ");

                nRes += SolveThePuzzle1(sParts);
            }

            Console.WriteLine("Part one: {0, 6:0}", 0);
            Console.WriteLine("Part one: {0, 6:0}", nRes);
        }

        private static int SolveThePuzzle1(string[] parts)
        {

            string[] USP = parts[0].Split(" ");
            string[] DOV = parts[1].Split(" ");

            int nRes = 0;

            string[] sN = new string[10];

            foreach (string s in USP)
            {
                if (s.Length == 2)
                {
                    sN[1] = s;
                }
                if (s.Length == 4)
                {
                    sN[4] = s;
                }
                if (s.Length == 3)
                {
                    sN[7] = s;
                }
                if (s.Length == 7)
                {
                    sN[8] = s;
                }
            }

            foreach (string s in USP)
            {
                int n = GetStringDiff(sN[1], s).Length;
                if (s.Length == 6 &&  n == 6)
                {
                    sN[6] = s;
                }
            }

            foreach (string s in USP)
            {
                if (s.Length == 5 && GetStringDiff(sN[6], s).Length == 1)
                {
                    sN[5] = s;
                }
                if (s.Length == 5 && GetStringDiff(sN[1], s).Length == 3)
                {
                    sN[3] = s;
                }
            }


            string sA = GetStringDiff(sN[7], sN[1]);
            string sC = GetStringDiff(sN[8], sN[6]);
            string sE = GetStringDiff(sN[6], sN[5]);
            string sB = GetStringDiff(GetStringDiff(sN[3], sN[8]), sE);
            string sF = GetStringDiff(sN[1], sC);
            string sD = GetStringDiff(GetStringDiff(sN[4], sN[1]), sB);
            string sG = GetStringDiff(GetStringDiff(sN[8], sN[7]), sB+sD+sE);

            string sDecode = sA + sB + sC + sD + sE + sF + sG;

            
            foreach (string s in DOV)
            {
                //Console.WriteLine("{0} = {1}", s, GetDigital(s, sDecode));

                nRes = nRes * 10 + GetDigital(s, sDecode);
            }
                

            return nRes;
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
