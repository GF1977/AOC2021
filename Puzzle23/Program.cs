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

        public bool IsItRightDestination(int RoomsCount, Room Rend, char[] roomOccupied)
        {
            if (Rend.OpenFor != Type)
                return false;

            for(int i = Rend.Id + 4; i < RoomsCount; i+=4)
                if (Program.WhoInTheRoom(i, roomOccupied) != Type)
                    return false;

            return true;
        }
        public static void ResetCounter() { IDcounter = 0; }
    }

    public class Program
    {
        // Answers for Data_p.txt  Part 1:  11516    Part 2: 40272

        static int nMinimalEnergy;
        static int LastMovedShrimp = -1;
        static Stopwatch stopwatch;

        static List<Room> GlobalMap = new List<Room>();
        static List<Shrimp> GlobalShrimps = new List<Shrimp>();
        static Dictionary<string, int> Cache = new Dictionary<string, int>();
        //static List<string> Queue = new List<string>();
        //static List<string> BestQueue = new List<string>();
        static char[] roomOccupied = new char[27];


        public static void Main(string[] args)
        {
            stopwatch = new Stopwatch();
            //stopwatch.Start();
            //SolveTheProblem(@".\..\..\..\Data_s.txt");
            SolveTheProblem(@".\..\..\..\Data_t.txt");
            //SolveTheProblem(@".\..\..\..\Data_pIO.txt");
            //SolveTheProblem(@".\..\..\..\Data_p.txt");

            //SolveTheProblem(@".\..\..\..\Data_s2.txt");
            SolveTheProblem(@".\..\..\..\Data_t2.txt");
            //SolveTheProblem(@".\..\..\..\Data_pIO2.txt");
            SolveTheProblem(@".\..\..\..\Data_p2.txt");
        }

        private static void SolveTheProblem(string filepath)
        {
            Cache.Clear();
            stopwatch.Reset();
            stopwatch.Start();
            Console.WriteLine("Calculation started: {0}", filepath);
            nMinimalEnergy =  int.MaxValue;
            ParsingInputData(filepath);
            startMoving(GlobalShrimps, 0, roomOccupied);
            Console.WriteLine("Energy: {0, 9:0}", nMinimalEnergy);
            Console.WriteLine("Total time {0,8:0.000} Seconds", stopwatch.ElapsedMilliseconds / 1000.0);
            Console.WriteLine();
            
            
            //foreach(string s in BestQueue)
            //    Console.WriteLine(s);
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
        public static string WhoInTheRoom(int RoomId, char[] roomOccupied)
        {
            return roomOccupied[RoomId].ToString();
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
        private static int startMoving(List<Shrimp> Shrimps, int recEnergy, char[] rOc)
        {
            if (nMinimalEnergy <= recEnergy + GetRestMinEnergy(Shrimps))
            {
                return 1;
            }

            if (isCorrectOrder(Shrimps))
            {
                //LastMovedShrimp = -1;
                if (recEnergy < nMinimalEnergy)
                {
                    nMinimalEnergy = recEnergy;
                    Console.WriteLine("Best Energy: {0,5:0}   - time {1,8:0.000} Seconds", recEnergy, stopwatch.ElapsedMilliseconds / 1000.0);

                }
                return recEnergy;
            }

            foreach (Shrimp S in Shrimps.Where(s=>s.ImHome == false)) // Skip Shrimp who is in the right place
            {
                Room RStart = GlobalMap[S.RoomId];
                int[] BestRoomOrder = GetBestRoomOrder(S);

                //foreach (Room REnd in GlobalMap.Where(r=>r.Connections.Count !=3)) // Skip room right above the caves
                for (int xx = 0;  BestRoomOrder[xx] != -1; xx++)
                {
                    if (BestRoomOrder[xx] >= GlobalMap.Count)
                        continue;

                    Room REnd = GlobalMap[BestRoomOrder[xx]];

                    if (IsGoodToMove(S, REnd, RStart, rOc))
                    {
                        int nMoveCost = RStart.Paths[REnd.Id].Count() * S.MoveCost;

                        char cPreviousShrimp = roomOccupied[S.RoomId];
                        int nOldRoomId = S.RoomId;
                        
                        bool bOldHome = S.ImHome;

                        roomOccupied[S.RoomId] = '\0';
                        S.RoomId = REnd.Id;
                        roomOccupied[S.RoomId] = S.Type[0];
                        if (REnd.OpenFor == S.Type)
                            S.ImHome = true;


                        // Move End
                        string sKey = GetKey(roomOccupied);

                        
                        if (!Cache.ContainsKey(sKey))
                        //if (nMinimalEnergy > recEnergy + nMoveCost + GetRestMinEnergy(Shrimps))
                        {
                            //int nOldLastMovedS = LastMovedShrimp;
                            // LastMovedShrimp = S.Id;

                            int res = startMoving(Shrimps, recEnergy + nMoveCost, roomOccupied);
                            
                            //LastMovedShrimp = nOldLastMovedS;
                            if (res == 1)
                                Cache.TryAdd(sKey, res);
                        }
                        //else
                        {
                            
                            roomOccupied[S.RoomId] = '\0';
                            S.RoomId = nOldRoomId;
                            S.ImHome = bOldHome;
                            roomOccupied[S.RoomId] = cPreviousShrimp;
                        }


                    }
                }

            }

            //LastMovedShrimp = -1;
            return 1;
        }

        private static string GetKey(char[] roomOccupied)
        {
            string sKey = "";
            foreach (char c in roomOccupied)
            {
                if (c == '\0')
                    sKey += "-";
                else
                    sKey += c;
            }

            return sKey;
        }

        private static int[] GetBestRoomOrder(Shrimp S)
        {
            int[] BestRoomOrder = new int[20];
            if (S.Type == "D")
            {
                //if (S.RoomId % 4 == 3)
                //    BestRoomOrder = new int[] { 26, 22, 18, 14, 7, 5, 3, 9, 10, 1, 0, -1 };

                //if (S.RoomId % 4 == 0)
                //    BestRoomOrder = new int[] { 26, 22, 18, 14, 5, 7, 9, 10, 3, 1, 0, -1 };

                //if (S.RoomId % 4 == 1)
                //    BestRoomOrder = new int[] { 26, 22, 18, 14, 7, 9, 10, 5, 3, 1, 0, -1 };

                //if (S.RoomId % 4 == 2)
                    BestRoomOrder = new int[] { 26, 22, 18, 14, 9, 7, 10, 5, 3, 1, 0, -1 };


            }

            if (S.Type == "C")
            {
                //if (S.RoomId % 4 == 3)
                //    BestRoomOrder = new int[] { 25, 21, 17, 13, 3, 5, 7, 9, 10, 1,  0, -1 };

                //if (S.RoomId % 4 == 0)
                //    BestRoomOrder = new int[] { 25, 21, 17, 13, 5, 7, 9, 10, 3, 1,  0, -1 };

                //if (S.RoomId % 4 == 1)
                //    BestRoomOrder = new int[] { 25, 21, 17, 13, 5, 7, 3, 9, 1,  10, 0, -1 };

                //if (S.RoomId % 4 == 2)
                    BestRoomOrder = new int[] { 25, 21, 17, 13, 7, 5, 9, 10, 3, 1,  0, -1 };


            }

            if (S.Type == "B")
                BestRoomOrder = new int[] { 24, 20, 16, 12, 7, 5, 3, 9, 10, 1, 0, -1 };

            if (S.Type == "A")
                BestRoomOrder = new int[] { 23, 19, 15, 11, 7, 5, 3, 9, 10, 1, 0, -1 };


            return BestRoomOrder;
        }

        private static bool IsGoodToMove(Shrimp S, Room REnd, Room RStart, char[] roomOccupied)
        {
            //if (RStart.OpenFor == "ABCD" && REnd.OpenFor == "ABCD")
            //    return false;

            //if (S.Id == LastMovedShrimp)
            //    return false;

            //if (S.RoomId == REnd.Id) // Skip where Start == End 
            //    return false;

            //if (REnd.Id >= 11 && REnd.OpenFor != S.Type) // Skip rooms which are not for this type (A goes to A, B to B etc)
            //    return false;

            if (WhoInTheRoom(REnd.Id, roomOccupied) != "\0") // Skip where the room is occupied
                return false;

            if (RStart.OpenFor == "ABCD" && !S.IsItRightDestination(GlobalMap.Count, REnd, roomOccupied)) // skip if a shrimps moves from corridor to not the right place
                return false;

            bool PathIsBlocked = false;
            foreach (int id in RStart.Paths[REnd.Id])
                if (WhoInTheRoom(id, roomOccupied) != "\0")
                    return false;

            return true;
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
                                roomOccupied[R.Id] = S.Type[0];
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
                shrimp.ImHome = shrimp.IsItRightDestination(GlobalMap.Count, GlobalMap[shrimp.RoomId], roomOccupied);


            //GlobalShrimps = GlobalShrimps.OrderBy(s=>s.RoomId).ToList();
            //GlobalShrimps.Reverse();
            //int id = 0;
            
            //foreach(Shrimp S in GlobalShrimps)
            //    S.Id = id++;



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
