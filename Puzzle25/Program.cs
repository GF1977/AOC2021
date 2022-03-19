namespace MyApp
{    
    public class Program
    {
        // Answers for Data_p.txt  Part 1:      Part 2: 
        static char[,] map = new char[X, Y];
        static int X, Y;
        public static void Main(string[] args)
        {
            ParsingInputData(@".\..\..\..\Data_t.txt");

            int res = SolveProblem(1);

            Console.WriteLine("Part one: {0, 10:0}", res);
            Console.WriteLine("Part one: {0, 10:0}", 0);
        }


        private static int SolveProblem(int v)
        {
            int nMoved = 1;
            int nSteps = 0;

            while (nMoved > 0)
            {
                nMoved = MoveCucumbers("East") + MoveCucumbers("South");
                nSteps++;
            }

            return 0;
        }

        private static int MoveCucumbers(string v)
        {
            return 0;
        }

        private static void ParsingInputData(string filePath)
        {
            StreamReader file = new(filePath);
            string[] Data = file.ReadToEnd().Split("\r\n");
            
            // Max Size of the region
            Y = Data[0].Length;
            X = Data.Length;

            for (int x = 0; x < X; x++)
                for (int y = 0; y < Y; y++)
                {
                    map[x,y] = Data[x][y];
                }
        }
    }
}
