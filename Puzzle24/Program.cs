namespace MyApp
{
    public class Program
    {
        // Answers for Data_p.txt   Part 1: 29991993698469      Part 2: 14691271141118
        // pIO                      Part 1: 96918996924991      Part 2:
        static readonly string filePath = @".\..\..\..\Data_pIO.txt";
        static List<(int A, int B, int C)> Koeff = new List<(int A, int B, int C)>();
        static long[] wxyz = {0,0,0,0};
        static int variableIndex = 0;
        static bool bFinalRound = false;
        public static void Main(string[] args)
        {
           
            ParsingInputData();
            string sIV = "999????????999";
            Console.WriteLine("Part One");
            Console.WriteLine("Started Round A: {0}", sIV);
            long nIV = SolveThePuzzle(sIV);

            sIV = "???" + nIV.ToString().Substring(3, 8) + "???";
            bFinalRound = true;

            Console.WriteLine("Started Round A: {0}", sIV);
            nIV = SolveThePuzzle(sIV);
            Console.WriteLine("Part one: {0, 10:0}", nIV);
        }
        private static long  SolveThePuzzle(string sIV)
        {
            if (sIV.Length != 14)
                return -1;

            long Inputvariable = -1;
            long BestInputvariable = -1;
            long resOne = 1;
            int nQuestionMark = sIV.Count(s => s.Equals('?'));
            long X = (long)Math.Pow(10, nQuestionMark) - 1;
            long Xlimit = (X + 1) / 10;
            long minRes = long.MaxValue;

            while (resOne != 0 && X >= Xlimit)
            {

                Inputvariable = GetModelNumber(sIV, X);
                if (Inputvariable != -1)
                {
                    variableIndex = 0;
                    wxyz[0] = wxyz[1] = wxyz[2] = wxyz[3] = 0;

                    resOne = ProcessCommands(Inputvariable);
                    if (resOne == 0)
                        return Inputvariable;

                    if (resOne < minRes && resOne >= 0)
                    //if (resOne == 0)
                    {
                        minRes = resOne;
                        BestInputvariable = Inputvariable;
                    }
                }
                X--;
                //X++;
            }

            return BestInputvariable;
        }

        // Replace wild cards by numbers. X=13 : sIV "?2?4" => "1234"
        private static long GetModelNumber(string sIV, long X)
        {
            int Xindex = 0;
            string s = "";
            foreach(char c in sIV)
            {
                if (c == '?')
                {
                    char newC = X.ToString()[Xindex];
                    if (newC == '0')
                        return -1;

                    s += newC;
                    Xindex++;
                }
                else
                    s += c;
            }
            return long.Parse(s);
        }

        private static long ProcessCommands(long Inputvariable)
        {
            Dictionary<int, char> OptionsTMP = new Dictionary<int, char>();
            foreach (var X in Koeff)
            {
                long w = ReadNextVariable(Inputvariable);
                long z = wxyz[3] / X.A;
                if ((wxyz[3] % 26 + X.B) != w)
                {
                    z = z * 26 + w + X.C;
                }
                wxyz[3] = z;
            }

            return wxyz[3];
        }
        private static int ReadNextVariable(long Inputvariable)
        {
            variableIndex++;

            if(bFinalRound == false && (variableIndex == 1 || variableIndex == 14))
                return -Koeff[variableIndex - 1].C;

            string sVariable = Inputvariable.ToString();
            int nRes = int.Parse(sVariable[variableIndex-1].ToString());
            
            return nRes;
        }
        private static void ParsingInputData()
        {
            (int A, int B, int C) KoeffTMP = (0, 0, 0);
            int n = 0;
            using (StreamReader file = new(filePath))
                while (!file.EndOfStream)
                {
                    string[] s = file.ReadLine().Split(" ");

                    if (n == 4)
                        KoeffTMP.A = int.Parse(s[2]);

                    if (n == 5)
                        KoeffTMP.B = int.Parse(s[2]);

                    if (n == 15)
                        KoeffTMP.C = int.Parse(s[2]);

                    n++;

                    if (n == 18)
                    {
                        n = 0;
                        Koeff.Add(KoeffTMP);
                    }
                }
        }
    }
}
