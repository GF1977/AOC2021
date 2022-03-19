namespace MyApp
{
    public class Program
    {
        // Answers for Data_p.txt   Part 1: 29991993698469      Part 2: 14691271141118
        // Answers for Data_pIO.txt Part 1: 96918996924991      Part 2: 91811241911641

        static List<(int A, int B, int C)> Koef = new List<(int , int , int)>();
        static Dictionary<long, string> Options = new Dictionary<long, string>();
        public static void Main(string[] args)
        {
           
            ParsingInputData(@".\..\..\..\Data_p.txt");
            SolvePuzzle("max");
            Console.WriteLine("Part one: {0, 10:0}", Options[0]);
            SolvePuzzle("min");
            Console.WriteLine("Part two: {0, 10:0}", Options[0]);

            ParsingInputData(@".\..\..\..\Data_pIO.txt");
            SolvePuzzle("max");
            Console.WriteLine("Part one: {0, 10:0}", Options[0]);
            SolvePuzzle("min");
            Console.WriteLine("Part two: {0, 10:0}", Options[0]);

        }

        private static void SolvePuzzle(string sPart)
        {
            Options.Clear();
            char MaxOrMin = sPart == "max" ? '9' : '1';

            string sIV = "???????";
            GetTheOptions(sIV, MaxOrMin);

            Dictionary<long, string> Options2 = new Dictionary<long, string>();
            foreach (var k in Options)
                Options2.Add(k.Key, k.Value);

            Options.Clear();

            foreach (var k in Options2)
                GetTheOptions(k.Value + "???", MaxOrMin);


            Options2.Clear();
            foreach (var k in Options)
                Options2.Add(k.Key, k.Value);

            Options.Clear();

            foreach (var k in Options2)
                GetTheOptions(k.Value + "????", MaxOrMin);
        }

        private static void  GetTheOptions(string sIV, char MaxOrMin)
        {
            long Inputvariable = -1;
            int nQuestionMark = sIV.Count(s => s.Equals('?'));

            long Xdelta;
            string sX = String.Empty.PadRight(nQuestionMark, MaxOrMin);
            string sXlimit = String.Empty;

            if (MaxOrMin == '1')
            {
                sXlimit = sXlimit.PadRight(nQuestionMark, '9');
                Xdelta = 1;
            }
            else
            {
                sXlimit = sXlimit.PadRight(nQuestionMark, '1');
                Xdelta = -1;
            }

            long Xlimit = long.Parse(sXlimit);
            long X = long.Parse(sX);

            while (X != Xlimit)
            {
                Inputvariable = GetModelNumber(sIV, X);
                if (Inputvariable != -1)
                    if (ProcessCommands(Inputvariable.ToString()) == 0)
                        //return Inputvariable;
                        break;
    
                X+=Xdelta;
            }
            //return Inputvariable;
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
                    if (newC == '0') // the number should contain no 0 digits
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
        private static long ProcessCommands(string Inputvariable)
        {
            long prevZ = 0;
            int variableIndex = 0;
            bool isGood = true;

            foreach (var K in Koef)
            {
                if (variableIndex >= Inputvariable.Length)
                    break;

                long w = long.Parse(Inputvariable[variableIndex].ToString());
                long z = prevZ / K.A;
                long x = prevZ % 26 + K.B;

                if (x != w)
                        z = z * 26 + w + K.C;

                prevZ = z;

                if (K.A == 26 & w != x)
                {
                    isGood = false;
                    break;
                }

                variableIndex++;
            }

            if (isGood)
                Options.TryAdd(prevZ, Inputvariable);

            return prevZ;
        }

        // the input is 14 iterations of the blocks of the 18 commands
        // only 3 koeficient are unique in each block (line 4,5,15)
        private static void ParsingInputData(string filePath)
        {
            Koef.Clear();
            (int A, int B, int C) KoefTMP = (0, 0, 0);
            int n = 0;
            using (StreamReader file = new(filePath))
                while (!file.EndOfStream)
                {
                    string[] s = file.ReadLine().Split(" ");

                    if (n == 4)     KoefTMP.A = int.Parse(s[2]); // div z ?     (line  4,22,40 etc)
                    if (n == 5)     KoefTMP.B = int.Parse(s[2]); // add x ?     (line  5,23,41 etc)
                    if (n == 15)    KoefTMP.C = int.Parse(s[2]); // add y ?     (line 15,33,51 etc)

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