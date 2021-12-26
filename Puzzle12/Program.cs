namespace MyApp
{
    public class Cave
    {
        public bool IsSmall;
        public List<string> Leafs = new List<string>();
        public Cave(string root, string leaf)
        {
            Leafs.Add(leaf);
            IsSmall = (Char.IsLower(root[0]))?  true: false;
        }
    }
    public class Program
    {
        // Answers for Data_p.txt  Part 1: 5178      Part 2: 130094
        static readonly string filePath = @".\..\..\..\Data_t.txt";
        static Dictionary<string, Cave> Caves = new Dictionary<string, Cave>();
        public static void Main(string[] args)
        {
            ParsingInputData();

            Console.WriteLine("Part one: {0, 10:0}", GetRoutes(1));
            Console.WriteLine("Part two: {0, 10:0}", GetRoutes(2));
        }
        private static int GetRoutes(int PuzzlePart)
        {
            int nRes = 0;
            List<string> queueA = new List<string>();
            List<string> queueB = new List<string>();

            queueA.Add("start");

            while(queueA.Count > 0)
            {
                foreach (string path in queueA)
                {
                    string Key = path.Split(",").Last();
                    foreach (string Name in Caves[Key].Leafs.Where(n => n != "start"))
                    {
                        // this condition is enough for the first part of the puzzle
                        if (Caves[Name].IsSmall && path.Contains(Name))
                        {
                            bool bVisitedTwice = false;
                            if (PuzzlePart == 2 )  
                            {
                                string[] hops = path.Split(",");
                                foreach (var Cave in Caves.Where(n => n.Value.IsSmall))
                                    if (hops.Count(n => n == Cave.Key) == 2) // small caves can be visited only twice
                                    {
                                        bVisitedTwice = true;
                                            break;
                                    }
                            }
                            if (bVisitedTwice || PuzzlePart == 1)
                                continue;
                        }
                        if (Name == "end")
                            nRes++;
                        else
                            queueB.Add(path + "," + Name);
                    }
                }
                queueA.Clear();
                foreach (string s in queueB)
                    queueA.Add(s);

                queueB.Clear();
            }
            return nRes;
        }
        private static void AddCaveInfo(string root, string leaf)
        {
            if (Caves.ContainsKey(root))
                Caves[root].Leafs.Add(leaf);
            else
                Caves.Add(root, new Cave(root, leaf));
        }
        private static void ParsingInputData()
        {
            using (StreamReader file = new(filePath))
                while (!file.EndOfStream)
                {
                    string line = file.ReadLine();
                    string[] parts = line.Split("-");
                    AddCaveInfo(parts[0], parts[1]);
                    AddCaveInfo(parts[1], parts[0]);
                }
        }
    }
}
