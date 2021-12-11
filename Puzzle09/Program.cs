namespace MyApp
{
    public class Coordinates
    {
        public int x;
        public int y;
        public Coordinates(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

    }
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
            Dictionary<Coordinates, bool> bassin = new Dictionary<Coordinates, bool>();
            List<int> BassinSizes = new List<int>(); 

            int nRiskLevel = 0;
            for (int x = 0; x < nX; x++)
                for (int y = 0; y < nY; y++)
                {
                    if (GetLowPoint(x, y))
                    {
                        nRiskLevel+=(Map[x, y] + 1);
                        // Console.WriteLine("Lower point: {0}    Risk level: {1}", Map[x, y], nRiskLevel);

                        //int nBasinSize = GetBasinSize(x, y, bassin);
                        int nBasinSize = GetBasinSize(x, y);
                        Console.WriteLine("Bassin (x,y): {0},{1}     Size: {2}", x, y, nBasinSize);
                        BassinSizes.Add(nBasinSize);

                        //for (int i = 0; i < nX; i++)
                        //{
                        //    Console.WriteLine();
                        //    for (int j = 0; j < nY; j++)
                        //    {
                        //        if(Map[i, j] == -1)
                        //            Console.Write("*");
                        //        else
                        //            Console.Write(Map[i, j]);
                        //    }
                        //}
                    }
                }

                BassinSizes.Sort();
                BassinSizes.Reverse();
                int nTop3Bassins = BassinSizes[0] * BassinSizes[1]*BassinSizes[2];
                Console.WriteLine("BassinSizes: {0}", nTop3Bassins);

                return nRiskLevel;
        }

        private static int GetBasinSize(int x, int y)
        {
            int nRes = 0;

            if (x - 1 >= 0 && Map[x - 1, y] != 9 && Map[x - 1, y] >= 0)
            {
                Map[x - 1, y] = -1;
                nRes+= GetBasinSize(x - 1, y) + 1;
            }

            if (x + 1 < nX && Map[x + 1, y] != 9 && Map[x + 1, y] >= 0)
            {
                Map[x + 1, y] = -1;
                nRes += GetBasinSize(x + 1, y) + 1;
            }

            if (y - 1 >= 0 && Map[x, y - 1] != 9 && Map[x, y - 1] >= 0)
            {
                Map[x, y - 1] = -1;
                nRes += GetBasinSize(x, y - 1) + 1;
            }

            if (y + 1 < nY && Map[x, y + 1] != 9 && Map[x, y + 1] >= 0)
            {
                Map[x, y + 1] = -1;
                nRes += GetBasinSize(x, y + 1) + 1;
            }


            return nRes;
        }

        private static int GetBasinSize(int x, int y, Dictionary<Coordinates, bool> Bassin)
        {
            int nRes = 0;
            Coordinates crd = new Coordinates(x, y);

            if (Bassin.ContainsKey(crd))
                Bassin[crd] = true;
            else
                Bassin.Add(crd,true);

            if (x - 1 >= 0 && Map[x - 1, y] !=9)
                Bassin.Add(new Coordinates(x-1,y), false);

            if (x + 1 < nX && Map[x + 1, y] != 9)
                Bassin.Add(new Coordinates(x+1, y), false);

            if (y - 1 >= 0 && Map[x, y - 1] != 9)
                Bassin.Add(new Coordinates(x, y-1), false);

            if (y + 1 < nY && Map[x, y + 1] != 9)
                Bassin.Add(new Coordinates(x, y+1), false);

            foreach (KeyValuePair<Coordinates, bool> kvp in Bassin)
                if (kvp.Value == false)
                    nRes += GetBasinSize(crd.x, crd.y, Bassin);


            return Bassin.Count;

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
