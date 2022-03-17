namespace MyApp
{
    public class Program
    {
        // Answers for Data_p.txt   Part 1: 29991993698469      Part 2: 14691271141118
        // Answers for Data_pIO.txt Part 1: 96918996924991      Part 2:
        static readonly string filePath = @".\..\..\..\Data_p.txt";
        static List<(int A, int B, int C)> Koef = new List<(int A, int B, int C)>();
        static bool bFinalRound = false;
        public static void Main(string[] args)
        {
           
            ParsingInputData();
            string sIV = "999????????999";
            Console.WriteLine("Part One");
            Console.WriteLine("Started Round A: {0}", sIV);
            long nIV = SolveThePuzzle(sIV);

            if (nIV >= 0)
            {
                sIV = "???" + nIV.ToString().Substring(3, 8) + "???";
                bFinalRound = true;

                Console.WriteLine("Started Round A: {0}", sIV);
                nIV = SolveThePuzzle(sIV);
            }
            Console.WriteLine("Part one: {0, 10:0}", nIV);
        }
        private static long  SolveThePuzzle(string sIV)
        {
            long Inputvariable, bestInput = -1;
            long resOne = 1;
            int nQuestionMark = sIV.Count(s => s.Equals('?'));
            long X = (long)Math.Pow(10, nQuestionMark) - 1;
            long Xlimit = (X + 1) / 10;

            while (X >= Xlimit)
            {
                Inputvariable = GetModelNumber(sIV, X);
                if (Inputvariable != -1)
                {
                    resOne = ProcessCommands(Inputvariable);
                    if (resOne == 0)
                        return Inputvariable;

                    bestInput = Inputvariable;
                }
                X--;
            }
            return bestInput;
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
        // the logic of single block of 18 commands
        private static long ProcessCommands(long Inputvariable)
        {
            long prevZ = 0;
            int variableIndex = 0;

            foreach (var K in Koef)
            {
                long w = ReadNextVariable(Inputvariable, variableIndex);
                long z = prevZ / K.A;
                long x = prevZ % 26 + K.B;
                if (x != w)
                    z = z * 26 + w + K.C;

                prevZ = z;
                variableIndex++;
            }

            return prevZ;
        }
        private static int ReadNextVariable(long Inputvariable, int variableIndex)
        {
            int nRes;
            // it's a trick, if we make W = -Koef.C, the Z will be 0
            // in this case we just replace first and last,
            if (bFinalRound == false && (variableIndex == 0 || variableIndex == 13))
                nRes = -Koef[variableIndex].C;
            else
            {
                string sVariable = Inputvariable.ToString();
                nRes = int.Parse(sVariable[variableIndex].ToString());
            }
            return nRes;
        }
        // the input is 14 iterations of the block of the 18 commands
        // only 3 koeficient are unique in each block (line 4,5,15)
        private static void ParsingInputData()
        {
            (int A, int B, int C) KoefTMP = (0, 0, 0);
            int n = 0;
            using (StreamReader file = new(filePath))
                while (!file.EndOfStream)
                {
                    string[] s = file.ReadLine().Split(" ");

                    if (n == 4)     KoefTMP.A = int.Parse(s[2]); // div z ?
                    if (n == 5)     KoefTMP.B = int.Parse(s[2]); // add x ?
                    if (n == 15)    KoefTMP.C = int.Parse(s[2]); // add y ?

                    n++;
                    if (n == 18)
                    {
                        n = 0;
                        Koef.Add(KoefTMP);
                    }
                }
        }
    }
}
