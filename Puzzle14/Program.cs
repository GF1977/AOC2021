namespace MyApp
{
    public class Program
    {
        // Answers for Data_p.txt  Part 1:      Part 2: 
        static readonly string filePath = @".\..\..\..\Data_p.txt";
        static string PTemplate;
        static Dictionary<string, char> Rules = new Dictionary<string,char>();
        
        public static void Main(string[] args)
        {
            Dictionary<char, long> Stats = new Dictionary<char, long>();

            ParsingInputData();
            string s = PTemplate;
            for (int i = 0; i < 10; i++)
            {
                //if (s.Length > 100)
                 //s = s.Substring(0, 100);
                
                s = EncodePolymer(s);
                //Console.WriteLine(s);
            }

            long nRes = CollectStats(s, out Stats);
            foreach(var k in Stats)
                Console.WriteLine("Key: {0}     Value: {1, 10:0}",k.Key, k.Value);

            Console.WriteLine("Part one: {0, 10:0}", nRes);
            Console.WriteLine("Part one: {0, 10:0}", 2);
        }

        private static long CollectStats(string s, out Dictionary<char, long> stats)
        {
            stats = new Dictionary<char, long>();
            long nMax = 0;
            long nMin = int.MaxValue;
            foreach (char value in Rules.Values.Distinct())
            {
                long n = s.Count(n => n == value);
                stats.Add(value, n);
                if (n > nMax) nMax = n;
                if (n < nMin) nMin = n;
            }

            return nMax - nMin;
        }

        private static string EncodePolymer(string s)
        {
            string sNewPolymer = s;
            int delta = 1;
            for(int i = 0; i < s.Length - 1; i++)
            {
                string Key = String.Concat(s[i],s[i + 1]);

                if (Rules.ContainsKey(Key))
                {
                    sNewPolymer = sNewPolymer.Insert(i + delta, Rules[Key].ToString());
                    delta++;
                }

            }

            return sNewPolymer;
        }

        private static void ParsingInputData()
        {
            using (StreamReader file = new(filePath))
            {
                PTemplate = file.ReadLine();
                            file.ReadLine(); // Empty line
                while (!file.EndOfStream)
                {
                    string line = file.ReadLine();
                    string[] parts = line.Split(" -> ");
                    Rules.Add(parts[0], parts[1][0]);
                }

            }
        }
    }
}
