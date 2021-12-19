namespace MyApp
{
    public class Program
    {
        // Answers for Data_p.txt  Part 1: 415     Part 2: 2864
        static readonly string filePath = @".\..\..\..\Data_p.txt";
        static List<int> InputData = new List<int>();

        // !!! change the X & Y. for the test X=Y=10 , for prod X=Y=100
        static int X = 100;
        static int Y = 100;

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
            // Recursive Wave function is too expensive, run it in iterations
            int n = 10;
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

                    if (nNewValue > 9)
                        nNewValue = nNewValue % 9;
                    
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
                if (isValidCoordinates(nXnew, nYnew))
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
        public static bool isValidCoordinates(int x, int y)
        {
            return (x >= 0 && y >= 0 && x < X && y < Y) ? true : false;
        }
    }
}