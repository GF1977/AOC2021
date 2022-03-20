namespace MyApp
{    
    public class Cucumber
    {
        public (int Y, int X) Coordinates;
        private (int Y, int X) NewCoordinates;
        static (int Y, int X) Edge;
        public char Type;
        bool canBeMoved = false;

        public Cucumber((int Y, int X) Crd, (int Y, int X) Ed, char Tp)
        {
            Coordinates = Crd;
            Edge = Ed;
            Type = Tp;
        }
        public bool Move()
        {
            if (canBeMoved == false)
                return false;

            Coordinates = NewCoordinates;
            return true;
        }
        public bool isMovable(List<Cucumber> Cucumbers)
        {
            (int newY, int newX) newCrd;

            if (Type == '>')
                newCrd = (Coordinates.Y, (Coordinates.X + 1) % Edge.X);
            else
                newCrd = ((Coordinates.Y + 1) % Edge.Y, Coordinates.X);

            canBeMoved = false;

            if (Cucumbers.FindIndex(c => c.Coordinates == newCrd) == -1)
            {
                canBeMoved = true;
                NewCoordinates = newCrd;
            }

            return canBeMoved;
        }
    }
    public class Program
    {
        // Answers for Data_p.txt  Part 1: 278       
        static int X, Y;
        static List<Cucumber> Cucumbers = new List<Cucumber>();
        public static void Main(string[] args)
        {
            ParsingInputData(@".\..\..\..\Data_p.txt");

            int nSteps = 0;

            bool bMoved = true;
            while (bMoved)
            {
                ScanTheField('>');
                bMoved = MoveCucumbers('>');
                ScanTheField('v');
                bool b = MoveCucumbers('v');
                bMoved = bMoved || b;

                nSteps++;
            }

            Console.WriteLine("Part one: {0, 10:0}", nSteps);
        }
        private static void ScanTheField(char Type)
        {
            foreach(Cucumber c in Cucumbers)
                if (c.Type == Type)
                    c.isMovable(Cucumbers);
        }
        private static bool MoveCucumbers(char Type)
        {
            bool res = false;
            foreach(Cucumber c in Cucumbers)
                if(c.Type == Type)
                {
                   bool b = c.Move();
                   if(b) res = true;
                }
            return res;
        }
        private static void ParsingInputData(string filePath)
        {
            StreamReader file = new(filePath);
            string[] Data = file.ReadToEnd().Split("\r\n");
            X = Data[0].Length;
            Y = Data.Length;

            for (int y = 0; y < Y; y++)
                for (int x = 0; x < X; x++)
                    if(Data[y][x] != '.')
                        Cucumbers.Add(new Cucumber((y, x), (Y, X), Data[y][x]));
        }
    }
}
