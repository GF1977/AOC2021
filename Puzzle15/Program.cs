namespace MyApp
{
    public class Program
    {
        // Answers for Data_p.txt  Part 1: 415     Part 2: 2864
        static readonly string filePath = @".\..\..\..\Data_t.txt";
        // !!! change the X & Y. for the test X=Y=10 , for prod X=Y=100
        static int X = 10;
        static int Y = 10;

        static int[,] Map = new int[X, Y];
        static int[,] InitialMap = new int[X, Y];
        static bool[,] VisitedMap = new bool[X, Y];
        public static void Main(string[] args)
        {
            ParsingInputData();
            Console.WriteLine("Part one: {0, 10:0}", FindSafeRoute());
            
            ScanWholeCave();
            Console.WriteLine("Part Two: {0, 10:0}", FindSafeRoute());
        }
        private static int FindSafeRoute()
        {
            // Recursive Wave function is too expensive, run it in iterations (6 is enough for good result)
            int n = 6;
            while (--n > 0)
                for (int x = 0; x < X; x++)
                    for (int y = 0; y < Y; y++)
                        Wave(x, y);

            return Map[X - 1, Y - 1] - Map[0, 0];
        }
        private static void ScanWholeCave()
        {
            int nOldDimension = X;
            X *= 5;
            Y *= 5;

            Map = new int[X, Y];
            VisitedMap = new bool[X, Y];

            int nRiskLevelDelta = 0;

            for(int x = 0; x < X; x++)
                for(int y = 0;y < Y; y++)
                {
                    nRiskLevelDelta = y / nOldDimension + x / nOldDimension;
                    int nNewValue = InitialMap[x % nOldDimension, y % nOldDimension] + nRiskLevelDelta;

                    nNewValue = (nNewValue > 9) ? nNewValue % 9 : nNewValue;
                    Map[x,y] = nNewValue;
                }

            InitialMap = new int[X, Y];
            for (int x = 0; x < X; x++)
                for (int y = 0; y < Y; y++)
                    InitialMap[x, y] = Map[x, y];
        }
        private static void Wave(int x, int y)
        {
            VisitedMap[x, y] = true;
            int[,] d = { { -1, 0 }, { 1, 0 }, { 0, -1 }, { 0, 1 } };
            int nCost = Map[x, y];

            for (int i = 0; i < 4; i++)
            {
                int nXnew = x + d[i, 0];
                int nYnew = y + d[i, 1];
                if (nXnew >= 0 && nYnew >= 0 && nXnew < X && nYnew < Y)
                    if (VisitedMap[nXnew, nYnew] == false || InitialMap[nXnew, nYnew] + nCost < Map[nXnew, nYnew])
                        Map[nXnew, nYnew] = InitialMap[nXnew, nYnew] + nCost;
            }
        }
        private static void ParsingInputData()
        {
            int y = 0;
            using (StreamReader file = new(filePath))
                while (!file.EndOfStream)
                {
                    string line = file.ReadLine();
                    int x = 0;
                    foreach (char c in line)
                    {
                        InitialMap[x, y] = int.Parse(c.ToString());
                        Map[x, y] = InitialMap[x, y];
                        x++;
                    }
                    y++;
                }
        }
    }
}