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
                bMoved = MoveCucumbers("East");
                bool b = MoveCucumbers("South");
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

                    if (map[y, x] == 'E') 
                        map[y, x] = '>';

                    if (map[y, x] == 'S') 
                        map[y, x] = 'v';
                }
        }
        private static bool MoveCucumbers(string v)
        {
            CheckFreeSlots();
            bool bMoved = false;
            int nLimit, xy, xEdge, yEdge;
            char nType, nTypeOccupied;
            (int dY, int dX) Delta;

            for (int y = 0; y < Y; y++)
                for (int x = 0; x < X; x++)
                {
                    if (v == "East")
                    {
                        nLimit = X - 1;
                        nType = '>';
                        nTypeOccupied = 'E';
                        xy = x;
                        xEdge = 0;
                        yEdge = y;
                        Delta = (0, 1);
                    }
                    else
                    {
                        nLimit = Y - 1;
                        nType = 'v';
                        nTypeOccupied = 'S';
                        xy = y;
                        xEdge = x;
                        yEdge = 0;
                        Delta = (1, 0);
                    }

                    if (map[y, x] == nType)
                        if (xy < nLimit)
                        {
                            if (FreeSlots[y + Delta.dY,x + Delta.dX])
                            {
                                map[y, x] = '.';
                                map[y+Delta.dY, x+Delta.dX] = nTypeOccupied;
                                bMoved = true;
                            }
                        }
                        else
                        {
                            if (FreeSlots[yEdge, xEdge])
                            {
                                map[y, x] = '.';
                                map[yEdge, xEdge] = nTypeOccupied;
                                bMoved = true;
                            }
                        }
                }
           return bMoved ;
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