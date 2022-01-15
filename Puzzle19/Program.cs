namespace MyApp
{
    public struct Coordinate
    {
        public int x { get; }
        public int y { get; }
        public int z { get; }
        public Coordinate(int x, int y, int z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public static List<Coordinate> CompareTwoCoordinate(Coordinate A, Coordinate B, int nSRotation)
        {
            List<Coordinate> result = new List<Coordinate>();
            Coordinate newB = RotateCoordinate(B, nSRotation);
            Coordinate Result = A - newB;

            result.Add(Result);
            return result;
        }
        public static Coordinate RotateCoordinateOpposite(Coordinate crd, int n)
        {
            if (n == 2) return RotateCoordinate(crd, 3);
            if (n == 8) return RotateCoordinate(crd, 11);
            if (n == 9) return RotateCoordinate(crd, 16);
            if (n == 10) return RotateCoordinate(crd, 18);
            if (n == 13) return RotateCoordinate(crd, 19);
            if (n == 14) return RotateCoordinate(crd, 17);
            if (n == 15) return RotateCoordinate(crd, 20);

            if (n == 3) return RotateCoordinate(crd, 2);
            if (n == 11) return RotateCoordinate(crd, 8);
            if (n == 16) return RotateCoordinate(crd, 9);
            if (n == 17) return RotateCoordinate(crd, 14);
            if (n == 18) return RotateCoordinate(crd, 10);
            if (n == 19) return RotateCoordinate(crd, 13);
            if (n == 20) return RotateCoordinate(crd, 15);

            return RotateCoordinate(crd, n);
        }

        public static Coordinate RotateCoordinate(Coordinate crd, int n)
        {
            int x = crd.x;
            int y = crd.y;
            int z = crd.z;

            if (n < 0 || n > 23) throw new ArgumentOutOfRangeException();

            if (n == 0) return new Coordinate(x, y, z);
            if (n == 1) return new Coordinate(x, -y, -z);
            if (n == 2) return new Coordinate(x, z, -y);
            if (n == 3) return new Coordinate(x, -z, y);
            if (n == 4) return new Coordinate(-x, y, -z);
            if (n == 5) return new Coordinate(-x, -y, z);
            if (n == 6) return new Coordinate(-x, z, y);
            if (n == 7) return new Coordinate(-x, -z, -y);

            if (n == 8) return new Coordinate(y, -x, z);
            if (n == 9) return new Coordinate(y, z, x);
            if (n == 10) return new Coordinate(y, -z, -x);
            if (n == 11) return new Coordinate(-y, x, z);
            if (n == 12) return new Coordinate(-y, -x, -z);
            if (n == 13) return new Coordinate(-y, z, -x);
            if (n == 14) return new Coordinate(-y, -z, x);
            if (n == 15) return new Coordinate(z, y, -x);

            if (n == 16) return new Coordinate(z, x, y);
            if (n == 17) return new Coordinate(z, -x, -y);
            if (n == 18) return new Coordinate(-z, x, -y);
            if (n == 19) return new Coordinate(-z, -x, y);
            if (n == 20) return new Coordinate(-z, y, x);
            if (n == 21) return new Coordinate(-z, -y, -x);
            if (n == 22) return new Coordinate(y, x, -z);
            if (n == 23) return new Coordinate(z, -y, x);

            return new Coordinate(0, 0, 0);
        }
        public static Coordinate operator -(Coordinate A, Coordinate B)
        {
            return new Coordinate(A.x - B.x, A.y - B.y, A.z - B.z);
        }

        public static Coordinate operator +(Coordinate A, Coordinate B)
        {
            return new Coordinate(A.x + B.x, A.y + B.y, A.z + B.z);
        }


        internal void Print()
        {
            Console.WriteLine("Coordinate {0}, {1}, {2}", x, y, z);
        }
    }
    public class Scanner
    {
        public int id { get; }
        public Coordinate AbsoluteCrd = new Coordinate(0, 0, 0);
        public Coordinate relativeCrd = new Coordinate(0, 0, 0);
        public int relative_to_scanner = -1;
        public int rotation_number = -1;
        public bool isVisited = false;

        public List<Coordinate> beacons_Coordinate = new List<Coordinate>();

        public Scanner(int id)
        {
            this.id = id;
        }

        public List<Coordinate> CompareCoordinate(Scanner Target, int nSRotation)
        {
            List<Coordinate> LDelta = new List<Coordinate>();

            foreach (Coordinate crdScannerA in this.beacons_Coordinate)
                foreach (Coordinate crdScannerB in Target.beacons_Coordinate)
                {
                    List<Coordinate> LDelta2 = Coordinate.CompareTwoCoordinate(crdScannerA, crdScannerB, nSRotation);
                    LDelta.AddRange(LDelta2);
                }

            return LDelta;
        }

        internal void Print()
        {
            Console.WriteLine("Scanner {0} :  Coordinate {1}, {2}, {3}     Beacons: {4}", id, AbsoluteCrd.x, AbsoluteCrd.y, AbsoluteCrd.z, beacons_Coordinate.Count());
        }




        internal List<Coordinate> MoveBeaconsTo(Scanner target)
        {
            List<Coordinate> Beacons = new List<Coordinate>();

            foreach (Coordinate b_target_crd in target.beacons_Coordinate)
            {
                // Coordinate new_b_crd = Coordinate.RotateCoordinate(b_target_crd, this.relative_scanner_rotation_number);
                Beacons.Add(b_target_crd);
            }

            foreach (Coordinate b_this_crd in this.beacons_Coordinate)
            {
                Coordinate new_b_crd = this.relativeCrd + Coordinate.RotateCoordinate(b_this_crd, this.rotation_number);
                Beacons.Add(new_b_crd);
            }

            var w3 = Beacons.GroupBy(x => x).Where(g => g.Count() >= 2).Select(y => y).ToList();
            var w2 = Beacons.Select(c => c).Distinct().ToList();

            target.beacons_Coordinate.Clear();
            foreach (Coordinate crd in Beacons)
            {
                //Coordinate crd_new = Coordinate.RotateCoordinateOpposite(crd, this.relative_scanner_rotation_number);
                target.beacons_Coordinate.Add(crd);
            }

            return Beacons;
        }

    }
    public class Program
    {
        // Answers for Data_p.txt  Part 1: 313     Part 2: 
        //static string filePath = @".\..\..\..\Data_p_alternative.txt";
        //static string filePath = @".\..\..\..\Data_p.txt";
        static string filePath = @".\..\..\..\Data_t.txt";
        static List<(int, int)> Route = new List<(int, int)>();
        static List<Scanner> scanners = new List<Scanner>();
        static Dictionary<int, List<int>> Order = new Dictionary<int, List<int>>();
        public static void Main(string[] args)
        {
            ParsingInputData();
            List<Coordinate> LDelta = new List<Coordinate>();

            for (int a = 0; a < scanners.Count; a++)
            {
                List<int> relative_scanners = new List<int>();
                Order.Add(a, relative_scanners);
            }

            scanners[0].AbsoluteCrd = new Coordinate(0, 0, 0);
            scanners[0].rotation_number = 0;

            for (int i = 0; i < scanners.Count; i++)
                for (int k = 1; k < scanners.Count; k++)
                {
                    if (k == i) continue;
                    for (int nSRotation = 0; nSRotation < 24; nSRotation++)
                    {
                        LDelta.Clear();
                        LDelta.AddRange(scanners[i].CompareCoordinate(scanners[k], nSRotation));
                        var query = LDelta.GroupBy(x => x).Where(g => g.Count() >= 12).Select(y => y).ToList();
                        if (query.Count() > 0)
                        {
                            if (Order.ContainsKey(k))
                                if (!Order[k].Contains(i))
                                    Order[k].Add(i);

                            if (scanners[k].rotation_number == -1)
                            {
                                Coordinate crd = query[0].Key;
                                scanners[k].relativeCrd = crd;
                                scanners[k].rotation_number = nSRotation;
                                scanners[k].relative_to_scanner = i;

                                Console.WriteLine("Scanner {0} vs Scanner {1} - R{5}   Coordinate {2}, {3}, {4}",
                                        i, k, crd.x, crd.y, crd.z, nSRotation);
                            }
                        }
                    }
                }

            List<Coordinate> Beacons = new List<Coordinate>();

            //Beacons = MoveAllBeacons3();
            for (int i = scanners.Count - 1; i >=0; i--)
            {
                UpdateScannerCoordinates(i);
            }

            int nResOne = 0;
            int nResTwo = 0;
            foreach (Scanner S in scanners)
            {
                nResOne += S.beacons_Coordinate.Select(c => c).Distinct().Count();
                Console.WriteLine("Scaner {0} = {1}", S.id, S.beacons_Coordinate.Select(c => c).Distinct().Count());
            }

            var w2 = Beacons.GroupBy(x => x).Where(g => g.Count() >= 2).Select(y => y).ToList();


            var w_unique = Beacons.Select(c => c).Distinct().ToList();
            Console.WriteLine("Count W2: {0}", w2.Count());
            Console.WriteLine("Count W_Unique: {0}", w_unique.Count());
            //Console.WriteLine("Answer: {0}", w2.Count()- w3.Count());

            Console.WriteLine();

            Console.WriteLine("Part one: {0, 10:0}", nResOne);
            Console.WriteLine("Part one: {0, 10:0}", nResTwo);
        }
        private static void UpdateScannerCoordinates(int id)
        {
            Scanner target = scanners[id];

            List<int> VisitedNodes = new List<int>();

            Route.Clear();
            FindTheWay(id, 0, VisitedNodes);
            Coordinate new_scanner_crdB = target.relativeCrd;
            foreach (var v in Route)
            {

                new_scanner_crdB = Coordinate.RotateCoordinate(new_scanner_crdB, scanners[v.Item2].rotation_number);
                new_scanner_crdB = scanners[v.Item2].relativeCrd + new_scanner_crdB;

                List<Coordinate> new_beakons_coordinates = new List<Coordinate>();
                foreach (var old_beakon_crd in scanners[v.Item1].beacons_Coordinate)
                {
                    Coordinate new_beakon_crd = Coordinate.RotateCoordinate(old_beakon_crd, scanners[v.Item1].rotation_number);
                    new_beakons_coordinates.Add(scanners[v.Item1].relativeCrd + new_beakon_crd);
                }
                scanners[v.Item1].beacons_Coordinate.Clear();
                scanners[v.Item2].beacons_Coordinate.AddRange(new_beakons_coordinates);
            }
            target.AbsoluteCrd = new_scanner_crdB;
            target.Print();
        }
        private static List<Coordinate> MoveAllBeacons3()
        {

            for (int i = Order.Count - 1; i >= 0; i--)
            {
                List<int> VisitedNodes = new List<int>();
                FindTheWay(i, 0, VisitedNodes);
                foreach (var v in Route)
                {
                    scanners[v.Item1].MoveBeaconsTo(scanners[v.Item2]);
                    scanners[v.Item1].beacons_Coordinate.Clear();
                }
                Route.Clear();
            }

            return scanners[0].beacons_Coordinate;
        }

        private static bool FindTheWay(int nStart, int nEnd, List<int> VisitedNodes)
        {

            if (nStart == nEnd) return true;
            if (VisitedNodes.Contains(nStart)) return false;

            VisitedNodes.Add(nStart);

            foreach (int n in Order[nStart])
            {
                if (!VisitedNodes.Contains(n))
                {
                    bool reached = FindTheWay(n, nEnd, VisitedNodes);

                    if (reached)
                    {
                        Route.Insert(0, (nStart, n));
                        return true;
                    }
                }
            }
            return false;
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

                    if (line == "")
                    {
                        scanners.Add(sc);
                        nScannerNum++;
                        sc = new Scanner(nScannerNum);
                        continue;
                    }

                    string[] parts = line.Split(",");
                    Coordinate crd = new Coordinate(int.Parse(parts[0]), int.Parse(parts[1]), int.Parse(parts[2]));
                    sc.beacons_Coordinate.Add(crd);
                }
            scanners.Add(sc);
        }
    }
}
