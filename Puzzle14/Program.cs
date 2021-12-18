namespace MyApp
{
    public class Program
    {
        // Answers for Data_p.txt  Part 1:      Part 2: 
        static readonly string filePath = @".\..\..\..\Data_t.txt";
        static string PTemplate;
        static Dictionary<string, char> Rules = new Dictionary<string,char>();
        public static void Main(string[] args)
        {
            ParsingInputData();
            string s = PTemplate;
            for (int i = 0; i < 10; i++)
            {
                s = EncodePolymer(s);
            }

            int nMax = 0;
            int nMin = int.MaxValue;    
            foreach(char value in Rules.Values.Distinct())
            {
                int n = s.Count(n => n == value);
                if(n > nMax) nMax = n;
                if(n < nMin) nMin = n;
            }

            Console.WriteLine("Part one: {0, 10:0}", nMax - nMin);
            Console.WriteLine("Part one: {0, 10:0}", 2);
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
