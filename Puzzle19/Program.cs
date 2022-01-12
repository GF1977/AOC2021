using static System.Math;

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

        public static List<Coordinate> CompareTwoCoordinate(Coordinate A, Coordinate B, int nScanner1Rotation, int nScanner2Rotation)
        {
            List<Coordinate> result = new List<Coordinate>();
            //for(int i = 0; i < 24; i++)
            {

                Coordinate newA = RotateCoordinate(A, nScanner1Rotation);
                Coordinate newB = RotateCoordinate(B, nScanner2Rotation);
                Coordinate Result = newA - newB;

                result.Add(Result);
            }

            return result;
        }
        public static Coordinate RotateCoordinateOpposite(Coordinate crd, int n)
        {
            if (n == 1) return RotateCoordinate(crd, 3);
            if (n == 5) return RotateCoordinate(crd, 7);
            if (n == 9) return RotateCoordinate(crd, 11);
            if (n == 13) return RotateCoordinate(crd, 15);
            if (n == 17) return RotateCoordinate(crd, 19);
            if (n == 21) return RotateCoordinate(crd, 23);

            if (n == 3) return RotateCoordinate(crd, 1);
            if (n == 7) return RotateCoordinate(crd, 5);
            if (n == 11) return RotateCoordinate(crd, 9);
            if (n == 15) return RotateCoordinate(crd, 13);
            if (n == 19) return RotateCoordinate(crd, 17);
            if (n == 23) return RotateCoordinate(crd, 21);

            return RotateCoordinate(crd, n);
        }
        public static Coordinate RotateCoordinate(Coordinate crd, int n)
        {
            int x = crd.x;
            int y = crd.y;
            int z = crd.z;

            if (n < 0 && n > 23) throw new ArgumentOutOfRangeException();

            if (n ==  0) return new Coordinate( x,  y,  z);
            if (n ==  1) return new Coordinate( y, -x,  z);
            if (n ==  2) return new Coordinate(-x, -y,  z);
            if (n ==  3) return new Coordinate(-y,  x,  z);
            if (n ==  4) return new Coordinate( x,  y, -z);
            if (n ==  5) return new Coordinate( y, -x, -z);
            if (n ==  6) return new Coordinate(-y, -x, -z);
            if (n ==  7) return new Coordinate(-y,  x, -z);

            if (n ==  8) return new Coordinate( z,  y,  x);
            if (n ==  9) return new Coordinate( z,  y, -x);
            if (n == 10) return new Coordinate(-x,  y, -z);
            if (n == 11) return new Coordinate(-z,  y,  x);
            if (n == 12) return new Coordinate( x, -y,  z);
            if (n == 13) return new Coordinate( z, -y, -x);
            if (n == 14) return new Coordinate(-x, -y, -z);
            if (n == 15) return new Coordinate(-z, -y,  x);

            if (n == 16) return new Coordinate( x,  z,  y);
            if (n == 17) return new Coordinate( x,  z, -y);
            if (n == 18) return new Coordinate( x, -y, -z);
            if (n == 19) return new Coordinate( x, -z,  y);
            if (n == 20) return new Coordinate(-x,  y,  z);
            if (n == 21) return new Coordinate(-x,  z, -y);
            if (n == 22) return new Coordinate(-x, -z, -y);
            if (n == 23) return new Coordinate(-x, -z,  y);

            return new Coordinate(0, 0, 0);
        }
        internal void Print()
        {
            Console.WriteLine("Coordinate {0}, {1}, {2}", x, y, z);
        }

        internal bool IsValid()
        {
            if (Abs(x) <= 1000 && Abs(y) <= 1000 && Abs(z) <= 1000)
                return true;
            else
                return false;
        }
        public static Coordinate operator -(Coordinate A, Coordinate B)
        {
            return new Coordinate(A.x - B.x, A.y - B.y, A.z - B.z);
        }

        public static Coordinate operator +(Coordinate A, Coordinate B)
        {
            return new Coordinate(A.x + B.x, A.y + B.y, A.z + B.z);
        }

        public static Coordinate operator *(Coordinate A, Coordinate B)
        {
            return new Coordinate(A.x * B.x, A.y * B.y, A.z * B.z);
        }

        public static bool operator ==(Coordinate A, Coordinate B)
        {
            if (A.x == B.x && A.y == B.y && A.z == B.z)
                return true;
            else
                return false;
        }
        public static bool operator !=(Coordinate A, Coordinate B)
        {
            return !(A == B);
        }
    }
    public class Scanner
    {
        public int id { get; }
        public Coordinate scanner_Coordinate = new Coordinate(0, 0, 0);
        public int relative_to_scanner = -1;
        public int rotation_number = -1;
        public int relative_scanner_rotation_number = -1;

        public List<Coordinate> beacons_Coordinate = new List<Coordinate>();

        public Scanner(int id)
        {
            this.id = id;
        }

        public List<Coordinate> CompareCoordinate(Scanner Target, int nScanner1Rotation, int nScanner2Rotation)
        {
            List<Coordinate> LDelta = new List<Coordinate>();

            foreach (Coordinate crdScannerA in this.beacons_Coordinate)
                foreach (Coordinate crdScannerB in Target.beacons_Coordinate)
                {
                    List<Coordinate> LDelta2 = Coordinate.CompareTwoCoordinate(crdScannerA, crdScannerB, nScanner1Rotation, nScanner2Rotation);
                    LDelta.AddRange(LDelta2);
                }

            return LDelta;
        }

        internal void Print()
        {
            Console.WriteLine("Scanner {0} relative to {1}:  Coordinate {2}, {3}, {4}", id, relative_to_scanner, scanner_Coordinate.x, scanner_Coordinate.y, scanner_Coordinate.z);
        }

        internal List<Coordinate> MoveBeaconsTo(Scanner target)
        {
            List<Coordinate> Beacons = new List<Coordinate>();

            foreach (Coordinate b_target_crd in target.beacons_Coordinate)
            {
                Coordinate new_b_crd = Coordinate.RotateCoordinate(b_target_crd, this.relative_scanner_rotation_number);
                Beacons.Add(new_b_crd);
            }

            foreach (Coordinate b_this_crd in this.beacons_Coordinate)
            {
                Coordinate new_b_crd = this.scanner_Coordinate + Coordinate.RotateCoordinate(b_this_crd, this.rotation_number) ;// this.rotation_number) + this.scanner_Coordinate;
                Beacons.Add(new_b_crd);
            }

            var w3 = Beacons.GroupBy(x => x).Where(g => g.Count() >= 2).Select(y => y).ToList();
            var w2 = Beacons.Select(c => c).Distinct().ToList();

            target.beacons_Coordinate.Clear();
            foreach (Coordinate crd in Beacons)
            {
                Coordinate crd_new = Coordinate.RotateCoordinateOpposite(crd, this.relative_scanner_rotation_number);
                target.beacons_Coordinate.Add(crd_new);
            }

            return Beacons;
        }
    }
    public class Program
    {
        // Answers for Data_p.txt  Part 1:      Part 2: 
        static string filePath = @".\..\..\..\Data_t.txt";
        static List<Scanner> scanners = new List<Scanner>();
        static Dictionary<int, List<int>> Order = new Dictionary<int, List<int>>();
        public static void Main(string[] args)
        {
            ParsingInputData();
            List<Coordinate> LDelta = new List<Coordinate>();

            //Coordinate CCC = new Coordinate(1, 2, 3);
            //Coordinate NNN = new Coordinate(0, 0, 0);
            //for (int x = 0; x < 24; x++)
            //    for (int y = 0; y < 24; y++)
            //    {
            //        NNN = Coordinate.RotateCoordinate(CCC, x);
            //        NNN = Coordinate.RotateCoordinate(NNN, y);
            //        if (CCC == NNN && x!=y)
            //        {
            //            Console.WriteLine("Rotation {0}  opposite {1}", x, y);
            //        }
            //    }

            
            for (int a=0; a < scanners.Count; a++)
            {
                List<int> relative_scanners = new List<int>();
                Order.Add(a, relative_scanners);
            }

            
                for (int i = 0; i < scanners.Count; i++)
                for (int nScanner1Rotation = 0; nScanner1Rotation < 24; nScanner1Rotation++)
                    for (int k = 0; k < scanners.Count; k++)
                    {

                        //int i = 1;
                        if (k == i) continue;
                        //Console.WriteLine("i: {0},   k: {1}", i, k);
                        //LDelta.AddRange(scanners[i].CompareCoordinate(scanners[k]));
                        //if(scanners[k].relative_to_scanner == -1 || scanners[i].scanner_Coordinate == new Coordinate(0,0,0))
                        for (int nScanner2Rotation = 0; nScanner2Rotation < 24; nScanner2Rotation++)
                        {
                            LDelta.Clear();
                            LDelta.AddRange(scanners[i].CompareCoordinate(scanners[k], nScanner1Rotation, nScanner2Rotation));
                            var query = LDelta.GroupBy(x => x).Where(g => g.Count() >= 12).Select(y => y).ToList();
                            if (query.Count() > 0)
                            {
                                if(Order.ContainsKey(k))
                                {
                                    if(!Order[k].Contains(i))
                                        Order[k].Add(i);
                                }
    
                                //if (Order.ContainsKey(k))
                                //{
                                //    if (!Order[k].Contains(i))
                                //        Order[k].Add(i);
                                //}





                                Coordinate crd = query[0].Key;
                                    if (scanners[k].relative_to_scanner == -1)
                                    {
                                        Coordinate normalCrd = new Coordinate(-1, -1, -1);
                                    //    normalCrd = Coordinate.RotateCoordinate(normalCrd, nScanner1Rotation);
                                    //if (scanners[k].relative_to_scanner > 0)
                                    //    normalCrd = Coordinate.RotateCoordinate(normalCrd, scanners[k].relative_to_scanner);
                                    //crd = crd * normalCrd;
                                    scanners[k].scanner_Coordinate = crd;
                                        scanners[k].relative_to_scanner = i;
                                        scanners[k].rotation_number = nScanner2Rotation;
                                        scanners[k].relative_scanner_rotation_number = nScanner1Rotation;


                                    Console.WriteLine("Scanner {0} - R{5}     vs Scanner {1} - R{6}   Coordinate {2}, {3}, {4}",
                                            i, k, crd.x, crd.y, crd.z, nScanner1Rotation, nScanner2Rotation);
                                        break;
                                    }

                                if (scanners[i].relative_to_scanner == -1 && i!=0)
                                {
                                    Coordinate normalCrd = new Coordinate(-1, -1, -1);
                                    //normalCrd = Coordinate.RotateCoordinate(normalCrd, nScanner2Rotation);
                                    //if (scanners[i].relative_to_scanner > 0)
                                    //    normalCrd = Coordinate.RotateCoordinate(normalCrd, scanners[i].relative_to_scanner);
                                    crd = crd * normalCrd;
                                    scanners[i].scanner_Coordinate = crd;
                                    scanners[i].relative_to_scanner = k;
                                    scanners[i].rotation_number = nScanner1Rotation;
                                    scanners[i].relative_scanner_rotation_number = nScanner2Rotation;


                                    Console.WriteLine("Scanner {0} - R{5}   vs Scanner {1} - R{6} :  Coordinate {2}, {3}, {4}",
                                        i, k, crd.x, crd.y, crd.z, nScanner1Rotation, nScanner2Rotation);
                                    break;
                                }


                            }

                        }
                    }

            List<Coordinate> Beacons = new List<Coordinate>();
            // FOR TEST CASE
            //scanners[3].MoveBeaconsTo(scanners[1]);
            //scanners[2].MoveBeaconsTo(scanners[4]);
            //scanners[4].MoveBeaconsTo(scanners[1]);
            //Beacons.AddRange(scanners[1].MoveBeaconsTo(scanners[0]));
            // END TEST CASE
            Beacons = MoveAllBeacons();
            //Beacons = MoveAllBeacons2();
            Beacons = MoveAllBeacons();
            Beacons = MoveAllBeacons();
            Beacons = MoveAllBeacons();
            Beacons = MoveAllBeacons();
            
            Beacons = MoveAllBeacons2();

            int nResOne = 0;// Beacons.Select(c => c).Distinct().Count();
            int nResTwo = 0;
            foreach (Scanner S in scanners)
            {
                nResOne += S.beacons_Coordinate.Select(c => c).Distinct().Count();
                Console.WriteLine("Scaner {0} = {1}", S.id, S.beacons_Coordinate.Select(c => c).Distinct().Count());
            }

            var w3 = Beacons.GroupBy(x => x).Where(g => g.Count() >= 2).Select(y => y).ToList();
            var w2 = Beacons.Select(c => c).Distinct().ToList();
            //Console.WriteLine("Count W3: {0}", w3.Count());

            //nResOne = w2.Count(); 
            //filePath = @".\..\..\..\Data_t_res.txt";
            //scanners.Clear();
            //ParsingInputData();

            //w2.AddRange(scanners[0].beacons_Coordinate);
            //w3 = w2.GroupBy(x => x).Where(g => g.Count() >= 2).Select(y => y).ToList();
            //w2 = w2.Select(c => c).Distinct().ToList();


           // Console.WriteLine("Count W2: {0}", w2.Count());

            Console.WriteLine();

            Console.WriteLine("Part one: {0, 10:0}", nResOne);
            Console.WriteLine("Part one: {0, 10:0}", nResTwo);
        }

        private static List<Coordinate> MoveAllBeacons()
        {
            for(int i = scanners.Count - 1; i > 0; i--)
            {
                int n = scanners[i].relative_to_scanner;
                scanners[i].MoveBeaconsTo(scanners[n]);
                scanners[i].beacons_Coordinate.Clear();

            }

            return scanners[0].beacons_Coordinate;
        }


        private static List<Coordinate> MoveAllBeacons2()
        {

            for (int i = Order.Count - 1; i >= 0; i--)
            {
                foreach(int related_scanners in Order[i])
                {
                    
                    //int n = scanners[related_scanners].relative_to_scanner;
                    scanners[i].MoveBeaconsTo(scanners[related_scanners]);
                }
                if(i!=0)
                    scanners[i].beacons_Coordinate.Clear();

            }

            return scanners[0].beacons_Coordinate;
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
