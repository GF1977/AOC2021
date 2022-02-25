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

        static int nMinimalEnergy = int.MaxValue;
        //static int LastMovedShrimp = -1;
        static Stopwatch stopwatch;

        static List<Room> GlobalMap = new List<Room>();
        static List<Shrimp> GlobalShrimps = new List<Shrimp>();
        static List<string> Queue = new List<string>();
        static List<string> BestQueue = new List<string>();
        static char[] roomOccupied = new char[27];


        public static void Main(string[] args)
        {
            stopwatch = new Stopwatch();
            //stopwatch.Start();
            //SolveTheProblem(@".\..\..\..\Data_s.txt");
            //SolveTheProblem(@".\..\..\..\Data_t.txt");
            //SolveTheProblem(@".\..\..\..\Data_pIO.txt");
            //SolveTheProblem(@".\..\..\..\Data_p.txt");

            //SolveTheProblem(@".\..\..\..\Data_s2.txt");
            //SolveTheProblem(@".\..\..\..\Data_t2.txt");
            SolveTheProblem(@".\..\..\..\Data_pIO2.txt");
            //SolveTheProblem(@".\..\..\..\Data_p2.txt");
        }

        private static void SolveTheProblem(string filepath)
        {
            stopwatch.Restart();
            Console.WriteLine("Calculation started: {0}", filepath);
            nMinimalEnergy = int.MaxValue;
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
        private static bool IsOptimalMove(Shrimp S, Room REnd, Room RStart)
        {
            if(S.RoomId > 10 )
            {

            }

            return true;
        }
        private static int startMoving(List<Shrimp> Shrimps, int recEnergy, char[] rOc)
        {
            //LastMovedShrimp = -1;
            if (nMinimalEnergy <= recEnergy + GetRestMinEnergy(Shrimps))
                return 1;

            if (isCorrectOrder(Shrimps))
            {
                if (recEnergy < nMinimalEnergy)
                {
                    nMinimalEnergy = recEnergy;
                    BestQueue.Clear();
                    foreach(string s in Queue)
                        BestQueue.Add(s);
                    
                }
                Queue.Clear();
                //Console.WriteLine("Best Energy: {0,5:0}   - time {1,8:0.000} Seconds", recEnergy, stopwatch.ElapsedMilliseconds / 1000.0);
                return recEnergy;
            }



            foreach (Shrimp S in Shrimps.Where(s=>s.ImHome == false)) // Skip Shrimp who is in the right place
            {
                Room RStart = GlobalMap[S.RoomId];
                int[] BestRoomOrder = new int[GlobalMap.Count];
                int RoomCount;

                if (GlobalMap.Count == 27)
                    RoomCount = 11;
                else
                    RoomCount = 9;

                if (S.Type == "D")
                {
                    BestRoomOrder[0] = 26;
                    BestRoomOrder[1] = 22;
                    BestRoomOrder[2] = 18;
                    BestRoomOrder[3] = 14;
                    BestRoomOrder[4] = 9;
                    BestRoomOrder[5] = 7;
                    BestRoomOrder[6] = -1;
                    BestRoomOrder[7] = 5;
                    BestRoomOrder[8] = 3;
                    BestRoomOrder[9] = -1;
                    BestRoomOrder[10] = -1;
                }

                if (S.Type == "C")
                {
                    BestRoomOrder[0] = 25;
                    BestRoomOrder[1] = 21;
                    BestRoomOrder[2] = 17;
                    BestRoomOrder[3] = 13;
                    BestRoomOrder[4] = 7;
                    BestRoomOrder[5] = 5;
                    BestRoomOrder[6] = 9;
                    BestRoomOrder[7] = 3;
                    BestRoomOrder[8] = -1;
                    BestRoomOrder[9] = 1;
                    BestRoomOrder[10] = 0;
                    BestRoomOrder[10] = -1;
                }

                if (S.Type == "B")
                {
                    BestRoomOrder[0] = 24;
                    BestRoomOrder[1] = 20;
                    BestRoomOrder[2] = 16;
                    BestRoomOrder[3] = 12;
                    BestRoomOrder[4] = 3;
                    BestRoomOrder[5] = 5;
                    BestRoomOrder[6] = 1;
                    BestRoomOrder[7] = 7;
                    BestRoomOrder[8] = 0;
                    BestRoomOrder[9] = 9;
                    BestRoomOrder[10] = 10;
                    BestRoomOrder[11] = -1;
                }

                if (S.Type == "A")
                {
                    BestRoomOrder[0] = 23;
                    BestRoomOrder[1] = 19;
                    BestRoomOrder[2] = 15;
                    BestRoomOrder[3] = 11;
                    BestRoomOrder[4] = 1;
                    BestRoomOrder[5] = 3;
                    BestRoomOrder[6] = 0;
                    BestRoomOrder[7] = 5;
                    BestRoomOrder[8] = 7;
                    BestRoomOrder[9] = 9;
                    BestRoomOrder[10] = 10;
                    BestRoomOrder[11] = -1;
                }


                //foreach (Room REnd in GlobalMap.Where(r=>r.Connections.Count !=3)) // Skip room right above the caves
                for (int xx = 0; xx < RoomCount; xx++)
                {
                   if (BestRoomOrder[xx] == -1 || BestRoomOrder[xx] >= GlobalMap.Count)
                            continue;

                   Room REnd = GlobalMap[BestRoomOrder[xx]];
                    //if (!IsOptimalMove(S, RStart, REnd))
                    //    continue;

                    if (IsGoodToMove(S, REnd, RStart, rOc))
                    {
                        List<Shrimp> ShrimpsTmp = new List<Shrimp>();
                        foreach (Shrimp SS in Shrimps)
                            ShrimpsTmp.Add((Shrimp)SS.Clone());

                        char[] roomOccupiedTMP = new char[27];
                        for(int i = 0; i < roomOccupiedTMP.Length; i++)
                        roomOccupiedTMP[i] = rOc[i];

                        Shrimp STmp = ShrimpsTmp[S.Id];

                        // Move Shrimp
                        roomOccupiedTMP[STmp.RoomId] = '\0';
                        STmp.RoomId = REnd.Id;
                        roomOccupiedTMP[STmp.RoomId] = STmp.Type[0];

                        if (REnd.OpenFor == STmp.Type)
                            STmp.ImHome = true;

                        int nMoveCost = RStart.Paths[REnd.Id].Count() * S.MoveCost;
                        string sMove = S.Type+S.Id+ "   "+ RStart.Id + " -> "+REnd.Id;
                        Queue.Add(sMove);
                        // Move End

                        if (nMinimalEnergy > recEnergy + nMoveCost +GetRestMinEnergy(ShrimpsTmp))
                        {
                            startMoving(ShrimpsTmp, recEnergy + nMoveCost, roomOccupiedTMP);
                        }
                    }
                }

            }
            Queue.Clear();
            return 1;
        }



        private static bool IsGoodToMove(Shrimp S, Room REnd, Room RStart, char[] roomOccupied)
        {
            if (S.RoomId == REnd.Id) // Skip where Start == End 
                return false;

            if (REnd.Id >= 11 && REnd.OpenFor != S.Type) // Skip rooms which are not for this type (A goes to A, B to B etc)
                return false;

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
