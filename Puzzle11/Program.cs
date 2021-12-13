
namespace MyApp
{
    public class Program
    {
        // Answers for Data_p.txt  Part 1:1723      Part 2:327 
        static readonly string filePath = @".\..\..\..\Data_t.txt";
        static readonly int X = 10;
        static readonly int Y = 10;
        
        static int[,] OctopusMap = new int[X, Y];
        public static void Main(string[] args)
        {
            int nFlashes = 0;
            ParsingInputData();
            // 1000 is a random limit to avoid infinite loop
            for (int nStep = 0; nStep < 1000; nStep++)
            {
                if (nStep == 100)
                    Console.WriteLine("Part one: {0, 10:0}", nFlashes);

                if (CheckBigFlash())
                {
                    Console.WriteLine("Part one: {0, 10:0}", nStep);
                    break;
                }

                nFlashes+=RunOneCycle();
            }
        }
        private static int RunOneCycle()
        {
            // increasing energy level of each octopus by 1
            for (int x = 0; x < X; x++)
                for (int y = 0; y < Y; y++)
                    OctopusMap[x, y]++;

            // initiate chain reaction
            bool bChainReaction = true;
            while (bChainReaction)
            {
                bChainReaction = false;
                for (int x = 0; x < X; x++)
                    for (int y = 0; y < Y; y++)
                        if (OctopusMap[x, y] > 9)
                        {
                            OctopusMap[x, y] = -1000; // -1000 is octopus who flashed this cycle
                            IncreaseEnergyOfNearestOctopus(x, y);
                            bChainReaction = true;
                        }
            }
            // return flashed octopus to 0 level of energy
            int nFlashes = 0;
            for (int x = 0; x < X; x++)
                for (int y = 0; y < Y; y++)
                    if (OctopusMap[x, y] < 0)
                    {
                        OctopusMap[x, y] = 0;
                        nFlashes++;
                    }
            
            return nFlashes;
        }
        private static bool CheckBigFlash()
        {
            int n = 0;
            for (int x = 0; x < X; x++)
                for (int y = 0; y < Y; y++)
                    if (OctopusMap[x, y] == 0) n++;

            return n==100 ? true : false;
        }
        private static void IncreaseEnergyOfNearestOctopus(int x, int y)
        {
            int[,] d = { { -1, 0 }, { 1, 0 }, { 0, -1 }, { 0, 1 }, { -1, -1 }, { -1, 1 }, { 1, -1 }, { 1, 1 } };

            for (int i = 0; i < 8; i++)
                if (isValidCoordinates(x + d[i, 0], y + d[i, 1]))
                    OctopusMap[x + d[i, 0], y + d[i, 1]]++;
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