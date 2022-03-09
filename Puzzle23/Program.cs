namespace MyApp
{
    public class Room
    {
        private static int IDcounter = 0;
        public int Id { get; }
        public char OpenFor { get; set; } // A,B,C,D   X = anyone 
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
                if (Id % 4 == 3) OpenFor = 'A';
                if (Id % 4 == 0) OpenFor = 'B';
                if (Id % 4 == 1) OpenFor = 'C';
                if (Id % 4 == 2) OpenFor = 'D';
            }
            else
                                 OpenFor = 'X';
        }
        public static void ResetCounter() { IDcounter = 0; }
    }
    public class Shrimp
    {
        public char Type { get; set; } // ABCD
        public int MoveCost { get; set; } // 1 - 1000
        public int RoomId { get; set; }
        public bool ImHome { get; set; }

        public Shrimp(char Type)
        {
            this.ImHome = false;
            this.Type = Type;
            this.MoveCost = (Type == 'A') ? 1 : (Type == 'B') ? 10 : (Type == 'C') ? 100 : (Type == 'D') ? 1000 : -1;
        }
        public bool IsItRightDestination( Room Rend, char[] roomOccupied)
        {
            if (Rend.OpenFor != Type)
                return false;

            for(int i = Rend.Id + 4; i < roomOccupied.Length; i+=4)
                if (roomOccupied[i] != Type)
                    return false;

            return true;
        }
    }

    public class Program
    {
        // Answers for Data_p.txt  Part 1:  11516    Part 2: 40272

        static int nMinimalEnergy;

        static List<Room> GlobalMap = new List<Room>();
        static List<Shrimp> GlobalShrimps = new List<Shrimp>();
        static Dictionary<long, int> Cache = new Dictionary<long, int>(); // To keep calculates results of recursive function startMoving()
        static char[] roomOccupied; //Show which room is free/occupied 

        // It might take few minutes
        public static void Main(string[] args)
        {
            Console.WriteLine("Part one: {0, 10:0}", SolveTheProblem(@".\..\..\..\Data_p.txt"));
            Console.WriteLine("Part Two: {0, 10:0}", SolveTheProblem(@".\..\..\..\Data_p2.txt"));
        }

        private static int SolveTheProblem(string filepath)
        {
            Cache.Clear();
            nMinimalEnergy =  int.MaxValue;
            ParsingInputData(filepath);
            startMoving(GlobalShrimps);
            return nMinimalEnergy;
        }
        // TGenerates the shortest route between Start and End as consequence of rooms  
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
        // Returns absolute minimum of the steps to move the shrimps to their home (doesn't consider blocked path)
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
        private static int startMoving(List<Shrimp> Shrimps, int recEnergy = 0)
        {
            if (nMinimalEnergy <= recEnergy + GetRestMinEnergy(Shrimps))
                return 1;

            if (isCorrectOrder(Shrimps))
            {
                if (recEnergy < nMinimalEnergy)
                    nMinimalEnergy = recEnergy;
                
                return recEnergy;
            }

            foreach (Shrimp S in Shrimps.Where(s=>s.ImHome == false)) // Skip Shrimp who is in the right place
            {
                Room RStart = GlobalMap[S.RoomId];
                int[] BestRoomOrder = GetBestRoomOrder(S);

                for (int xx = 0;  BestRoomOrder[xx] != -1; xx++)
                {
                    if (BestRoomOrder[xx] >= GlobalMap.Count)
                        continue;

                    Room REnd = GlobalMap[BestRoomOrder[xx]];

                    if (IsGoodToMove(S, REnd, RStart))
                    {
                        (char Shrimp, int RoomId, bool imHome) previousStatus = (roomOccupied[S.RoomId], S.RoomId, S.ImHome);

                        // Move Shrimp to another room
                        roomOccupied[S.RoomId]  = '\0';
                        S.RoomId                = REnd.Id;
                        S.ImHome                = (REnd.OpenFor == S.Type)? true : false;
                        roomOccupied[REnd.Id]   = S.Type;
                        // move ends here

                        long lKey = GetKey(roomOccupied, S.Type);

                        int nMoveCost = RStart.Paths[REnd.Id].Count() * S.MoveCost;

                        if (!Cache.ContainsKey(lKey)) // If this combination was not considered before
                            if (nMinimalEnergy > recEnergy + nMoveCost + GetRestMinEnergy(Shrimps)) // and the energy to spent is less then current minimal energy
                                if (startMoving(Shrimps, recEnergy + nMoveCost) == 1) // run recursive function for the new shrimps' positions
                                    Cache.TryAdd(lKey, 1); 

                        // Restore the previous state
                        roomOccupied[REnd.Id]   = '\0';
                        S.RoomId                = previousStatus.RoomId;
                        S.ImHome                = previousStatus.imHome;
                        roomOccupied[S.RoomId]  = previousStatus.Shrimp;
                    }
                }
            }
            return 1;
        }

        private static long GetKey(char[] roomOccupied, char ShrimpType)
        {
            long lKey = (int)ShrimpType;
            int n = 0;
            foreach (char c in roomOccupied)
            {
                switch (c)
                {
                    case 'A': n = 1; break;
                    case 'B': n = 2; break;
                    case 'C': n = 3; break;
                    case 'D': n = 4; break;
                    default : n = 0; break;
                }
                lKey = lKey * 4 + n;
            }
            return lKey;
        }
        // small optimization - proposes the optimnal order of room for given type
        private static int[] GetBestRoomOrder(Shrimp S)
        {
            int[] BestRoomOrder = new int[20];
            switch (S.Type)
            {
                case 'D':                BestRoomOrder = new int[] { 26, 22, 18, 14, 7, 9, 5, 3, 10, 1, 0, -1 }; break;
                case 'C':                BestRoomOrder = new int[] { 25, 21, 17, 13, 7, 5, 3, 9, 10, 1, 0, -1 }; break;
                case 'B':                BestRoomOrder = new int[] { 24, 20, 16, 12, 7, 5, 3, 9, 10, 1, 0, -1 }; break;
                case 'A':                BestRoomOrder = new int[] { 23, 19, 15, 11, 7, 5, 3, 9, 10, 1, 0, -1 }; break;
            }
            return BestRoomOrder;
        }
        private static bool IsGoodToMove(Shrimp S, Room REnd, Room RStart)
        {
            // false if moves from corridor to corridor
            if (RStart.OpenFor == 'X' && REnd.OpenFor == 'X')
                return false;

            // false if the room is occupied
                if (roomOccupied[REnd.Id] != '\0')
                return false;

            // false if a shrimps moves from corridor to not the right place
            if (RStart.OpenFor == 'X' && !S.IsItRightDestination(REnd, roomOccupied)) 
                return false;

            // false if the path is blocked (any room in the lsit is occupied)
            foreach (int id in RStart.Paths[REnd.Id])
                if (roomOccupied[id] != '\0')
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
            StreamReader file3 = new(filePath);
            string[] Data = file3.ReadToEnd().Split("\r\n");

            int Columns = Data[0].Length, Rows = Data.Length; // Max Size of the caverns
            if (Rows <= 5)
                roomOccupied = new char[19]; // Number of rooms in the first part 
            else
                roomOccupied = new char[27]; // Number of rooms in the second part 

            GlobalMap.Clear();
            GlobalShrimps.Clear();
            Room.ResetCounter();

            int[,] map = new int[Rows, Columns];

            for(int R = 0; R < Rows ; R++)
                for (int C = 0; C < Data[R].Length; C++)
                {
                    map[R, C] = -1;
                    char c = Data[R][C];
                    if (c == '.' || c == 'A' || c == 'B' || c == 'C' || c == 'D')
                    {
                        Room Rm = new Room();
                        GlobalMap.Add(Rm);

                        map[R, C] = Rm.Id;

                        if (c != '.')
                        {
                            Shrimp S = new Shrimp(c);
                            S.RoomId = Rm.Id;
                            GlobalShrimps.Add(S);

                            roomOccupied[Rm.Id] = S.Type;
                        }
                    }
                }

            List<(int Dx, int Dy)> Deltas = new List<(int, int)> {(0, 1), (0, -1), (1, 0), (-1, 0)};
            // We need the second walk 
            for (int R = 0; R < Rows; R++)
                for (int C = 0; C < Data[R].Length; C++)
                    if (map[R, C] >= 0)
                    {
                        Room Rm = GlobalMap[map[R, C]];

                        foreach(var delta in Deltas)
                        if (map[R + delta.Dx, C + delta.Dy] >= 0)
                            Rm.Connections.Add(map[R + delta.Dx, C + delta.Dy]);
                    }

            // Check if the shrimp is home
            foreach (Shrimp shrimp in GlobalShrimps)
                shrimp.ImHome = shrimp.IsItRightDestination(GlobalMap[shrimp.RoomId], roomOccupied);

            // Generate all possible paths for each room
            foreach (Room RStart in GlobalMap)
                foreach (Room REnd in GlobalMap.Where(re=>re.Id != RStart.Id))
                    {
                        RStart.Paths.Add(REnd.Id, GetFullPath(RStart, REnd));
                        foreach (Room R in GlobalMap) 
                            R.isVisited = false;
                    }
        }
    }
}