namespace MyApp
{
    public class Program
    {
        // Answers for Data_p.txt  Part 1: 294195      Part 2: 
        static readonly string filePath = @".\..\..\..\Data_p.txt";
        static List<string> InputData = new List<string>();
        static Dictionary<string, int> Scores = new Dictionary<string, int>();
        
        public static void Main(string[] args)
        {
            Scores.Add(")",     3); // )
            Scores.Add("]",    57); // ]
            Scores.Add("}",  1197); // }
            Scores.Add(">", 25137); // >


            ParsingInputData();
            int nRes = 0;
            foreach (string input in InputData)
            {

                string s = input;
                while (SimplifyCodeLine(s, out s));

                string sBrace;
                if (GetIllegalChar(s, out sBrace))
                {
                    Console.WriteLine("String: {0, 20:0}   Illegal char: {1}", s, sBrace);
                    nRes += Scores[sBrace];
                }
                //int n = GetIllegalBracer(input);
                //if (Scores.ContainsKey(n))
                    //nRes += Scores[n];
            }

            Console.WriteLine("Part one: {0, 6:0}", nRes);
            Console.WriteLine("Part one: {0, 6:0}", 2);
        }

        private static bool GetIllegalChar(string s, out string sRes)
        {
            string sOpening = "([{<";
            string sEnding  = ")]}>";
            sRes = "";
            foreach (char op in sOpening)
                foreach (char en in sEnding)
                {
                    string sCombination = op.ToString() + en.ToString();
                    if (s.Contains(sCombination))
                    {
                        sRes = en.ToString();
                        return true;
                    }
                }

            return false;
        }

        private static bool SimplifyCodeLine(string input, out string result)
        {
            bool res = false;
            string[] sPairs = { "()", "[]", "{}", "<>" };
            foreach(string pair in sPairs)
                if (input.Contains(pair))
                {
                    input = input.Remove(input.IndexOf(pair), 2);
                    res = true;
                }

            result = input;

            return res;

        }
        private static void ParsingInputData()
        {
            using (StreamReader file = new(filePath))
                while (!file.EndOfStream)
                {
                    string line = file.ReadLine();
                    string[] parts = line.Split(",");

                    foreach (string s in parts)
                        InputData.Add(s);
                }
        }
    }
}
