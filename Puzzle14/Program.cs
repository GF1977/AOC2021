namespace MyApp
{
    public class Program
    {
        // Answers for Data_p.txt  Part 1: 2899     Part 2: 3528317079545
        static readonly string filePath = @".\..\..\..\Data_t.txt";
        static string? PTemplate;
        static Dictionary<string, char> Rules           = new Dictionary<string, char>();
        static Dictionary<string, long> TemplateStats   = new Dictionary<string, long>();
        public static void Main(string[] args)
        {
            ParsingInputData();

            Console.WriteLine("Part one: {0, 20:0}", EncodePolymer(10));
            Console.WriteLine("Part two: {0, 20:0}", EncodePolymer(40));
        }
        private static void AddValue(Dictionary<string, long> D, string key, long value)
        {
            if (D.ContainsKey(key))
                D[key] += value;
            else
                D.Add(key, value);
        }
        private static long EncodePolymer(int nSteps)
        {
            Dictionary<string, long> StatsFinal  = new Dictionary<string, long>();
            Dictionary<string, long> ResultTempA = new Dictionary<string, long>();
            Dictionary<string, long> ResultTempB = new Dictionary<string, long>();

            foreach (var kvp in TemplateStats.Where(kvp=>kvp.Value > 0))
                    ResultTempA.Add(kvp.Key, kvp.Value);

            for (int i = 0; i < nSteps; i++)
            {
                ResultTempB.Clear();
                foreach(var pair in ResultTempA)
                {   // CH -> B
                    string Key1 = pair.Key[0].ToString() + Rules[pair.Key]; // Key1 = CB
                    string Key2 = Rules[pair.Key] + pair.Key[1].ToString(); // Key2 = BH

                    AddValue(ResultTempB, Key1, pair.Value);
                    AddValue(ResultTempB, Key2, pair.Value);
                }

                ResultTempA.Clear();
                foreach(var kvp in ResultTempB)
                    ResultTempA.Add(kvp.Key, kvp.Value);
            }

            foreach (var kvp in ResultTempA)
                AddValue(StatsFinal, kvp.Key[1].ToString(), kvp.Value);

            // As we count the second character of each pair, we missed one instance of the very first char in the string, so adding +1 to the stats manually
            // NNCB  => NN NC CB => N[N] N[C] C[B]   the [N] "N in square brackets" - counted only once, but there are two Ns in the  string
            StatsFinal[PTemplate[0].ToString()]++;

            return (StatsFinal.Values.Max() - StatsFinal.Values.Min());
        }
        private static void ParsingInputData()
        {
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