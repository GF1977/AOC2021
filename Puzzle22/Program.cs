namespace MyApp
{
    public class Cuboid
    {
        private const int LIMIT = 50;
        public bool cmd { get; private set; }
        public (int Start, int End) X { get; }
        public (int Start, int End) Y { get; }
        public (int Start, int End) Z { get; }
        public long Size { get; private set; }
        public Cuboid((int Start, int End) x, (int Start, int End) y, (int Start, int End) z, bool cmd)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
            this.cmd = cmd;
            Size = (!isValid()) ? 0 : Abs(X.Start, X.End) * Abs(Y.Start, Y.End) * Abs(Z.Start, Z.End);
        }
        private long Abs(int Start, int End) => (long)(Math.Abs(Start - End) + 1); // +1 because 10..10 means not an empty cube but cube with side = 1
        public bool isWithinLIMIT() => X.Start >= -LIMIT && X.End <= LIMIT && Y.Start >= -LIMIT && Y.End <= LIMIT && Z.Start >= -LIMIT && Z.End <= LIMIT;
        public bool isValid() => (X.Start <= X.End) && (Y.Start <= Y.End) && (Z.Start <= Z.End);
        public static Cuboid operator ^(Cuboid C1, Cuboid C2) => new Cuboid(Intersection(C1.X, C2.X), Intersection(C1.Y, C2.Y), Intersection(C1.Z, C2.Z), !C1.cmd);
        private static (int Start, int End) Intersection((int Start, int End) C1, (int Start, int End) C2)
        {
            int Start = (C1.Start <= C2.Start) ? C2.Start : C1.Start;
            int   End = (C1.End <= C2.End) ? C1.End : C2.End;
            // (1, -1) impossible value => the intersection doesn't exist
            return (End < Start) ? (1, -1) : (Start, End);
        }
    }
    public class Program
    {
        // Answers for Data_p.txt  Part 1: 570915     Part 2: 1268313839428137
        static readonly string filePath = @".\..\..\..\Data_p.txt";
        static List<Cuboid> Cuboids = new List<Cuboid>();
        public static void Main(string[] args)
        {
            ParsingInputData();
            Console.WriteLine("Part one: {0, 18:0}", SolvePuzzle(Part: 1));
            Console.WriteLine("Part one: {0, 18:0}", SolvePuzzle(Part: 2));
        }
        private static long SolvePuzzle(int Part)
        {
            List<Cuboid> queue = new List<Cuboid>();
            List<Cuboid> queue_tail = new List<Cuboid>();
            foreach (Cuboid Cuboid in Cuboids)
            {
                if(Part == 1 && Cuboid.isWithinLIMIT() == false) continue; // for the Part One where we check init zone (within -50 .. +50) only
                if (Cuboid.isValid())
                {
                    if (Cuboid.cmd == true)
                        queue.Add(Cuboid);
                    
                    queue_tail.Clear();
                    foreach (Cuboid Q in queue)
                    {
                        Cuboid NewC = Q ^ Cuboid;
                        if (Q != Cuboid && NewC.Size > 0)
                            queue_tail.Add(NewC);
                    }
                    queue.AddRange(queue_tail);
                }
            }
            long Res = 0;
            foreach (Cuboid C in queue)
                Res += (C.cmd == true) ? C.Size : - C.Size;

            return Res;
        }
        private static void ParsingInputData()
        {
            using (StreamReader file = new(filePath))
                while (!file.EndOfStream)
                {
                    string line = file.ReadLine();
                    bool command = line.Split(" ")[0] == "on" ? true : false;
                    string[] parts = line.Split(",");
                    Cuboids.Add(new Cuboid(GetStartEnd(parts[0]), GetStartEnd(parts[1]), GetStartEnd(parts[2]), command));  
                }
        }
        private static (int,int) GetStartEnd(string Part)
        {
            string[] StartEnd = Part.Split("=")[1].Split("..");
            return (int.Parse(StartEnd[0]), int.Parse(StartEnd[1]));
        }
    }
}