using System.IO;
using System.Threading.Tasks;
namespace MyApp
{
    public class Program
    {
        // Answers for Data_p.txt  Part 1: 415     Part 2: 
        static readonly string filePath = @".\..\..\..\Data_p.txt";
        static List<int> InputData = new List<int>();

        static readonly int X = 100;
        static readonly int Y = 100;

        static int[,] Map = new int[X, Y];
        static int[,] InitialMap = new int[X, Y];
        static bool[,] VisitedMap = new bool[X, Y];
        public static void Main(string[] args)
        {
            ParsingInputData();
            //ShowMap();
            int nRes = int.MaxValue;

            int n = 40;
            while (n>0)
            {
                //ShowMap();
                for(int x = 0; x < X; x++)
                    for(int y = 0; y < Y; y++)
                    {
                        Wave(x, y);
                        //Map[n - i, i] = 0;
                        //ShowMap();
                        nRes = Map[X-1,Y-1] - Map[0,0];
                    }
                n--;
            }


            //ShowMap();
            Console.WriteLine("Part one: {0, 10:0}", nRes);
            Console.WriteLine("Part one: {0, 10:0}", 2);
        }

        private static void Wave(int x, int y)
        {
            //ShowMap();
            VisitedMap[x, y] = true;
            if (Map[x, y] > 2000) Map[x, y] = 2000;
            
            int nCost = Map[x, y];

            

            int[,] d = { { -1, 0 }, { 1, 0 }, { 0, -1 }, { 0, 1 }};

            for (int i = 0; i < 4; i++)
                if (isValidCoordinates(x + d[i, 0], y + d[i, 1]))
                {
                    if (VisitedMap[x + d[i, 0], y + d[i, 1]] == false)
                    {
                        Map[x + d[i, 0], y + d[i, 1]] += nCost;
                        //Wave(x + d[i, 0], y + d[i, 1]);
                    }

                    else
                        if (InitialMap[x + d[i, 0], y + d[i, 1]] + nCost < Map[x + d[i, 0], y + d[i, 1]])
                    {
                        Map[x + d[i, 0], y + d[i, 1]] = InitialMap[x + d[i, 0], y + d[i, 1]] + nCost;
                        //Wave(x + d[i, 0], y + d[i, 1]);
                    }
                   // if (VisitedMap[9, 9] == true)
                      //break;
                    //else
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
                        Map[x,y] = InitialMap[x, y];
                        x++;
                    }
                    y++;
                }
        }
        private static void ShowMap()
        {
            for (int y = 0; y < Y; y++)
            {

                string text = "";
                for (int x = 0; x < X; x++)
                {
                    Console.Write("{0, 4:0}", Map[x, y]);
                    text+=Map[x, y].ToString()+"  ";
                }

                 Console.WriteLine();
                //using StreamWriter file = new(@"e:\WriteLines2.txt", append: true);
                //    file.WriteLineAsync(text);
                Console.WriteLine();
            }
            Console.WriteLine();
        }
        public static bool isValidCoordinates(int x, int y)
        {
            return (x >= 0 && y >= 0 && x < X && y < Y) ? true : false;
        }
    }
}
