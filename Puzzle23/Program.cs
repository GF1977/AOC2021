using System.Diagnostics;

namespace MyApp
{
    public class Room
    {
        private static int IDcounter = 0;
        public int Id { get; }
        public string OpenFor { get; set; } // ABCD
        public string isOccupiedBy { get; set; }
        public List<int> Connections { get; }
        public bool isVisited { get; set; }

        public Dictionary<int, List<int>> Paths = new Dictionary<int, List<int>>();
        public Room()
        {
            Id = IDcounter++;
            isOccupiedBy = "";
            OpenFor = "ABCD";
            isVisited = false;

            Connections = new List<int>();

            if (Id >= 11)
            {
                if (Id % 4 == 3) OpenFor = "A";
                if (Id % 4 == 0) OpenFor = "B";
                if (Id % 4 == 1) OpenFor = "C";
                if (Id % 4 == 2) OpenFor = "D";
            }

        }
        public object Clone()
        {
            return this.MemberwiseClone();
        }

        public int GetSteps(Room REnd)
        {
            return this.Paths[REnd.Id].Count();
        }

    }

    public class Shrimp
    {
        private static int IDcounter = 0;
        public string Type { get; set; } // ABCD
        public int Id { get; set; }
        public int MoveCount { get; set; }
        public int MoveCost { get; set; } // 1 - 1000
        public int RoomId { get; set; }
        public bool ImHome { get; set; }

        public Shrimp(string Type)
        {
            this.Id = IDcounter++;
            this.ImHome = false;
            this.Type = Type;
            this.MoveCount = 0;
            this.RoomId = RoomId;

            this.MoveCost = (Type == "A") ? 1 : (Type == "B") ? 10 : (Type == "C") ? 100 : (Type == "D") ? 1000 : -1;
        }
        public object Clone()
        {
            return this.MemberwiseClone();
        }

        public bool IsItHome(List<Room> Rooms, Room Rend)
        {
            //if (Rend.OpenFor != Type)
            //    return false;

            //Room RMax = Rooms.Find(r => r.Id == Rend.Connections.Max());

            //if (Type == Rend.OpenFor && (Rend.Connections.Count == 1 || RMax.isOccupiedBy == Type))
            //    return true;

            //return false;


            //if (Rend.isOccupiedBy != "" || Rend.Id < 11 || Rend.OpenFor != Type)
            //    return false;
            if (Rend.OpenFor != Type)
                return false;


            int i = Rend.Id + 4;
            while (i < Rooms.Count)
            {
                if (Rooms[i].isOccupiedBy != Type)
                    return false;

                i += 4;
            }


            return true;
        }
    }

    public class Program
    {
        // Answers for Data_p.txt  Part 1:  11516    Part 2: 
        static readonly string filePath = @".\..\..\..\Data_pIO.txt";

        static int nStepScoreBest = int.MaxValue;
        static int LastMovedShrimp = 0;
        static long Iterations = 0;
        static Stopwatch stopwatch;

        static List<Room> GlobalMap = new List<Room>();
        static List<Shrimp> GlobalShrimps = new List<Shrimp>();

        public static void Main(string[] args)
        {
            ParsingInputData();

            List<Room> Rooms = GlobalMap;

            List<Shrimp> Shrimps = GlobalShrimps;

            int i = 0;
            foreach (Shrimp shrimp in Shrimps)
            {
                shrimp.Id = i++;
                shrimp.ImHome = shrimp.IsItHome(Rooms, Rooms[shrimp.RoomId]);
            }


            foreach (Room RStart in Rooms)
                foreach (Room REnd in Rooms)
                    if (RStart.Id != REnd.Id)
                    {
                        List<int> tmp = GetFullPath(RStart, REnd, Rooms);
                        tmp.Reverse();
                        RStart.Paths.Add(REnd.Id, tmp);

                        foreach (Room R in Rooms) R.isVisited = false;
                    }


            int a = GetRestMinScore(Shrimps, Rooms);


            stopwatch = new Stopwatch();
            stopwatch.Start();


            startMoving(Shrimps, Rooms, 0);


            Console.WriteLine("Total time {0,8:0.000} Seconds", stopwatch.ElapsedMilliseconds / 1000.0);

            Console.WriteLine("Part one: {0, 10:0}", Iterations);
            Console.WriteLine("Part two: {0, 10:0}", nStepScoreBest);
        }

        private static List<int> GetFullPath(Room rStart, Room Rend, List<Room> Rooms)
        {
            rStart.isVisited = true;

            if (rStart.Id == Rend.Id)
                return new List<int>();

            foreach (int id in rStart.Connections)
                if (!Rooms[id].isVisited)
                {
                    List<int> Res = GetFullPath(Rooms[id], Rend, Rooms);
                    if (Res != null)
                    {
                        Res.Add(id);
                        return Res;
                    }
                }

            return null;
        }

        public static bool isMovable(Shrimp S, int toRoom, List<Room> Rooms)
        {
            if (S.ImHome) return false;
            if (S.Id == LastMovedShrimp) return false;

            if (S.RoomId == toRoom || Rooms[toRoom].isOccupiedBy != "")
                return false;

            if (Rooms[S.RoomId].OpenFor == "ABCD" && !S.IsItHome(Rooms, Rooms[toRoom]))
                return false;

            if (Rooms[toRoom].Connections.Count == 3)
                return false;

            if (Rooms[toRoom].OpenFor.Contains(S.Type) == false)
                return false;

            foreach (int id in Rooms[S.RoomId].Paths[toRoom])
                if (Rooms[id].isOccupiedBy != "") return false;

            return true;
        }

        internal static void MoveTo(Shrimp S, int toRoom, List<Shrimp> Shrimps, List<Room> Rooms)
        {
            LastMovedShrimp = S.Id;

            Room Rstart = Rooms[S.RoomId];
            Room Rend = Rooms[toRoom];

            //string Res = S.Type.ToString() + S.Id.ToString() + " moves from " + Rstart.Id + " to " + Rend.Id;

            Rstart.isOccupiedBy = "";
            Rend.isOccupiedBy = S.Type;

            if (Rend.OpenFor == S.Type)
                S.ImHome = true;

            S.RoomId = Rend.Id;

            //return Res;
        }


        private static int GetRestMinScore(List<Shrimp> Shrimps, List<Room> Rooms)
        {
            List<int> OccupiedRoom = new List<int>();
            int Res = 0;
            foreach(Shrimp S in Shrimps)
            {
                if(!S.ImHome)
                {
                    foreach (Room R in Rooms.Where(r=>r.Id!=S.RoomId && r.Id >= 11))
                        if (R.OpenFor == S.Type && !OccupiedRoom.Contains(R.Id))
                        {
                            Res += S.MoveCost * R.Paths[S.RoomId].Count();
                            OccupiedRoom.Add(R.Id);
                            break;
                        }

                }
            }

            return Res;
        }

        private static int startMoving(List<Shrimp> Shrimps, List<Room> Rooms, int recScore)
        {
            Iterations++;
            if (nStepScoreBest <= recScore + GetRestMinScore(Shrimps, Rooms))
                return 1;

            if (isCorrectOrder(Shrimps))
            {
                Console.WriteLine("Best score: {0}   - time {1,8:0.000} Seconds", recScore, stopwatch.ElapsedMilliseconds / 1000.0);
                return 0;
            }


            foreach (Shrimp S in Shrimps)
            {
                Room RStart = Rooms[S.RoomId];

                foreach (Room REnd in Rooms)
                {
                    if (isMovable(S, REnd.Id, Rooms))
                    {

                        List<Room> RoomsTmp = new List<Room>();
                        foreach (Room RR in Rooms)
                            RoomsTmp.Add((Room)RR.Clone());

                        List<Shrimp> ShrimpsTmp = new List<Shrimp>();
                        foreach (Shrimp SS in Shrimps)
                            ShrimpsTmp.Add((Shrimp)SS.Clone());

                        MoveTo(ShrimpsTmp[S.Id], REnd.Id, ShrimpsTmp, RoomsTmp);

                        int recScoreTMP = recScore + RStart.Paths[REnd.Id].Count() * S.MoveCost;

                        if (nStepScoreBest < recScoreTMP + GetRestMinScore(ShrimpsTmp, RoomsTmp))
                            return 1;

                        if (startMoving(ShrimpsTmp, RoomsTmp, recScoreTMP) == 0)
                        {
                            if (recScoreTMP < nStepScoreBest && recScoreTMP > 0)
                                nStepScoreBest = recScoreTMP;

                            return 1;
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

        private static void ParsingInputData()
        {

            List<Shrimp> TempList = new List<Shrimp>();

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
                                R.isOccupiedBy = S.Type;
                                S.RoomId = R.Id;
                                TempList.Add(S);
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
                {
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
                }

            GlobalShrimps = TempList;

            //GlobalShrimps = TempList.OrderBy(s=>s.RoomId).ToList();
            //GlobalShrimps.Reverse();
        }
    
    
    
    // the end
    }
}
