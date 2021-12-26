namespace MyApp
{
    public class Node
    {
        public string Name { get; set; }
        public bool IsVisited { get; set; }
        public bool IsLarge { get; set; }
        public List<string> Leafs { get; set; }
        public Node()
        {
            Leafs = new List<string>();
            IsVisited = false;
            IsLarge = false;
        }
    }
    public class Program
    {
        // Answers for Data_p.txt  Part 1: 5178      Part 2: 
        static readonly string filePath = @".\..\..\..\Data_t.txt";
        static List<string> InputData = new List<string>();
        static List<string> Route = new List<string>();
        static Dictionary<string, Node> Nodes = new Dictionary<string, Node>();
        public static void Main(string[] args)
        {
            ParsingInputData();
            GenerateGraph();
            Node nStart = Nodes["start"];

            WalkEverywhere(nStart);

            int sRes = WalkEverywhere(nStart);

            Console.WriteLine("Part one: {0, 10:0}", sRes);
            Console.WriteLine("Part one: {0, 10:0}", 2);
        }

        private static int WalkEverywhere(Node NStart)
        {
            List<string> queue = new List<string>();
            List<string> queue2 = new List<string>();
            List<string> Routes = new List<string>();

            queue.Add(NStart.Name);

            while(queue.Count > 0)
            {
                queue2.Clear();
                foreach (string path in queue)
                {
                    string[] Keys = path.Split(",");
                    string Key = Keys.Last();

                    Node kvp = Nodes[Key];
                    foreach (string N in kvp.Leafs)
                    {
                        
                        if (!Nodes[N].IsLarge && path.Contains(N))
                            continue;

                        if(N != "start" && !path.Contains("end"))
                            queue2.Add(path + "," + N);

                        if(N == "end")
                            Routes.Add(path + "," + N);
                    }


                }
                queue.Clear();
                foreach (string s in queue2)
                    queue.Add(s);
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

                    if (Char.IsUpper(N.Name[0]))
                        N.IsLarge = true;
                    else
                        N.IsLarge = false;


                    Nodes.Add(N.Name, N);
                }
                else
                    Nodes[parts[0]].Leafs.Add(parts[1]);

                if (Nodes.ContainsKey(parts[1]) == false)
                {
                    Node N = new Node();
                    N.Name = parts[1];
                    N.Leafs.Add(parts[0]);


                    if (Char.IsUpper(N.Name[0]))
                        N.IsLarge = true;
                    else
                        N.IsLarge = false;

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
