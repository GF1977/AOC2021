namespace MyApp
{
    public class snailfish_number
    {
        public string number { get; }
        private Dictionary<string, string> pairs { get; }
        public snailfish_number()
        {
            pairs = new Dictionary<string, string>();
        }
        public snailfish_number(string s)
        {
            number = s;
            pairs = new Dictionary<string, string>();
            int nKey = 0;
            while (true)
            {
                int nPointerStart = 0;
                int nPointerEnd = 0;
                for(int i = 0; i < s.Length; i++)
                {
                    if (s[i] == '[') nPointerStart = i;
                    if (s[i] == ']')
                    {
                        nPointerEnd = i;
                        break;
                    }
                }
                string sPair = s.Substring(nPointerStart, nPointerEnd - nPointerStart + 1);

                if (nPointerEnd == 0)
                    break;

                string sKey = "K" + nKey.ToString();
                pairs.Add(sKey, sPair);
                s = s.Replace(sPair, sKey);

                nKey++;
            }
        }

        public static snailfish_number operator +(snailfish_number A, snailfish_number B)
        {
            string res = "[" + A.number + "," + B.number + "]";
            snailfish_number X = new snailfish_number(res);
            return X;
        }
        

        public snailfish_number Explode(string sToExplode)
        {
            string sRes = string.Empty;

                string[] twoInts = sToExplode.Substring(1, sToExplode.Length - 2).Split(",");
                int nLeft = int.Parse(twoInts[0]);
                int nRight = int.Parse(twoInts[1]);

                string s = number.Replace(sToExplode, "X");
                s = s.Replace("[", ".[.");
                s = s.Replace("]", ".].");
                s = s.Replace(",", ".,.");
                s = s.Replace("..", ".");

                string[] allElements = s.Split(".");

                for (int i = 0; i < allElements.Length; i++)
                {
                    if (allElements[i] == "X")
                    {
                        for (int a = i; a > 0; a--)
                        {
                            if (int.TryParse(allElements[a], out int N))
                            {
                                allElements[a] = (N + nLeft).ToString();
                                break;
                            }
                        }

                        for (int a = i; a < allElements.Length; a++)
                        {
                            if (int.TryParse(allElements[a], out int N))
                            {
                                allElements[a] = (N + nRight).ToString();
                                break;
                            }
                        }

                        break;
                    }
                }

                for (int i = 0; i < allElements.Length; i++)
                {
                    sRes += allElements[i];
                }
                sRes = sRes.Replace("X", "0");

            return new snailfish_number(sRes);
        }
        public snailfish_number Reduce()
        {
            snailfish_number Res = new snailfish_number();

            foreach (var pair in pairs)
            {
                //var FirstPairToCheck = pair.V.First(p => p.Value[1] != 'K');
                //string sKey = FirstPairToCheck.Key;

                if(!pair.Value.Contains("K"))
                if (GetDepthLevel(pair.Key) >= 4)
                {
                    Res = Explode(pair.Value);
                    break;
                }
            }
            return Res;
        }

        private int GetDepthLevel(string sKey)
        {
            int nDepthLevel = 0;
            while (sKey != null)
            {
                if (sKey != pairs.Last().Key)
                    sKey = pairs.First(v => v.Value.Contains(sKey)).Key;
                else
                    sKey = null;
                
                nDepthLevel++;
            }
            return nDepthLevel;
        }

        public override string ToString() => $"{number}";

    }
    public class Program
    {
        // Answers for Data_p.txt  Part 1:      Part 2: 
        static readonly string filePath = @".\..\..\..\Data_t.txt";
        static List<string> InputData = new List<string>();
        public static void Main(string[] args)
        {
            ParsingInputData();

            //snailfish_number A = new snailfish_number("[1,2]");
            //snailfish_number B = new snailfish_number("[[3,4],5]");

            snailfish_number A = new snailfish_number("[[[[[9,8],1],2],3],4]");
            snailfish_number B = new snailfish_number("[[7,1],[6,[5,[4,[3,2]]]]]");
            snailfish_number C = new snailfish_number("[[6,[5,[4,[3,2]]]],1]");
            //Console.WriteLine(A + B);


            snailfish_number A1 = A.Reduce();
            snailfish_number B1 = B.Reduce();
            snailfish_number C1 = C.Reduce();
            Console.WriteLine("Before: {0}     After: {1}", A, A1);
            Console.WriteLine("Before: {0}     After: {1}", B, B1);
            Console.WriteLine("Before: {0}     After: {1}", C, C1);
        }


        private static void ParsingInputData()
        {
            using (StreamReader file = new(filePath))
                while (!file.EndOfStream)
                {
                    string line = file.ReadLine();
                    InputData.Add(line);
                }
        }
    }
}