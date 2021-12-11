namespace MyApp
{
    public class Program
    {
        // Answers for Data_p.txt  Part 1: 444     Part 2: 1168440
        static readonly string filePath = @".\..\..\..\Data_p.txt";
        static List<string> InputData = new List<string>();
        static int[,] Map;
        static int nX, nY;
        public static void Main(string[] args)
        {
            ParsingInputData();

            int nRiskLevel, nTop3Bassins;
            SolvePuzzle(out nRiskLevel, out nTop3Bassins);

            Console.WriteLine("Part one: {0, 10:0}", nRiskLevel);
            Console.WriteLine("Part one: {0, 10:0}", nTop3Bassins);
        }

        private static void SolvePuzzle(out int nRiskLevel, out int nTop3Bassins)
        {
            List<int> BassinSizes = new List<int>(); 

            nRiskLevel = 0;
            for (int x = 0; x < nX; x++)
                for (int y = 0; y < nY; y++)
                    if (GetLowPoint(x, y))
                    {
                        nRiskLevel += (Map[x, y] + 1);
                        BassinSizes.Add(GetBasinSize(x, y));
                    }

                BassinSizes.Sort();
                BassinSizes.Reverse();
                nTop3Bassins = BassinSizes[0] * BassinSizes[1] * BassinSizes[2];
        }

        public static bool isValidCoordinates(int x, int y)
        {
            return  (x >=0 && y >= 0 && x < nX && y < nY) ?  true : false;
        }

        private static int GetBasinSize(int x, int y)
        {
            int nRes = 0;
            int[,] d = { { -1, 0 }, { 1, 0 }, { 0, -1 }, { 0, 1 } };

            for (int i = 0; i < 4; i++)
                if (isValidCoordinates(x + d[i, 0], y + d[i, 1]) && Map[x + d[i, 0], y + d[i, 1]] != 9)
                {
                    Map[x + d[i, 0], y + d[i, 1]] = 9;
                    nRes += GetBasinSize(x + d[i, 0], y + d[i, 1]) + 1;
                }

            return nRes;
        }

         
        private static bool GetLowPoint(int x, int y)
        {
            int nPoint = Map[x, y];
            int[,] d = { { -1, 0 }, { 1, 0 }, { 0, -1 }, { 0, 1 } };

            for (int i = 0; i < 4; i++)
                if (isValidCoordinates(x + d[i, 0], y + d[i, 1]))
                    if (Map[x + d[i, 0], y + d[i, 1]] <= nPoint)
                        return false;

            return true;
        }

        private static void ParsingInputData()
        {
            using (StreamReader file = new(filePath))
                while (!file.EndOfStream)
                {
                    string line = file.ReadLine();
                    InputData.Add(line);
                }

            nX = InputData.Count;
            nY = InputData.First().Length; 

            Map = new int[nX,nY];
            int x = 0;
            foreach (string line in InputData)
            {
                for (int i = 0; i < nY; i++)
                {
                    Map[x, i] = int.Parse(line[i].ToString());
                }
                x++;
            }
        }
    }
}
