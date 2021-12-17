namespace MyApp
{
    public struct FoldInstruction
    {
        public string Axis;
        public int Value;
        public FoldInstruction(string axis, int value)
        {
            Axis = axis;
            Value = value;
        }
    }
    public class Program
    {
        const int ARR_SIZE = 2000;
        // Answers for Data_p.txt  Part 1: 745     Part 2: ABKJFBGC
        static readonly string filePath = @".\..\..\..\Data_p.txt";
        static List<FoldInstruction> FoldInstructions = new List<FoldInstruction>();
        static int[,] Paper = new int[ARR_SIZE, ARR_SIZE];
        public static void Main(string[] args)
        {
            ParsingInputData();
            FlatThePaper(FoldInstructions.First());
            int n = GetDotsCount();

            ParsingInputData();
            foreach (FoldInstruction fi in FoldInstructions)
                FlatThePaper(fi);
            
            Console.WriteLine("Part one: {0, 10:0}", n);
            ShowMap(FoldInstructions.Last(axis => axis.Axis == "x").Value, FoldInstructions.Last(axis => axis.Axis == "y").Value);
        }
        private static void ShowMap(int X, int Y)
        {
            for (int y = 0; y < Y; y++)
            {
                for (int x = 0; x < X; x++)
                    if (Paper[x, y] > 0)
                        Console.Write("#");
                    else
                        Console.Write(" ");
                Console.WriteLine();
            }
        }
        private static void FlatThePaper(FoldInstruction fi)
        {
            if (fi.Axis == "y")
                for (int x = 0; x < ARR_SIZE; x++)
                    for (int y = fi.Value * 2; y > fi.Value; y--)
                    {
                        Paper[x, fi.Value * 2 - y] += Paper[x, y];
                        Paper[x, y] = 0;
                    }

            if (fi.Axis == "x")
                for (int x = fi.Value * 2; x > fi.Value; x--)
                    for (int y = 0; y < ARR_SIZE; y++)
                    {
                        Paper[fi.Value * 2 - x, y] += Paper[x, y];
                        Paper[x, y] = 0;
                    }
        }
        private static int GetDotsCount()
        {
            int nRes = 0;
            for (int x = 0; x < ARR_SIZE; x++)
                for (int y = 0; y< ARR_SIZE;y++)
                    if (Paper[x, y] > 0)
                        nRes ++;

            return nRes;
        }
        private static void ParsingInputData()
        {
            bool bFirstPart = true;
            using (StreamReader file = new(filePath))
                while (!file.EndOfStream)
                {
                    string line = file.ReadLine();
                    if (line == "")
                    {
                        bFirstPart = false;
                        continue;
                    }
                    if (bFirstPart)
                    {
                        string[] parts = line.Split(",");
                        Paper[int.Parse(parts[0]), int.Parse(parts[1])] = 1;
                    }
                    else 
                    {
                        string[] parts  = line.Split("fold along ");
                        string[] parts2 = parts[1].Split("=");
                        FoldInstruction fi = new FoldInstruction(parts2[0],int.Parse(parts2[1]));
                        FoldInstructions.Add(fi);
                    }
                }
        }
    }
}