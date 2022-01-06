namespace MyApp
{
    public struct Coordinates
    {
        public int x { get; }
        public int y { get; }
        public int z { get; }
        public Coordinates(int x, int y, int z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public static List<Coordinates> CompareTwoCoordinates(Coordinates A, Coordinates B)
        {
             List<Coordinates> result = new List<Coordinates>();
            result.Add(new(A.x + B.x, A.y + B.y, A.z + B.z));
            result.Add(new(A.x + B.x, A.y + B.y, A.z - B.z));
            result.Add(new(A.x + B.x, A.y - B.y, A.z + B.z));
            result.Add(new(A.x + B.x, A.y - B.y, A.z - B.z));
            
            result.Add(new(A.x - B.x, A.y + B.y, A.z + B.z));
            result.Add(new(A.x - B.x, A.y + B.y, A.z - B.z));
            result.Add(new(A.x - B.x, A.y - B.y, A.z + B.z));
            result.Add(new(A.x - B.x, A.y - B.y, A.z - B.z));

            return result;
        }

        public static Coordinates operator -(Coordinates A, Coordinates B)
        {
            return new Coordinates(A.x - B.x, A.y - B.y, A.z - B.z);
        }

        public static Coordinates operator +(Coordinates A, Coordinates B)
        {
            return new Coordinates(A.x + B.x, A.y + B.y, A.z + B.z);
        }
    }
    public class Scanner
    {
        private int id { get; }
        public List<Coordinates> coordinates = new List<Coordinates>();

        public Scanner(int id)
        {
            this.id = id;
        }

        public List<Coordinates> CompareCoordinates(Scanner Target)
        {
            List<Coordinates> LDelta = new List<Coordinates>();

            foreach (Coordinates crdScannerA in this.coordinates)
                foreach (Coordinates crdScannerB in Target.coordinates)
                {
                    //Coordinates delta_crd = crdScannerA + crdScannerB;
                    List<Coordinates> LDelta2 = Coordinates.CompareTwoCoordinates(crdScannerA, crdScannerB);
                    LDelta.AddRange(LDelta2);
                }
            
            return LDelta;
        }

    }
    public class Program
    {
        // Answers for Data_p.txt  Part 1:      Part 2: 
        static readonly string filePath = @".\..\..\..\Data_t.txt";
        static List<Scanner> scanners = new List<Scanner>();
        public static void Main(string[] args)
        {
            ParsingInputData();
            List<Coordinates> LDelta = new List<Coordinates>();
            for (int i = 0; i < scanners.Count; i++)
                for (int k = i+1; k < scanners.Count; k++)
                {
                    LDelta.AddRange(scanners[i].CompareCoordinates(scanners[k]));
                }

            var query = LDelta.GroupBy(x => x).Where(g => g.Count() >= 12).Select(y => y).ToList();

            foreach (Coordinates crd in LDelta)
                Console.WriteLine("x={0}  y={1}  z={2}", crd.x, crd.y, crd.z);
            
            Console.WriteLine("Part one: {0, 10:0}", 1);
            Console.WriteLine("Part one: {0, 10:0}", 2);
        }


        private static void ParsingInputData()
        {
            int nScannerNum = 0;
            Scanner sc = new Scanner(nScannerNum);
            using (StreamReader file = new(filePath))
                while (!file.EndOfStream)
                {
                    string line = file.ReadLine();
                    
                    if (line.Contains("scanner"))
                        continue;

                    if(line == "")
                    {
                        scanners.Add(sc);
                        nScannerNum++;
                        sc = new Scanner(nScannerNum);
                        continue;
                    }

                    string[] parts = line.Split(",");
                    Coordinates crd = new Coordinates(int.Parse(parts[0]), int.Parse(parts[1]), int.Parse(parts[2]));
                    sc.coordinates.Add(crd);
                }
                scanners.Add(sc);
        }
    }
}
