namespace MyApp
{
    public class Program
    {
        // Answers for Data_p.txt  Part 1: 2899     Part 2: 3528317079545
        static readonly string filePath = @".\..\..\..\Data_t.txt";
        static Dictionary<string, char> Rules = new Dictionary<string,char>();
        static Dictionary<string, long> TemplateStats = new Dictionary<string, long>();
        static Dictionary<string, long> StatsFinal = new Dictionary<string, long>();
        public static void Main(string[] args)
        {
            ParsingInputData();
            long nResOne = EncodePolymer(10);
            long nResTwo = EncodePolymer(40);

            Console.WriteLine("Part one: {0, 20:0}", nResOne);
            Console.WriteLine("Part two: {0, 20:0}", nResTwo);
        }
        private static void AddValue(Dictionary<string, long> D, string key, long value)
        {
            if (D.ContainsKey(key))
                D[key] += value;
            else
                D.Add(key, value);
        }
        private static long CollectStats(Dictionary<string, long> Res)
        {
            StatsFinal.Clear();
            foreach (var kvp in Res)
            {
                AddValue(StatsFinal, kvp.Key[0].ToString(), kvp.Value);
                AddValue(StatsFinal, kvp.Key[1].ToString(), kvp.Value);
            }
            return StatsFinal.Values.Max() - StatsFinal.Values.Min();
        }
        private static long EncodePolymer(int nSteps)
        {
            Dictionary<string, long> ResultTempA = new Dictionary<string, long>();
            Dictionary<string, long> ResultTempB = new Dictionary<string, long>();

            foreach (var kvp in TemplateStats.Where(kvp=>kvp.Value > 0))
                    ResultTempA.Add(kvp.Key, kvp.Value);

            long nRes = 0;
            for (int i = 0; i < nSteps; i++)
            {
                ResultTempB.Clear();
                foreach(KeyValuePair<string, long> pair in ResultTempA)
                {
                        string Key1 = pair.Key[0].ToString() + Rules[pair.Key];
                        string Key2 = Rules[pair.Key] + pair.Key[1].ToString();

                        AddValue(ResultTempB, Key1, pair.Value);
                        AddValue(ResultTempB, Key2, pair.Value);
                }
                ResultTempA.Clear();
                foreach(var kvp in ResultTempB)
                    ResultTempA.Add(kvp.Key, kvp.Value);
            }

            nRes = CollectStats(ResultTempA);
            return (nRes + 1) / 2;
        }
        private static void ParsingInputData()
        {
            string? PTemplate;
            using (StreamReader file = new(filePath))
            {
                PTemplate = file.ReadLine();
                file.ReadLine(); // Empty line
                while (!file.EndOfStream)
                {
                    string? line = file.ReadLine();
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