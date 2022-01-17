﻿namespace MyApp
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

        public static Coordinate CompareTwoCoordinate(Coordinate A, Coordinate B, int nSRotation)
        {

            return (A - RotateCoordinate(B, nSRotation));

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
    }
    public class Scanner
    {
        public List<Coordinate> beacons_Coordinate = new List<Coordinate>();

        public bool CompareCoordinate(Scanner Target, int nSRotation, out Coordinate key)
        {
            key = new Coordinate();
            List<Coordinate> LDelta = new List<Coordinate>();

            foreach (Coordinate beaconA in this.beacons_Coordinate)
                foreach (Coordinate beaconB in Target.beacons_Coordinate)
                    LDelta.Add(Coordinate.CompareTwoCoordinate(beaconA, beaconB, nSRotation));

            var query = LDelta.GroupBy(x => x).Where(g => g.Count() >= 12).Select(y => y).ToList();

            if (query.Count() > 0)
            {
                key = query[0].Key;
                return true;
            }

            return false;
        }

    }
    public class Program
    {
        // Answers for Data_p.txt               Part 1: 313     Part 2: 
        // Answers for Data_p_alternative.txt   Part 1: 372     Part 2: 

        //static string filePath = @".\..\..\..\Data_p_alternative.txt";
        static string filePath = @".\..\..\..\Data_p.txt";
        //static string filePath = @".\..\..\..\Data_t.txt";
        static List<Scanner> scanners = new List<Scanner>();

        public static void Main(string[] args)
        {
            ParsingInputData();

            while (scanners.Count != 1)
            {
                for (int k = 1; k < scanners.Count; k++)
                    for (int nSRotation = 0; nSRotation < 24; nSRotation++)
                    {
                        bool res = scanners[0].CompareCoordinate(scanners[k], nSRotation, out Coordinate key);
                        if (res)
                        {
                            foreach (Coordinate c in scanners[k].beacons_Coordinate)
                            {
                                Coordinate newC = Coordinate.RotateCoordinate(c, nSRotation) + key;
                                scanners[0].beacons_Coordinate.Add(newC);
                            }
                            scanners.RemoveAt(k);
                            break;
                        }
                    }
            }

            int nResOne = scanners[0].beacons_Coordinate.Select(c => c).Distinct().ToList().Count();
            int nResTwo = 0;

            Console.WriteLine("Part one: {0, 10:0}", nResOne);
            Console.WriteLine("Part one: {0, 10:0}", nResTwo);
        }
        private static void ParsingInputData()
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
                    Coordinate crd = new Coordinate(int.Parse(parts[0]), int.Parse(parts[1]), int.Parse(parts[2]));
                    sc.beacons_Coordinate.Add(crd);
                }
            scanners.Add(sc);
        }
    }
}
