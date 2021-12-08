namespace MyApp // Note: actual namespace depends on the project name.
{
    public class Program
    {   
        // Answers for Data_p.txt  Part 1: 343441     Part 2: 98925151
        static readonly string filePath = @".\..\..\..\Data_p.txt";
        static List<int> Positions = new List<int>();
        public static void Main(string[] args)
        {
            ParsingInputData();
            int nResOne, nResTwo;
            GetOptimalPosition(out nResOne, out nResTwo);

            Console.WriteLine("Part one: {0, 6:0}", nResOne);
            Console.WriteLine("Part one: {0, 6:0}", nResTwo);
        }

        private static void GetOptimalPosition(out int nResOne, out int nResTwo)
        {
            nResOne = nResTwo = int.MaxValue;

            for (int posStart = Positions.Min(); posStart <= Positions.Max(); posStart++)
            {
                int nResOneT =0, nResTwoT = 0;
                foreach (int posEnd in Positions)
                {
                    int nDelta = Math.Abs(posStart - posEnd);
                    nResOneT += nDelta;
                    nResTwoT += (1+nDelta)*nDelta/2;
                }
                if (nResOneT < nResOne) 
                    nResOne = nResOneT;

                if (nResTwoT < nResTwo)
                    nResTwo = nResTwoT;
            }
        }

        private static void ParsingInputData()
        {
            List<string> InputData = new();
            using (StreamReader file = new(filePath))
            {
                string line = file.ReadLine();
                string[] parts = line.Split(",");

                foreach (string s in parts)
                    Positions.Add(int.Parse(s));
            }
        }
    }
}
