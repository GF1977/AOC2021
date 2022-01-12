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

        public static Coordinate RotateCoordinate(Coordinate crd, int n)
        {
            int x = crd.x;
            int y = crd.y;
            int z = crd.z;

            if (n < 0 && n > 23) throw new ArgumentOutOfRangeException();

            if (n == 0) return new Coordinate(x, y, z);
            if (n == 1) return new Coordinate(y, -x, z);
            if (n == 2) return new Coordinate(-x, -y, z);
            if (n == 3) return new Coordinate(-y, x, z);
            if (n == 4) return new Coordinate(x, y, -z);
            if (n == 5) return new Coordinate(y, -x, -z);
            if (n == 6) return new Coordinate(-x, -y, -z);
            if (n == 7) return new Coordinate(-y, x, -z);

            if (n == 8) return new Coordinate(x, y, z);
            if (n == 9) return new Coordinate(z, y, -x);
            if (n == 10) return new Coordinate(-x, y, -z);
            if (n == 11) return new Coordinate(-z, y, x);
            if (n == 12) return new Coordinate(x, -y, z);
            if (n == 13) return new Coordinate(z, -y, -x);
            if (n == 14) return new Coordinate(-x, -y, -z);
            if (n == 15) return new Coordinate(-z, -y, x);

            if (n == 16) return new Coordinate(x, y, z);
            if (n == 17) return new Coordinate(x, z, -y);
            if (n == 18) return new Coordinate(x, -y, -z);
            if (n == 19) return new Coordinate(x, -z, y);
            if (n == 20) return new Coordinate(-x, y, z);
            if (n == 21) return new Coordinate(-x, z, -y);
            if (n == 22) return new Coordinate(-x, -y, -z);
            if (n == 23) return new Coordinate(-x, -z, y);

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

        internal List<Coordinate> MoveBeaconsTo(Scanner target, int nRotation = 0)
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
                Coordinate crd_new = Coordinate.RotateCoordinate(crd, nRotation);// this.relative_scanner_rotation_number);
                target.beacons_Coordinate.Add(crd_new);
            }

            return Beacons;
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
            List<Coordinate> LDelta = new List<Coordinate>();
            
                for (int i = 0; i < scanners.Count; i++)
                for (int nScanner1Rotation = 0; nScanner1Rotation < 24; nScanner1Rotation++)
                    for (int k = i + 1; k < scanners.Count; k++)
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
            scanners[2].MoveBeaconsTo(scanners[4],18);
            scanners[4].MoveBeaconsTo(scanners[1],1);
            scanners[3].MoveBeaconsTo(scanners[1],0);
            
            Beacons.AddRange(scanners[1].MoveBeaconsTo(scanners[0],0));

            var w3 = Beacons.GroupBy(x => x).Where(g => g.Count() >= 2).Select(y => y).ToList();
            var w2 = Beacons.Select(c => c).Distinct().ToList();
            Console.WriteLine("Count W3: {0}", w3.Count());
            Console.WriteLine("Count W2: {0}", w2.Count());
            //foreach (Coordinate crd in w3)
            //{
            //    crd.Print();
            //}


            Console.WriteLine();
            //scanners[0].scanner_Coordinate = new Coordinate(0, 0, 0);
            NormalizeScannerCoordinate();

            //foreach (Coordinate crd in LDelta)
            //  Console.WriteLine("x={0}  y={1}  z={2}", crd.x, crd.y, crd.z);

            Console.WriteLine("Part one: {0, 10:0}", 1);
            Console.WriteLine("Part one: {0, 10:0}", 2);
        }
        private static void NormalizeScannerCoordinate()
        {
            foreach (Scanner S in scanners)
            {
                if (S.id > 0 && S.relative_to_scanner > 0)
                {
                    //Coordinate normalCrd = new Coordinate(1, 1, 1);
                    //normalCrd = Coordinate.RotateCoordinate(normalCrd, scanners[S.relative_to_scanner].rotation_number);

                    Coordinate crd = S.scanner_Coordinate;
                    //crd = crd * normalCrd;

                    //crd = Coordinate.RotateCoordinate(crd, scanners[S.relative_to_scanner].relative_scanner_rotation_number);
                    //crd = Coordinate.RotateCoordinate(crd, S.relative_scanner_rotation_number);
                    //crd = Coordinate.RotateCoordinate(crd, S.rotation_number);
                    Coordinate one = new Coordinate(1, 1, 1);

                    Coordinate crd1 = Coordinate.RotateCoordinate(one, scanners[S.relative_to_scanner].relative_scanner_rotation_number);
                    Coordinate crd2 = Coordinate.RotateCoordinate(one, S.relative_scanner_rotation_number);
                    Coordinate crd3 = Coordinate.RotateCoordinate(one, S.rotation_number);

                    crd = crd * crd1 * crd2 * crd3;

                    crd = crd + scanners[S.relative_to_scanner].scanner_Coordinate;
                    S.scanner_Coordinate = crd;
                    S.relative_to_scanner = scanners[S.relative_to_scanner].relative_to_scanner;
                }
                S.Print();
            }

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
