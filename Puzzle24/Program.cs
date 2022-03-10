namespace MyApp
{
    public class Program
    {
        // Answers for Data_p.txt  Part 1:  29991993698469    Part 2: 
        static readonly string filePath = @".\..\..\..\Data_t.txt";
        static List<string[]> InputData = new List<string[]>();
        static long[] wxyz = {0,0,0,0};
        static int variableIndex = 0;
        public static void Main(string[] args)
        {
            ParsingInputData();
            // 91091993909179 => 470
            //string sIV = "9999?99?9?????";
            string sIV = "?999?99??9????";
            if (sIV.Length != 14)
                return;
            long Inputvariable = 99999999999999;
            long resOne = 1;
            int nQuestionMark = sIV.Count(s => s.Equals('?'));
            long X = (long)Math.Pow(10, nQuestionMark) - 1;
            long Xlimit = (X + 1) / 10;
            long minRes = long.MaxValue;

            (int A, int B, int C)[] ABCV = {(1,15,9), (1,11,1), (1,10,11), (1,12,3), (26,-11,10), (1,11,5), (1,14,0), (26,-6,7), (1,10,9), (26,-6,15), (26,-6,4), (26,-16,10), (26,-4,4), (26,-2,9) };

            foreach (var XX in ABCV)
            {
                ProcessEasy(Inputvariable, XX.A, XX.B, XX.C);
                Console.WriteLine("Part one: {0, 10:0}", wxyz[3]);
            }

           // return;
            while (resOne !=0 && X >= Xlimit)
            {
                Inputvariable = GetModelNumber(sIV, X);
                if (Inputvariable != -1)
                {


                    variableIndex = 0;
                    wxyz[0] = wxyz[1] = wxyz[2] = wxyz[3] = 0;

                    //Inputvariable -= (1000000000000);
                    //resOne = ProcessCommands(Inputvariable);


                    foreach (var XX in ABCV)
                    {
                        ProcessEasy(Inputvariable, XX.A, XX.B, XX.C);
                        //Console.WriteLine("Part one: {0, 10:0}", wxyz[3]);
                    }
                    resOne = wxyz[3];
                    //Console.WriteLine("Part one: {0, 10:0}", resOne);
                    if (resOne < minRes)
                    {
                        Console.WriteLine("Inputvariable = {0}       Res = {1}", Inputvariable, resOne);
                        minRes = resOne;
                    }
                    //Inputvariable--;
                }
                X--;

            }

            Console.WriteLine("Part one: {0, 10:0}", minRes);
        }

        private static void ProcessEasy(long sIV, int A, int B, int C)
        {
            int value = ReadNextVariable(sIV);

            long w = wxyz[0];
            long x = wxyz[1];
            long y = wxyz[2];
            long z = wxyz[3];

            w = value;
            if (z % 26 + B == w)
                x = 0;
            else
                x = 1;

            z /= A;

            z *= 25 * x + 1;
            y = (w + C) * x;
            z+= y;

            wxyz[0] = w;
            wxyz[1] = x;
            wxyz[2] = y;
            wxyz[3] = z;
         }

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
                    {
                        return -1;
                    }
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
            int res = 0;

            foreach (string[] cmd in InputData)
            {
               // Console.WriteLine();
                switch (cmd[0])
                {
                    case "inp":
                        {
                            int value = ReadNextVariable(Inputvariable);
                            wxyz[GetAddress(cmd[1])] = value;
                            //Console.WriteLine("{0} {1} = {2}", cmd[0], cmd[1], value);
                            break;
                        }
                    
                    case "add":
                        {
                            long argumentB = GetUnitNumber(cmd[2]);
                           // Console.WriteLine("{0} {1} = {2}      {3} = {4}", cmd[0], cmd[1], wxyz[GetAddress(cmd[1])], cmd[2], argumentB);
                            wxyz[GetAddress(cmd[1])] += argumentB;
                           // Console.WriteLine("{0} {1} = {2}      {3} = {4}", cmd[0], cmd[1], wxyz[GetAddress(cmd[1])], cmd[2], argumentB);
                            break;
                        }
                    case "mul":
                        {
                            long argumentB = GetUnitNumber(cmd[2]);
                           // Console.WriteLine("{0} {1} = {2}      {3} = {4}", cmd[0], cmd[1], wxyz[GetAddress(cmd[1])], cmd[2], argumentB);
                            wxyz[GetAddress(cmd[1])] *= argumentB;
                           // Console.WriteLine("{0} {1} = {2}      {3} = {4}", cmd[0], cmd[1], wxyz[GetAddress(cmd[1])], cmd[2], argumentB);
                            break;
                        }
                    case "div":
                        {
                            long argumentB = GetUnitNumber(cmd[2]);
                          //  Console.WriteLine("{0} {1} = {2}      {3} = {4}", cmd[0], cmd[1], wxyz[GetAddress(cmd[1])], cmd[2], argumentB);
                            wxyz[GetAddress(cmd[1])] /= argumentB;
                           // Console.WriteLine("{0} {1} = {2}      {3} = {4}", cmd[0], cmd[1], wxyz[GetAddress(cmd[1])], cmd[2], argumentB);
                            break;
                        }
                    case "mod":
                        {
                            long argumentB = GetUnitNumber(cmd[2]);
                          // Console.WriteLine("{0} {1} = {2}      {3} = {4}", cmd[0], cmd[1], wxyz[GetAddress(cmd[1])], cmd[2], argumentB);
                            wxyz[GetAddress(cmd[1])] %= argumentB;
                          //  Console.WriteLine("{0} {1} = {2}      {3} = {4}", cmd[0], cmd[1], wxyz[GetAddress(cmd[1])], cmd[2], argumentB);
                            break;
                        }
                    case "eql":
                        {
                            long argumentB = GetUnitNumber(cmd[2]);
                         //   Console.WriteLine("{0} {1} = {2}      {3} = {4}", cmd[0], cmd[1], wxyz[GetAddress(cmd[1])], cmd[2], argumentB);
                            wxyz[GetAddress(cmd[1])] = (wxyz[GetAddress(cmd[1])] == argumentB) ?  1:0;
                           // Console.WriteLine("{0} {1} = {2}      {3} = {4}", cmd[0], cmd[1], wxyz[GetAddress(cmd[1])], cmd[2], argumentB);
                            break;
                        }
                }
            }




            return wxyz[3];
        }

        private static long GetAddress(string Unit)
        {
            switch (Unit)
            {
                case "w": return 0;
                case "x": return 1;
                case "y": return 2;
                case "z": return 3;
                    default: return -1;
            }
        }

        private static long GetUnitNumber(string Unit)
        {
            long res = GetAddress(Unit);
            if (res >=0)
                return wxyz[res];
            else
                return long.Parse(Unit);

        }
        
        private static int ReadNextVariable(long Inputvariable)
        {
            string sVariable = Inputvariable.ToString();
            int nRes = 0;
            bool bParsingResult = int.TryParse(sVariable[variableIndex].ToString(), out nRes);
            if(bParsingResult)
            {
                variableIndex++;
                return nRes;
            }

            throw new Exception();
        }

        private static void ParsingInputData()
        {
            using (StreamReader file = new(filePath))
                while (!file.EndOfStream)
                {
                    string line = file.ReadLine();
                    string[] parts = line.Split(",");

                    foreach (string s in parts)
                        InputData.Add(s.Split(" "));
                }
        }
    }
}
