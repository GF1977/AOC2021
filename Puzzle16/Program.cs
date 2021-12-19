namespace MyApp
{
    public class Program
    {
        // Answers for Data_p.txt  Part 1:      Part 2: 
        static readonly string filePath = @".\..\..\..\Data_t.txt";
        static List<string> InputData = new List<string>();
        public static void Main(string[] args)
        {
            ParsingInputData();

            string res = Convert.ToString(Convert.ToInt64("1234567890ABCDEF", 16), 2);
            Console.WriteLine("Part one: {0, 80:0}", res);
            Console.WriteLine("Part one: {0, 80:0}", 2);
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
