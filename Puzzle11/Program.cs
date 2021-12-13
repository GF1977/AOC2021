
namespace MyApp
{
    public class Program
    {
        // Answers for Data_p.txt  Part 1:      Part 2: 
        static readonly string filePath = @".\..\..\..\Data_p.txt";
        static readonly int X = 10;
        static readonly int Y = 10;

        static int nFlashes = 0;
        static int[,] OctopusMap = new int[X, Y];
        public static void Main(string[] args)
        {
            ParsingInputData();
            int nResOne = 0;
            int nResTwo = 0;

            for (int nStep = 0; nStep < 100 || nResTwo == 0; nStep++)
            {
                if (nStep == 100)
                    nResOne = nFlashes;

                if (CheckBigFlash() && nResTwo == 0)
                    nResTwo = nStep;

                IncreaseEnergyByOne();
                InitiateFlash();
            }

            Console.WriteLine("Part one: {0, 10:0}", nResOne);
            Console.WriteLine("Part one: {0, 10:0}", nResTwo);
        }
        private static void InitiateFlash()
        {
            bool bChainReaction = false;

            for (int x = 0; x < X; x++)
                for (int y = 0; y < Y; y++)
                {
                    if (OctopusMap[x, y] > 9)
                    {
                        OctopusMap[x, y] = -1000;
                        IncreaseEnergyOfAdjusted(x, y);
                        bChainReaction = true;
                    }
                }

            if (bChainReaction)
                InitiateFlash();

            for (int x = 0; x < X; x++)
                for (int y = 0; y < Y; y++)
                    if (OctopusMap[x, y] < 0)
                    {
                        OctopusMap[x, y] = 0;
                        nFlashes++;
                    }
        }
        private static void ShowTheMap()
        {
            for (int y = 0; y < Y; y++)
            {
                for (int x = 0; x < X; x++)
                    Console.Write(OctopusMap[x,y].ToString());

                Console.WriteLine();
            }
        }

        private static bool CheckBigFlash()
        {
            int n = 0;
            for (int x = 0; x < X; x++)
                for (int y = 0; y < Y; y++)
                    if (OctopusMap[x, y] == 0) n++;

            return n==100 ? true : false;
        }

        private static void IncreaseEnergyByOne()
        {
            for (int x = 0; x < X; x++)
                for (int y = 0; y < Y; y++)
                    OctopusMap[x, y]++;
        }
        private static void IncreaseEnergyOfAdjusted(int x, int y)
        {
            int[,] d = { { -1, 0 }, { 1, 0 }, { 0, -1 }, { 0, 1 }, { -1, -1 }, { -1, 1 }, { 1, -1 }, { 1, 1 } };

            for (int i = 0; i < 8; i++)
                if (isValidCoordinates(x + d[i, 0], y + d[i, 1]))
                {
                    OctopusMap[x + d[i, 0], y + d[i, 1]]++;
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
                        OctopusMap[x++,y] = int.Parse(c.ToString());
                    y++;
                }
        }
        public static bool isValidCoordinates(int x, int y)
        {
            return (x >= 0 && y >= 0 && x < X && y < Y) ? true : false;
        }
    }
}