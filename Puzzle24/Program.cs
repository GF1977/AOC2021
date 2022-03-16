namespace MyApp
{
    public class Program
    {
        // Answers for Data_p.txt   Part 1: 29991993698469      Part 2: 14691271141118
        // pIO                      Part 1: 96918996924991      Part 2:
        static readonly string filePath = @".\..\..\..\Data_p.txt";
        static List<string[]> InputData = new List<string[]>();
        static Dictionary<int,char> Options = new Dictionary<int, char>();
        static List<(int A, int B, int C)> Koeff = new List<(int A, int B, int C)>();
        static long[] wxyz = {0,0,0,0};
        static int variableIndex = 0;
        static bool bFinalRound = false;
        public static void Main(string[] args)
        {
            
            ParsingInputData();
            string sIV = "";
            int n = 0;
            int nCount = 0;

            //while (n <= 14)
            //{
            //    n = GetNextPair(n, out sIV);

            //    string sIVtmp = "";

            //    if (Options.Count > 0)
            //    {
            //        for (int i = 0; i < sIV.Length; i++)
            //        {
            //            if (Options.ContainsKey(i))
            //                sIVtmp += Options[i];
            //            else
            //                sIVtmp += sIV[i];
            //        }
            //        // sIVtmp = "999999????9999";
            //        sIV = sIVtmp;
            //    }
            //    nCount += sIV.Count(c => c == '?');

            //    SolveThePuzzle(sIV);
            //    Options = Options.OrderBy(x => x.Key).ToDictionary(x => x.Key, x => x.Value);

            //}


            string sIVtmp2 = "";
            //for (int i = 0; i < sIV.Length; i++)
            //{
            //    if (Options.ContainsKey(i))
            //        sIVtmp2 += Options[i];
            //    else
            //        sIVtmp2 += "?";
            //}

            sIVtmp2 = "999????????999";
            Console.WriteLine("--- {0}", sIVtmp2);

            long nIV = SolveThePuzzle(sIVtmp2);

            sIVtmp2 = "???" + nIV.ToString().Substring(3, 8) + "???";
           
            bFinalRound = true;
                 nIV = SolveThePuzzle(sIVtmp2);


            Console.WriteLine("Part one: {0, 10:0}", nIV);
        }

        private static int GetNextPair(int n, out string s)
        {
            s = "";
            s = s.PadRight(n, '9');
            // 5 is the fifth command "add x N", we need to cappture N (v[2])
            for (int i = n*18 + 5; i < InputData.Count - 18; i += 18)
            {
                string[] v = InputData[i];
                if (v[0] == "add" && v[1] == "x")
                {
                    if (int.Parse(v[2]) < 0)
                    {
                        s = s.Substring(0, s.Length - 1) + "??";
                    }
                    else
                    {
                        if (s.Contains("??"))
                            break;

                        s += "9";
                    }
                }
                n++;

            }
            s = s.PadRight(14, '9');
            return n+1;
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
            //X = 1111111;
            while (resOne != 0 && X >= Xlimit)
            //while (resOne != 0 && X <= 9999999)
            {

                Inputvariable = GetModelNumber(sIV, X);
                if (Inputvariable != -1)
                {
                    variableIndex = 0;
                    wxyz[0] = wxyz[1] = wxyz[2] = wxyz[3] = 0;

                    resOne = ProcessCommands(Inputvariable, sIV.Count(s=>s =='?'));
                    if(resOne < minRes && resOne >= 0)
                    //if (resOne == 0)
                    {
                        Console.WriteLine("Inputvariable = {0}       Res = {1}", Inputvariable, resOne);
                        minRes = resOne;
                        BestInputvariable = Inputvariable;
                    }
                }
                X--;
                //X++;
            }

            return BestInputvariable;
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

        private static long ProcessCommands(long Inputvariable, int nQuestionMarks)
        {
            Dictionary<int, char> OptionsTMP = new Dictionary<int, char>();
            bool isX0 = true;
            int cntX = 0;
            int cntMax = 0;

            int nInstructionN = 0;

            foreach (var X in Koeff)
            {
                long x = 1;
                long w = ReadNextVariable(Inputvariable);
                long z = wxyz[3] / X.A;
                if ((wxyz[3] % 26 + X.B) != w)
                {
                    z = z * 26 + w + X.C;
                }

                
                
                //z =+ x * (w + X.C);

                wxyz[3] = z;
            }

            return wxyz[3];


            foreach (string[] cmd in InputData)
            {
                nInstructionN++;
                switch (cmd[0])
                {
                    case "inp":
                        {


                            int value = ReadNextVariable(Inputvariable);

                            wxyz[GetAddress(cmd[1])] = value;
                            break;
                        }
                    
                    case "add":
                        {
                            long argumentB = GetUnitNumber(cmd[2]);
                            wxyz[GetAddress(cmd[1])] += argumentB;
                            break;
                        }
                    case "mul":
                        {
                            long argumentB = GetUnitNumber(cmd[2]);
                            wxyz[GetAddress(cmd[1])] *= argumentB;
                            break;
                        }
                    case "div":
                        {
                            long argumentB = GetUnitNumber(cmd[2]);
                            wxyz[GetAddress(cmd[1])] /= argumentB;
                            break;
                        }
                    case "mod":
                        {
                            long argumentB = GetUnitNumber(cmd[2]);
                            wxyz[GetAddress(cmd[1])] %= argumentB;
                            break;
                        }
                    case "eql":
                        {
                            long argumentB = GetUnitNumber(cmd[2]);
                            wxyz[GetAddress(cmd[1])] = (wxyz[GetAddress(cmd[1])] == argumentB) ?  1:0;


                            if (cmd[2] == "0")
                            {
                                if (wxyz[1] == 0)
                                {
                                    OptionsTMP.TryAdd(variableIndex - 1, Inputvariable.ToString()[variableIndex - 1]);
                                    OptionsTMP.TryAdd(variableIndex - 2, Inputvariable.ToString()[variableIndex - 2]);
                                    cntX++;
                                    //Console.WriteLine("Index = {0}     sIV[{1}] = {2}  sIV[{3}] = {4}", variableIndex, variableIndex - 2, Inputvariable.ToString()[variableIndex - 2], variableIndex - 1, Inputvariable.ToString()[variableIndex - 1]);
                                }
                                else
                                {
                                    if (cntX > cntMax)
                                        cntMax = cntX;

                                    cntX = 0;
                                }
                            }

                            break;
                        }
                }


                if (wxyz[3] == 0 && nInstructionN % 18 == 0 && variableIndex > 6)
                {
                    Console.WriteLine("InputVariable:  {0}    Offset: {1}", Inputvariable.ToString().Substring(0,variableIndex), variableIndex);
                    //return 0;
                }
            }

            //if (Options.Count + OptionsTMP.Count >= nQuestionMarks -1   && OptionsTMP.Count > 0)
            if (cntMax >= nQuestionMarks - 1 && OptionsTMP.Count > Options.Count) 
            {
                Options.Clear();
                foreach (var kvp in OptionsTMP)
                    Options.TryAdd(kvp.Key, kvp.Value);
                return long.MaxValue;
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

            variableIndex++;

            if(bFinalRound == false)
            switch (variableIndex)
            {

                case  1: return -Koeff[variableIndex - 1].C;
                //case  2: return -Koeff[variableIndex - 1].C;
                //case  3: return -Koeff[variableIndex - 1].C;

                //case 12: return -Koeff[variableIndex - 1].C;
                //case 13: return -Koeff[variableIndex - 1].C;
                case 14: return -Koeff[variableIndex - 1].C;

            }



            string sVariable = Inputvariable.ToString();
            int nRes = 0;
            bool bParsingResult = int.TryParse(sVariable[variableIndex-1].ToString(), out nRes);
            //if(bParsingResult)
            {
                //variableIndex++;
                return nRes;
            }

            //throw new Exception();
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

            (int A, int B, int C) KoeffTMP = (0,0,0);

            int n = 0;
            foreach(string[] s in InputData)
            {
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
