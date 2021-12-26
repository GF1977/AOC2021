namespace MyApp
{
    public class Node
    {
        public string Name { get; set; }
        public bool IsSmall { get; set; }
        public List<string> Leafs { get; set; }
        public Node()
        {
            Leafs = new List<string>();
            IsSmall = true;
        }
    }
    public class Program
    {
        // Answers for Data_p.txt  Part 1: 5178      Part 2: 130094
        static readonly string filePath = @".\..\..\..\Data_p.txt";
        static List<string> InputData = new List<string>();
        static Dictionary<string, Node> Nodes = new Dictionary<string, Node>();
        public static void Main(string[] args)
        {
            ParsingInputData();
            GenerateGraph();

            int nResOne = WalkEverywhere(1);
            int nResTwo = WalkEverywhere(2);

            Console.WriteLine("Part one: {0, 10:0}", nResOne);
            Console.WriteLine("Part one: {0, 10:0}", nResTwo);
        }

        private static int WalkEverywhere(int PuzzlePart)
        {
            List<string> queueA = new List<string>();
            List<string> queueB = new List<string>();
            List<string> Routes = new List<string>();

            queueA.Add("start");

            while(queueA.Count > 0)
            {
                foreach (string path in queueA)
                {
                    string Key = path.Split(",").Last();

                    foreach (string N in Nodes[Key].Leafs)
                    {
                        if(PuzzlePart == 1) // small caves can be visited only once
                            if (Nodes[N].IsSmall && path.Contains(N))
                                continue;

                        if (PuzzlePart == 2) // small caves can be visited only twice
                        {
                            string[] hops = path.Split(",");

                            bool bVisitedTwice = false;
                            foreach(var Node in Nodes)
                            {
                                if (Node.Value.IsSmall && hops.Count(n => n == Node.Key) == 2 ) 
                                    bVisitedTwice = true;
                            }

                            if (bVisitedTwice && Nodes[N].IsSmall && path.Contains(N))
                              continue;


                            if (N == "start" && path.Contains(N))
                                continue;
                        }

                        if (N == "end")
                            Routes.Add(path + "," + N);
                        else
                            queueB.Add(path + "," + N);
                    }
                }

                queueA.Clear();
                foreach (string s in queueB)
                    queueA.Add(s);

                queueB.Clear();
            }
            return Routes.Count;
        }

        private static void GenerateGraph()
        {
            foreach(string s in InputData)
            {
                string[] parts = s.Split("-");
                if (Nodes.ContainsKey(parts[0]) == false)
                {
                    Node N = new Node();
                    N.Name = parts[0];
                    N.Leafs.Add(parts[1]);

                    if (Char.IsLower(N.Name[0]))
                        N.IsSmall = true;
                    else
                        N.IsSmall = false;


                    Nodes.Add(N.Name, N);
                }
                else
                    Nodes[parts[0]].Leafs.Add(parts[1]);

                if (Nodes.ContainsKey(parts[1]) == false)
                {
                    Node N = new Node();
                    N.Name = parts[1];
                    N.Leafs.Add(parts[0]);


                    if (Char.IsLower(N.Name[0]))
                        N.IsSmall = true;
                    else
                        N.IsSmall = false;


                    Nodes.Add(N.Name, N);
                }
                else
                    Nodes[parts[1]].Leafs.Add(parts[0]);

            }
        }

        private static void ParsingInputData()
        {
            using (StreamReader file = new(filePath))
                while (!file.EndOfStream)
                {
                    string line = file.ReadLine();
                    InputData.Add(line);
                }
        }
    }
}
