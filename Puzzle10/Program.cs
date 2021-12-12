namespace MyApp
{
    public class Program
    {
        // Answers for Data_p.txt  Part 1: 294195      Part 2: 3490802734
        static readonly string filePath = @".\..\..\..\Data_p.txt";
        static List<string> InputData = new List<string>();
        public static void Main(string[] args)
        {
            Dictionary<string, int>  IllegalCharScores       = new Dictionary<string, int>() {{ ")", 3}, { "]", 57}, { "}", 1197}, { ">", 25137 }};
            Dictionary<string, int>  AutoСompleteCharScores  = new Dictionary<string, int>() {{ ")", 1}, { "]",  2}, { "}",    3}, { ">",     4 }};

            List<long> PartTwoResults = new List<long>();
            ParsingInputData();

            int nResOne = 0;
            foreach (string input in InputData)
            {
                string sSimplifiedInput = input;
                while (SimplifyCodeLine(sSimplifiedInput, out sSimplifiedInput));

                string sBrace;
                if (GetIllegalChar(sSimplifiedInput, out sBrace))
                    nResOne += IllegalCharScores[sBrace];
                else
                {
                    long nResTwo = 0;
                    string sMissedPart = AutoComplete(sSimplifiedInput);
                    foreach (char brace in sMissedPart)
                        nResTwo = nResTwo * 5 + AutoСompleteCharScores[brace.ToString()];

                    PartTwoResults.Add(nResTwo);
                }
            }
            PartTwoResults.Sort();
            int n = (PartTwoResults.Count - 1) / 2;

            Console.WriteLine("Part one: {0, 10:0}", nResOne);
            Console.WriteLine("Part one: {0, 10:0}", PartTwoResults[n]);
        }
        private static string AutoComplete(string s)
        {
            string sEnd = "";
            foreach (char c in s)
                switch (c)
                {
                    case '(': sEnd = ")" + sEnd; break;
                    case '[': sEnd = "]" + sEnd; break;
                    case '{': sEnd = "}" + sEnd; break;
                    case '<': sEnd = ">" + sEnd; break;

                    default:
                        break;
                }
            return sEnd;
        }
        private static bool GetIllegalChar(string s, out string sRes)
        {
            string sOpening = "([{<";
            string sEnding  = ")]}>";
            sRes = "";
            foreach (char op in sOpening)
                foreach (char en in sEnding)
                    if (s.Contains(op.ToString() + en.ToString()))
                    {
                        sRes = en.ToString();
                        return true;
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