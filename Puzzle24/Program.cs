namespace MyApp
{
    public class Program
    {
        // Answers for Data_p.txt  Part 1:      Part 2: 
        static readonly string filePath = @".\..\..\..\Data_t.txt";
        static List<string[]> InputData = new List<string[]>();
        static long[] wxyz = {0,0,0,0};
        static int variableIndex = 0;
        public static void Main(string[] args)
        {
            ParsingInputData();
            string sIV = "??891?93?0??7?";
            long Inputvariable = 99891993909979;
            long resOne = 1;
            long X = 3999999;
            long minRes = long.MaxValue;
            while(resOne !=0)
            //for(int i = 0; i <10; i++)
            {
                Inputvariable = GetModelNumber(sIV, X);

                variableIndex = 0;
                wxyz[0] = wxyz[1] = wxyz[2] = wxyz[3] = 0;
                
                //Inputvariable -= (1000000000000);
                resOne = ProcessCommands(Inputvariable);
                //Console.WriteLine("Part one: {0, 10:0}", resOne);
                if (resOne < minRes)
                {
                    Console.WriteLine("Inputvariable = {0}       Res = {1}", Inputvariable, resOne);
                    minRes = resOne;
                }

                X--;

            }

            Console.WriteLine("Part one: {0, 10:0}", Inputvariable);
        }

        private static long GetModelNumber(string sIV, long X)
        {
            int Xindex = 0;

            string s = "";
            foreach(char c in sIV)
            {
                if (c == '?')
                {
                    s += X.ToString()[Xindex];
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
