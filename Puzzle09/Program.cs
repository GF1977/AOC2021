namespace MyApp
{
    public class Program
    {
        // Answers for Data_p.txt  Part 1:      Part 2: 
        static readonly string filePath = @".\..\..\..\Data_p.txt";
        static List<string> InputData = new List<string>();
        static int[,] Map;
        static int nX, nY;
        public static void Main(string[] args)
        {
            ParsingInputData();

            int nRiskLevel = SolvePuzzle();

            Console.WriteLine("Part one: {0, 6:0}", nRiskLevel);
            Console.WriteLine("Part one: {0, 6:0}", 0);
        }

        private static int SolvePuzzle()
        {
            int nRiskLevel = 0;
            for (int x = 0; x < nX; x++)
                for (int y = 0; y < nY; y++)
                {
                    if (GetLowPoint(x, y))
                    {
                        nRiskLevel+=(Map[x, y] + 1);
                       // Console.WriteLine("Lower point: {0}    Risk level: {1}", Map[x, y], nRiskLevel);
                    }
                }
                return nRiskLevel;
        }

        private static bool GetLowPoint(int x, int y)
        {
            int nPoint = Map[x, y];
            List<int> points = new List<int>();

            if (x - 1 >= 0)
                points.Add(Map[x - 1, y]);

            if (x + 1 < nX)
                points.Add(Map[x + 1, y]);

            if (y - 1 >= 0)
                points.Add(Map[x, y - 1]);

            if (y + 1 < nY)
                points.Add(Map[x, y + 1]);

            foreach (int i in points)
                if (i <= nPoint)
                    return false;

            //foreach (int i in points)
                //Console.Write("{0} ",i);

            //Console.WriteLine(" - {0}", nPoint);

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
