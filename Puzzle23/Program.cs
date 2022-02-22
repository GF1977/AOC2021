using System.Diagnostics;

namespace MyApp
{
    public class Room
    {
        private static int IDcounter = 0;
        public int Id { get; }
        public string OpenFor { get; set; } // ABCD
        public List<int> Connections { get; }
        public bool isVisited { get; set; }
        public Dictionary<int, List<int>> Paths = new Dictionary<int, List<int>>();
        public Room()
        {
            Id = IDcounter++;
            isVisited = false;
            Connections = new List<int>();

            if (Id >= 11)
            {
                if (Id % 4 == 3) OpenFor = "A";
                if (Id % 4 == 0) OpenFor = "B";
                if (Id % 4 == 1) OpenFor = "C";
                if (Id % 4 == 2) OpenFor = "D";
            }
            else
                            OpenFor = "ABCD";
        }
        public static void ResetCounter() { IDcounter = 0; }
    }
    public class Shrimp
    {
        private static int IDcounter = 0;
        public string Type { get; set; } // ABCD
        public int Id { get; set; }
        public int MoveCost { get; set; } // 1 - 1000
        public int RoomId { get; set; }
        public bool ImHome { get; set; }

        public Shrimp(string Type)
        {
            this.Id = IDcounter++;
            this.ImHome = false;
            this.Type = Type;
            this.MoveCost = (Type == "A") ? 1 : (Type == "B") ? 10 : (Type == "C") ? 100 : (Type == "D") ? 1000 : -1;
        }
        public object Clone()
        {
            return this.MemberwiseClone();
        }

        public bool IsItRightDestination(List<Shrimp> Shrimps, int RoomsCount, Room Rend)
        {
            if (Rend.OpenFor != Type)
                return false;

            for(int i = Rend.Id + 4; i < RoomsCount; i+=4)
                if (Program.WhoInTheRoom(i, Shrimps) != Type)
                    return false;

            return true;
        }
        public static void ResetCounter() { IDcounter = 0; }
    }

    public class Program
    {
        // Answers for Data_p.txt  Part 1:  11516    Part 2: 40272

        static int nMinimalEnergy = int.MaxValue;
        //static int LastMovedShrimp = -1;
        static Stopwatch stopwatch;

        static List<Room> GlobalMap = new List<Room>();
        static List<Shrimp> GlobalShrimps = new List<Shrimp>();

        public static void Main(string[] args)
        {
            stopwatch = new Stopwatch();
            stopwatch.Start();
            SolveTheProblem(@".\..\..\..\Data_s.txt");
            SolveTheProblem(@".\..\..\..\Data_t.txt");
            SolveTheProblem(@".\..\..\..\Data_pIO.txt");

            SolveTheProblem(@".\..\..\..\Data_s2.txt");
            SolveTheProblem(@".\..\..\..\Data_t2.txt");
            SolveTheProblem(@".\..\..\..\Data_p2.txt");
        }

        private static void SolveTheProblem(string filepath)
        {
            Console.WriteLine("Calculation started: {0}", filepath);
            nMinimalEnergy = int.MaxValue;
            ParsingInputData(filepath);
            startMoving(GlobalShrimps, 0);
            Console.WriteLine("Energy: {0, 9:0}", nMinimalEnergy);
            Console.WriteLine("Total time {0,8:0.000} Seconds", stopwatch.ElapsedMilliseconds / 1000.0);
            Console.WriteLine();
        }

        private static List<int> GetFullPath(Room rStart, Room Rend)
        {
            rStart.isVisited = true;

            if (rStart.Id == Rend.Id)
                return new List<int>();

            foreach (int id in rStart.Connections)
                if (!GlobalMap[id].isVisited)
                {
                    List<int> Res = GetFullPath(GlobalMap[id], Rend);
                    if (Res != null)
                    {
                        Res.Add(id);
                        return Res;
                    }
                }

            return null;
        }

        public static string WhoInTheRoom(int RoomId, List<Shrimp> Shrimps)
        {
            foreach (Shrimp S in Shrimps)
                if (S.RoomId == RoomId)
                    return S.Type;
            
            return "";
        }


        private static int GetRestMinEnergy(List<Shrimp> Shrimps)
        {
            bool[] OccupiedRoom = new bool[27];
            int Res = 0;
            foreach(Shrimp S in Shrimps)
                if(!S.ImHome)
                    foreach (Room REnd in GlobalMap.Where(r=>r.Id!=S.RoomId && r.Id >= 11))
                        if (REnd.OpenFor == S.Type && !OccupiedRoom[REnd.Id])
                        {
                            Res += S.MoveCost * REnd.Paths[S.RoomId].Count();
                            OccupiedRoom[REnd.Id] = true;
                            break;
                        }

            return Res;
        }

        private static int startMoving(List<Shrimp> Shrimps, int recEnergy)
        {
            //LastMovedShrimp = -1;
            if (nMinimalEnergy <= recEnergy + GetRestMinEnergy(Shrimps))
                return 1;

            if (isCorrectOrder(Shrimps))
            {
                if (recEnergy < nMinimalEnergy)
                    nMinimalEnergy = recEnergy;

                //Console.WriteLine("Best Energy: {0,5:0}   - time {1,8:0.000} Seconds", recEnergy, stopwatch.ElapsedMilliseconds / 1000.0);
                return recEnergy;
            }

            foreach (Shrimp S in Shrimps.Where(s=>s.ImHome == false)) // Skip Shrimp who is in the right place
            {
                Room RStart = GlobalMap[S.RoomId];

                foreach (Room REnd in GlobalMap.Where(r=>r.Connections.Count !=3)) // Skip room right above the caves
                {
                    if (S.RoomId == REnd.Id) // Skip where Start == End 
                        continue;

                    if (REnd.Id >= 11 && REnd.OpenFor != S.Type) // Skip rooms which are not for this type (A goes to A, B to B etc)
                        continue;

                    if (WhoInTheRoom(REnd.Id, Shrimps) != "") // Skip where the room is occupied
                        continue;

                    if (RStart.OpenFor == "ABCD" && !S.IsItRightDestination(Shrimps, GlobalMap.Count, REnd)) // skip if a shrimps moves from corridor to not the right place
                        continue;

                    bool PathIsBlocked = false;
                    foreach (int id in RStart.Paths[REnd.Id])
                        if (WhoInTheRoom(id, Shrimps) != "")
                        {
                            PathIsBlocked = true;
                            break;
                        }

                    if (!PathIsBlocked)
                    {
                        List<Shrimp> ShrimpsTmp = new List<Shrimp>();
                        foreach (Shrimp SS in Shrimps)
                            ShrimpsTmp.Add((Shrimp)SS.Clone());

                        Shrimp STmp = ShrimpsTmp[S.Id];

                       // Move Shrimp
                        STmp.RoomId = REnd.Id;

                        if (REnd.OpenFor == STmp.Type)
                            STmp.ImHome = true;

                        int nMoveCost = RStart.Paths[REnd.Id].Count() * S.MoveCost;
                        // Move End

                        if (nMinimalEnergy > recEnergy + nMoveCost +GetRestMinEnergy(ShrimpsTmp))
                        {
                            startMoving(ShrimpsTmp, recEnergy + nMoveCost);
                        }
                    }
                }

            }
            return 1;
        }

        private static bool isCorrectOrder(List<Shrimp> Shrimps)
        {
            foreach (Shrimp S in Shrimps)
                if (S.ImHome == false)
                    return false;

            return true;
        }

        private static void ParsingInputData(string filePath)
        {
            GlobalMap.Clear();
            GlobalShrimps.Clear();
            Room.ResetCounter();
            Shrimp.ResetCounter();
            int[,] map = new int[13, 7];
            int X = 0, Y = 0;
            using (StreamReader file = new(filePath))
                while (!file.EndOfStream)
                {
                    string line = file.ReadLine();
                    foreach (char c in line)
                    {
                        if (c == '.' || c == 'A' || c == 'B' || c == 'C' || c == 'D')
                        {
                            Room R = new Room();
                            map[X, Y] = 1000 + R.Id;

                            if (c != '.')
                            {
                                Shrimp S = new Shrimp(c.ToString());
                                //R.isOccupiedBy = S.Type;
                                S.RoomId = R.Id;
                                GlobalShrimps.Add(S);
                            }

                            GlobalMap.Add(R);
                        }
                        X++;
                    }
                    X = 0;
                    Y++;
                }

            for (int x = 0; x < 13; x++)
                for (int y = 0; y < 7; y++)
                    if (map[x, y] >= 1000)
                    {
                        int RoomID = map[x, y] - 1000;
                        Room R = GlobalMap.Find(r => r.Id == RoomID);

                        if (map[x, y + 1] >= 1000)
                            R.Connections.Add(map[x, y + 1] - 1000);
                        if (map[x, y - 1] >= 1000)
                            R.Connections.Add(map[x, y - 1] - 1000);
                        if (map[x + 1, y] >= 1000)
                            R.Connections.Add(map[x + 1, y] - 1000);
                        if (map[x - 1, y] >= 1000)
                            R.Connections.Add(map[x - 1, y] - 1000);
                    }


            foreach (Shrimp shrimp in GlobalShrimps)
                shrimp.ImHome = shrimp.IsItRightDestination(GlobalShrimps,GlobalMap.Count, GlobalMap[shrimp.RoomId]);

            foreach (Room RStart in GlobalMap)
                foreach (Room REnd in GlobalMap)
                    if (RStart.Id != REnd.Id)
                    {
                        List<int> tmp = GetFullPath(RStart, REnd);
                        tmp.Reverse();
                        RStart.Paths.Add(REnd.Id, tmp);

                        foreach (Room R in GlobalMap) R.isVisited = false;
                    }

        }
    }
}
