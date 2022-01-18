namespace MyApp
{
    public struct Crd
    {
        public int x { get; }
        public int y { get; }
        public int z { get; }
        public Crd(int x, int y, int z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public Crd Rotate(int n)
        {
            if (n ==  0) return new Crd(x, y, z);
            if (n ==  1) return new Crd(-x, -y, z);
            if (n ==  2) return new Crd(y, -x, z);
            if (n ==  3) return new Crd(-y, x, z);

            if (n ==  4) return new Crd(y, x, -z);
            if (n ==  5) return new Crd(-y, -x, -z);
            if (n ==  6) return new Crd(x, -y, -z);
            if (n ==  7) return new Crd(-x, y, -z);

            if (n ==  8) return new Crd(y, z, x);
            if (n ==  9) return new Crd(-y, -z, x);
            if (n == 10) return new Crd(z, -y, x);
            if (n == 11) return new Crd(-z, y, x);

            if (n == 12) return new Crd(z, y, -x);
            if (n == 13) return new Crd(-z, -y, -x);
            if (n == 14) return new Crd(y, -z, -x);
            if (n == 15) return new Crd(-y, z, -x);

            if (n == 16) return new Crd(z, x, y);
            if (n == 17) return new Crd(-z, -x, y);
            if (n == 18) return new Crd(x, -z, y);
            if (n == 19) return new Crd(-x, z, y);

            if (n == 20) return new Crd(x, z, -y);
            if (n == 21) return new Crd(-x, -z, -y);
            if (n == 22) return new Crd(z, -x, -y);
            if (n == 23) return new Crd(-z, x, -y);

            throw new ArgumentOutOfRangeException();
        }
        public static Crd operator -(Crd A, Crd B) => (new Crd(A.x - B.x, A.y - B.y, A.z - B.z));
        public static Crd operator +(Crd A, Crd B) => (new Crd(A.x + B.x, A.y + B.y, A.z + B.z));
        public int ManhattanDistance(Crd B) => Math.Abs(x - B.x) + Math.Abs(y - B.y) + Math.Abs(z - B.z);
    }
    public class Scanner
    {
        public HashSet<Crd> beacons_coordinates = new HashSet<Crd>();
        public bool FindOverlappingScanner(Scanner Target, int nSRotation, out Crd relativeDistance )
        {
            List<Crd> Overlapping = new List<Crd>();

            foreach (Crd beaconA in this.beacons_coordinates)
                foreach (Crd beaconB in Target.beacons_coordinates)
                {
                    Crd newCrd = beaconA - beaconB.Rotate(nSRotation);
                    Overlapping.Add(newCrd);
                }

            var query = Overlapping.GroupBy(x => x).Where(g => g.Count() >= 12).Select(y => y).ToList();
            relativeDistance = new Crd();
            if (query.Count() > 0)
            {
                relativeDistance = query[0].Key;
                return true;
            }

            return false;
        }
    }
    public class Program
    {
        // Answers for Data_t.txt    Part 1:  79     Part 2  3621: 
        // Answers for Data_p0.txt   Part 1: 313     Part 2 10656: 

        static List<Scanner> scanners = new List<Scanner>();
        static List<Crd> scannersCoordinates = new List<Crd>();
        public static void Main(string[] args)
        {
            ParsingInputData(@".\..\..\..\Data_p0.txt");
            Console.WriteLine("Part one: {0, 10:0}", SolvePartOne());
            Console.WriteLine("Part two: {0, 10:0}", SolvePartTwo());

        }
        private static int SolvePartOne()
        {
            Scanner S0 = scanners[0];
            while (scanners.Count != 1)
                for (int k = 1; k < scanners.Count; k++)
                    for (int nSRotation = 0; nSRotation < 24; nSRotation++)
                    {
                        bool found = S0.FindOverlappingScanner(scanners[k], nSRotation, out Crd relativeDistance);
                        if (found)
                        {
                            foreach (Crd b_crd in scanners[k].beacons_coordinates)
                                S0.beacons_coordinates.Add(b_crd.Rotate(nSRotation) + relativeDistance);

                            scannersCoordinates.Add(relativeDistance);
                            scanners.RemoveAt(k);
                            break;
                        }
                    }
            return S0.beacons_coordinates.Select(c => c).Distinct().ToList().Count();
        }
        private static int SolvePartTwo()
        {
            int nMax = 0;
            for (int i = 0; i < scannersCoordinates.Count; i++)
                for (int k = i+1; k < scannersCoordinates.Count; k++)
                {
                    int n = scannersCoordinates[i].ManhattanDistance(scannersCoordinates[k]);
                    nMax = (n > nMax) ? n:nMax; 
                }
            return nMax;
        }
        private static void ParsingInputData(string filePath)
        {
            Scanner sc = new Scanner();

            using (StreamReader file = new(filePath))
                while (!file.EndOfStream)
                {
                    string line = file.ReadLine();

                    if (line.Contains("scanner"))
                        continue;

                    if (line == "")
                    {
                        scanners.Add(sc);
                        sc = new Scanner();
                        continue;
                    }

                    string[] parts = line.Split(",");
                    Crd crd = new Crd(int.Parse(parts[0]), int.Parse(parts[1]), int.Parse(parts[2]));
                    sc.beacons_coordinates.Add(crd);
                }
            scanners.Add(sc);
        }
    }
}