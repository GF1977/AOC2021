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


            snailfish_number A = new snailfish_number("[[[[4,3],4],4],[7,[[8,4],9]]]");
            snailfish_number B = new snailfish_number("[[[[14,13],14],14],[17,[[18,14],19]]]");
            //Console.WriteLine(A + B);

            Console.WriteLine("Part one: {0}", A);
            Console.WriteLine("Part one: {0}", B);
            snailfish_number X = A + B;
            Console.WriteLine("Part one: {0}", X);
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