
namespace MyApp
{
    public class Program
    {
        // Answers for Data_p.txt  Part 1:      Part 2: 
        static readonly string filePath = @".\..\..\..\Data_p.txt";
        
        static List<string> UniqueSigPattern = new List<string>();
        static List<string> DigOutoutValue = new List<string>();
        public static void Main(string[] args)
        {
            ParsingInputData();

            int nRes = SolveThePuzzle();


            Console.WriteLine("Part one: {0, 6:0}", nRes);
            Console.WriteLine("Part one: {0, 6:0}", 0);
        }

        private static int SolveThePuzzle()
        {
            int nRes = 0;

            foreach (string s in DigOutoutValue)
                if(s.Length == 2 || s.Length == 3 || s.Length == 4 || s.Length == 7 )
                    nRes++;

            return nRes;
        }

        private static void ParsingInputData()
        {
            using (StreamReader file = new(filePath))
                while(!file.EndOfStream)
                {
                    string line = file.ReadLine();
                    string[] parts = line.Split(" | ");

                    string[] USP = parts[0].Split(" ");
                    string[] DOV = parts[1].Split(" ");

                    foreach (string s in USP)
                        UniqueSigPattern.Add(s);

                    foreach (string s in DOV)
                        DigOutoutValue.Add(s);
                }
        }
    }
}
