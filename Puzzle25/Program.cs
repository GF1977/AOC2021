namespace MyApp
{    
    public class Program
    {
        // Answers for Data_p.txt  Part 1: 278       
        static char[,] map;
        static bool[,] FreeSlots;
        static int X, Y;
        public static void Main(string[] args)
        {
            ParsingInputData(@".\..\..\..\Data_p.txt");
            Console.WriteLine("Part one: {0, 10:0}", SolveProblem());
        }
        private static int SolveProblem()
        {
            bool bMoved = true;
            int nSteps = 0;

            while (bMoved)
            {
                bMoved = MoveCucumbersE("East");
                bool b = MoveCucumbersS("South");
                bMoved = bMoved || b;
                nSteps++;
            } 
            return nSteps;
        }
        private static void CheckFreeSlots()
        {
            FreeSlots = new bool[Y, X];

            for (int y = 0; y < Y; y++)
                for (int x = 0; x < X; x++)
                {
                    if (map[y, x] == '.')
                        FreeSlots[y, x] = true;
                    else
                        FreeSlots[y, x] = false;

                    if (map[y, x] == 'E') map[y, x] = '>';
                    if (map[y, x] == 'S') map[y, x] = 'v';
                }
        }
        private static bool MoveCucumbersE(string v)
        {
            CheckFreeSlots();
            bool bMoved = false;

            for (int y = 0; y < Y; y++)
                for (int x = 0; x < X; x++)
                {
                    if (map[y, x] == '>')
                        if (x < X - 1)
                        {
                            if (FreeSlots[y,x+1])
                            {
                                map[y, x] = '.';
                                map[y, x+1] = 'E';
                                bMoved = true;
                            }
                        }
                        else
                        {
                            if (FreeSlots[y,0])
                            {
                                map[y, x] = '.';
                                map[y, 0] = 'E';
                                bMoved = true;
                            }
                        }
                }
           return bMoved ;
        }
        private static bool MoveCucumbersS(string v)
        {
            CheckFreeSlots();
            bool bMoved = false;

            for (int y = 0; y < Y; y++)
                for (int x = 0; x < X; x++)
                {
                    if (map[y, x] == 'v')
                        if (y < Y - 1)
                        {
                            if (FreeSlots[y+1,x])
                            {
                                map[y, x] = '.';
                                map[y+1, x] = 'S';
                                bMoved = true;
                            }
                        }
                        else
                        {
                             if (FreeSlots[0,x])
                             {
                                map[y, x] = '.';
                                map[0, x] = 'S';
                                bMoved = true;
                            }
                        }
                }
            return bMoved;
        }
        private static void ParsingInputData(string filePath)
        {
            StreamReader file = new(filePath);
            string[] Data = file.ReadToEnd().Split("\r\n");
            X = Data[0].Length;
            Y = Data.Length;
            map = new char[Y, X];

            for (int y = 0; y < Y; y++)
                for (int x = 0; x < X; x++)
                    map[y,x] = Data[y][x];
        }
    }
}