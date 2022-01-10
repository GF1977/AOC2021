using static System.Math;

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

        public static List<Coordinates> CompareTwoCoordinates(Coordinates A, Coordinates B, int nScanner1Rotation, int nScanner2Rotation)
        {
            List<Coordinates> result = new List<Coordinates>();
            //for(int i = 0; i < 24; i++)
            {

                Coordinates newA = RotateCoordinate(A, nScanner1Rotation);
                Coordinates newB = RotateCoordinate(B, nScanner2Rotation);
                Coordinates Result = newA + newB;

                result.Add(Result);
            }

            return result;
        }

        public static Coordinates RotateCoordinate(Coordinates crd, int n)
        {
            int x = crd.x;
            int y = crd.y;
            int z = crd.z;

            if (n < 0 && n > 23) throw new ArgumentOutOfRangeException();

            if (n == 0) return new Coordinates(x, y, z);
            if (n == 1) return new Coordinates(y, -x, z);
            if (n == 2) return new Coordinates(-x, -y, z);
            if (n == 3) return new Coordinates(-y, x, z);
            if (n == 4) return new Coordinates(x, y, -z);
            if (n == 5) return new Coordinates(y, -x, -z);
            if (n == 6) return new Coordinates(-x, -y, -z);
            if (n == 7) return new Coordinates(-y, x, -z);

            if (n == 8) return new Coordinates(x, y, z);
            if (n == 9) return new Coordinates(z, y, -x);
            if (n == 10) return new Coordinates(-x, y, -z);
            if (n == 11) return new Coordinates(-z, y, x);
            if (n == 12) return new Coordinates(x, -y, z);
            if (n == 13) return new Coordinates(z, -y, -x);
            if (n == 14) return new Coordinates(-x, -y, -z);
            if (n == 15) return new Coordinates(-z, -y, x);

            if (n == 16) return new Coordinates(x, y, z);
            if (n == 17) return new Coordinates(x, z, -y);
            if (n == 18) return new Coordinates(x, -y, -z);
            if (n == 19) return new Coordinates(x, -z, y);
            if (n == 20) return new Coordinates(-x, y, z);
            if (n == 21) return new Coordinates(-x, z, -y);
            if (n == 22) return new Coordinates(-x, -y, -z);
            if (n == 23) return new Coordinates(-x, -z, y);

            return new Coordinates(0, 0, 0);
        }
        internal void Print()
        {
            Console.WriteLine("Coordinates {0}, {1}, {2}", x, y, z);
        }

        internal bool IsValid()
        {
            if (Abs(x) <= 1000 && Abs(y) <= 1000 && Abs(z) <= 1000)
                return true;
            else
                return false;
        }
        public static Coordinates operator -(Coordinates A, Coordinates B)
        {
            return new Coordinates(A.x - B.x, A.y - B.y, A.z - B.z);
        }

        public static Coordinates operator +(Coordinates A, Coordinates B)
        {
            return new Coordinates(A.x + B.x, A.y + B.y, A.z + B.z);
        }

        public static Coordinates operator *(Coordinates A, Coordinates B)
        {
            return new Coordinates(A.x * B.x, A.y * B.y, A.z * B.z);
        }

        public static bool operator ==(Coordinates A, Coordinates B)
        {
            if (A.x == B.x && A.y == B.y && A.z == B.z)
                return true;
            else
                return false;
        }
        public static bool operator !=(Coordinates A, Coordinates B)
        {
            return !(A == B);
        }
    }
    public class Scanner
    {
        public int id { get; }
        public Coordinates scanner_coordinates = new Coordinates(0, 0, 0);
        public int relative_to_scanner = -1;
        public int rotation_number = -1;
        public int relative_scanner_rotation_number = -1;

        public List<Coordinates> beacons_coordinates = new List<Coordinates>();

        public Scanner(int id)
        {
            this.id = id;
        }

        public List<Coordinates> CompareCoordinates(Scanner Target, int nScanner1Rotation, int nScanner2Rotation)
        {
            List<Coordinates> LDelta = new List<Coordinates>();

            foreach (Coordinates crdScannerA in this.beacons_coordinates)
                foreach (Coordinates crdScannerB in Target.beacons_coordinates)
                {
                    List<Coordinates> LDelta2 = Coordinates.CompareTwoCoordinates(crdScannerA, crdScannerB, nScanner1Rotation, nScanner2Rotation);
                    LDelta.AddRange(LDelta2);
                }

            return LDelta;
        }

        internal void Print()
        {
            Console.WriteLine("Scanner {0} relative to {1}:  Coordinates {2}, {3}, {4}", id, relative_to_scanner, scanner_coordinates.x, scanner_coordinates.y, scanner_coordinates.z);
        }

        internal List<Coordinates> MoveBeaconsTo(Scanner target)
        {
            List<Coordinates> Beacons = new List<Coordinates>();

            foreach (Coordinates b_target_crd in target.beacons_coordinates)
            {
                Coordinates new_b_crd = Coordinates.RotateCoordinate(b_target_crd, this.rotation_number);// this.relative_scanner_rotation_number);
                Beacons.Add(new_b_crd);
            }

            foreach (Coordinates b_this_crd in this.beacons_coordinates)
            {
                Coordinates new_b_crd = this.scanner_coordinates - Coordinates.RotateCoordinate(b_this_crd, this.relative_scanner_rotation_number) ;// this.rotation_number) + this.scanner_coordinates;
                Beacons.Add(new_b_crd);
            }


            target.beacons_coordinates.Clear();
            foreach(Coordinates crd in Beacons)
                target.beacons_coordinates.Add(crd);

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
            List<Coordinates> LDelta = new List<Coordinates>();
            
                for (int i = 0; i < scanners.Count; i++)
                for (int nScanner1Rotation = 0; nScanner1Rotation < 24; nScanner1Rotation++)
                    for (int k = i + 1; k < scanners.Count; k++)
                    {
                        //int i = 1;
                        //if (k == i) continue;
                        //Console.WriteLine("i: {0},   k: {1}", i, k);
                        //LDelta.AddRange(scanners[i].CompareCoordinates(scanners[k]));
                        //if(scanners[k].relative_to_scanner == -1 || scanners[i].scanner_coordinates == new Coordinates(0,0,0))
                        for (int nScanner2Rotation = 0; nScanner2Rotation < 24; nScanner2Rotation++)
                        {
                            LDelta.Clear();
                            LDelta.AddRange(scanners[i].CompareCoordinates(scanners[k], nScanner1Rotation, nScanner2Rotation));
                            var query = LDelta.GroupBy(x => x).Where(g => g.Count() >= 12).Select(y => y).ToList();
                            if (query.Count() > 0)
                            {

                                    Coordinates crd = query[0].Key;
                                    if (scanners[k].relative_to_scanner == -1)
                                    {
                                        Coordinates normalCrd = new Coordinates(1, 1, 1);
                                        normalCrd = Coordinates.RotateCoordinate(normalCrd, nScanner1Rotation);
                                    if (scanners[k].relative_to_scanner > 0)
                                        normalCrd = Coordinates.RotateCoordinate(normalCrd, scanners[k].relative_to_scanner);
                                    crd = crd * normalCrd;
                                    scanners[k].scanner_coordinates = crd;
                                        scanners[k].relative_to_scanner = i;
                                        scanners[k].rotation_number = nScanner1Rotation;
                                        scanners[k].relative_scanner_rotation_number = nScanner2Rotation;


                                    Console.WriteLine("Scanner {0} - R{5}     vs Scanner {1} - R{6}   Coordinates {2}, {3}, {4}",
                                            i, k, crd.x, crd.y, crd.z, nScanner1Rotation, nScanner2Rotation);
                                        break;
                                    }

                                if (scanners[i].relative_to_scanner == -1 && i!=0)
                                {
                                    Coordinates normalCrd = new Coordinates(1, 1, 1);
                                    normalCrd = Coordinates.RotateCoordinate(normalCrd, nScanner2Rotation);
                                    if (scanners[i].relative_to_scanner > 0)
                                        normalCrd = Coordinates.RotateCoordinate(normalCrd, scanners[i].relative_to_scanner);
                                    crd = crd * normalCrd;
                                    scanners[i].scanner_coordinates = crd;
                                    scanners[i].relative_to_scanner = k;
                                    scanners[i].rotation_number = nScanner1Rotation;
                                    scanners[i].relative_scanner_rotation_number = nScanner2Rotation;


                                    Console.WriteLine("Scanner {0} - R{5}   vs Scanner {1} - R{6} :  Coordinates {2}, {3}, {4}",
                                        i, k, crd.x, crd.y, crd.z, nScanner1Rotation, nScanner2Rotation);
                                    break;
                                }


                            }

                        }
                    }

            List<Coordinates> Beacons = new List<Coordinates>();
            //Beacons.AddRange(scanners[0].beacons_coordinates);
            Beacons.AddRange(scanners[2].MoveBeaconsTo(scanners[4]));
            Beacons.AddRange(scanners[4].MoveBeaconsTo(scanners[1]));
            Beacons.AddRange(scanners[3].MoveBeaconsTo(scanners[1]));
            Beacons.AddRange(scanners[1].MoveBeaconsTo(scanners[0]));

            var w3 = Beacons.GroupBy(x => x).Where(g => g.Count() >= 2).Select(y => y).ToList();
            //var w3 = Beacons.Select(c => c).Distinct();
            Console.WriteLine("Count: {0}", w3.Count());
            //foreach (Coordinates crd in w3)
            //{
            //    crd.Print();
            //}


            Console.WriteLine();
            //scanners[0].scanner_coordinates = new Coordinates(0, 0, 0);
            NormalizeScannerCoordinates();

            //foreach (Coordinates crd in LDelta)
            //  Console.WriteLine("x={0}  y={1}  z={2}", crd.x, crd.y, crd.z);

            Console.WriteLine("Part one: {0, 10:0}", 1);
            Console.WriteLine("Part one: {0, 10:0}", 2);
        }
        private static void NormalizeScannerCoordinates()
        {
            foreach (Scanner S in scanners)
            {
                if (S.id > 0 && S.relative_to_scanner > 0)
                {
                    //Coordinates normalCrd = new Coordinates(1, 1, 1);
                    //normalCrd = Coordinates.RotateCoordinate(normalCrd, scanners[S.relative_to_scanner].rotation_number);

                    Coordinates crd = S.scanner_coordinates;
                    //crd = crd * normalCrd;

                    //crd = Coordinates.RotateCoordinate(crd, scanners[S.relative_to_scanner].relative_scanner_rotation_number);
                    //crd = Coordinates.RotateCoordinate(crd, S.relative_scanner_rotation_number);
                    //crd = Coordinates.RotateCoordinate(crd, S.rotation_number);
                    Coordinates one = new Coordinates(1, 1, 1);

                    Coordinates crd1 = Coordinates.RotateCoordinate(one, scanners[S.relative_to_scanner].relative_scanner_rotation_number);
                    Coordinates crd2 = Coordinates.RotateCoordinate(one, S.relative_scanner_rotation_number);
                    Coordinates crd3 = Coordinates.RotateCoordinate(one, S.rotation_number);

                    crd = crd * crd1 * crd2 * crd3;

                    crd = crd + scanners[S.relative_to_scanner].scanner_coordinates;
                    S.scanner_coordinates = crd;
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
                    Coordinates crd = new Coordinates(int.Parse(parts[0]), int.Parse(parts[1]), int.Parse(parts[2]));
                    sc.beacons_coordinates.Add(crd);
                }
            scanners.Add(sc);
        }
    }
}
