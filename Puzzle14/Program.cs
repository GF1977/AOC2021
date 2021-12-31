namespace MyApp
{
    public class Program
    {
        // Answers for Data_p.txt  Part 1: 2899     Part 2: 3528317079545
        static readonly string filePath = @".\..\..\..\Data_t.txt";
        static Dictionary<string, char> Rules = new Dictionary<string,char>();
        static Dictionary<string, long> TemplateStats = new Dictionary<string, long>();
        static Dictionary<char, long> StatsFinal = new Dictionary<char, long>();
        public static void Main(string[] args)
        {
            ParsingInputData();
            long nResOne = EncodePolymer(10);
            long nResTwo = EncodePolymer(40);

            Console.WriteLine("Part one: {0, 20:0}", nResOne);
            Console.WriteLine("Part two: {0, 20:0}", nResTwo);
        }

        private static long CollectStats(Dictionary<string, long> Res)
        {
            foreach (var kvp in Res)
            {
                char c = kvp.Key[0];

                if (StatsFinal.ContainsKey(c))
                    StatsFinal[c] += kvp.Value;
                else
                    StatsFinal.Add(c, kvp.Value);

                c = kvp.Key[1];

                if (StatsFinal.ContainsKey(c))
                    StatsFinal[c] += kvp.Value;
                else
                    StatsFinal.Add(c, kvp.Value);
            }


            long nMax = StatsFinal.Values.Max();
            long nMin = StatsFinal.Values.Min();

            return nMax - nMin;
        }


        private static long EncodePolymer(int nSteps)
        {
            Dictionary<string, long> ResultTempA = new Dictionary<string, long>();
            Dictionary<string, long> ResultTempB = new Dictionary<string, long>();

            foreach (var kvp in TemplateStats)
                ResultTempA.Add(kvp.Key, kvp.Value);

            long nRes2Previous;
            long nRes2 = 0;
            long nRes2Final = 0;
            for (int i = 0; i < nSteps; i++)
            {
                ResultTempB.Clear();
                foreach(KeyValuePair<string, long> pair in ResultTempA)
                {
                    if(pair.Value > 0)
                    {
                        char c = Rules[pair.Key];
                        string Key1 = pair.Key[0].ToString() + c;
                        string Key2 = c + pair.Key[1].ToString();
                        if(ResultTempB.ContainsKey(Key1))
                            ResultTempB[Key1]+= pair.Value;
                        else
                            ResultTempB.Add(Key1, pair.Value);

                        if (ResultTempB.ContainsKey(Key2))
                            ResultTempB[Key2]+=pair.Value;
                        else
                            ResultTempB.Add(Key2, pair.Value);
                    }
                }

                ResultTempA.Clear();
                foreach(var kvp in ResultTempB)
                    ResultTempA.Add(kvp.Key, kvp.Value);

                nRes2Previous = nRes2;
                nRes2 = CollectStats(ResultTempA);

                 nRes2Final = (nRes2 - nRes2Previous + 1) / 2;

            }

            return nRes2Final;
        }


        private static void ParsingInputData()
        {
            string PTemplate;
            using (StreamReader file = new(filePath))
            {
                PTemplate = file.ReadLine();
                file.ReadLine(); // Empty line

                while (!file.EndOfStream)
                {
                    string line = file.ReadLine();
                    string[] parts = line.Split(" -> ");
                    Rules.Add(parts[0], parts[1][0]);
                    TemplateStats.Add(parts[0], 0);
                }
            }

            for (int i = 0; i < PTemplate.Length - 1; i++)
            {
                string Key = String.Concat(PTemplate[i], PTemplate[i + 1]);
                TemplateStats[Key]++;
            }

        }
    }
}
