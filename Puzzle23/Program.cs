using System.Diagnostics;

namespace MyApp
{
    public class Room
    {
        private static int IDcounter = 0;
        public int Id { get;  }
        public string OpenFor { get; set; } // ABCD
        public string isOccupiedBy { get; set; }
        public List<int> Connections { get;  }
        public bool isVisited { get; set; }

        public Dictionary<int, List<int>> Paths = new Dictionary<int, List<int>>();
        public Room()
        {
            this.Id = IDcounter++;
            this.isOccupiedBy = "";
            this.OpenFor = "ABCD";
            this.isVisited = false;

            if (Id == 1) Connections = new List<int>() { 2};
            if (Id == 2) Connections = new List<int>() { 1,16};
            if (Id == 3) Connections = new List<int>() { 16,17};
            if (Id == 4) Connections = new List<int>() { 17,18};
            if (Id == 5) Connections = new List<int>() { 18, 19};
            if (Id == 6) Connections = new List<int>() { 19,7};
            if (Id == 7) Connections = new List<int>() { 6 };
            if (Id == 8) Connections = new List<int>() { 12, 16 };
            if (Id == 9) Connections = new List<int>() {17, 13 };
            if (Id == 10) Connections = new List<int>() {18, 14  };
            if (Id == 11) Connections = new List<int>() {19, 15 };
            if (Id == 12) Connections = new List<int>() {8 };
            if (Id == 13) Connections = new List<int>() {9 };
            if (Id == 14) Connections = new List<int>() {10 };
            if (Id == 15) Connections = new List<int>() {11 };
            if (Id == 16) Connections = new List<int>() {2,3,8 };
            if (Id == 17) Connections = new List<int>() {3,4,9};
            if (Id == 18) Connections = new List<int>() {4,5,10};
            if (Id == 19) Connections = new List<int>() {5,6,11};

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
       // public bool JustMoved { get; set; }
        public bool ImHome { get; set; }

        public Shrimp(string Type)
        {
            this.Id = IDcounter++;
            this.ImHome = false;
            //this.JustMoved = false;
            this.Type = Type;
            this.MoveCount = 0;
            this.RoomId = RoomId;
           
           
            this.MoveCost = (Type == "A") ? 1 : (Type == "B") ? 10 : (Type == "C") ? 100 : (Type == "D") ? 1000 : -1;
        }
        public object Clone()
        { 
            return this.MemberwiseClone();
        }

        public void CheckHome(List<Room> Rooms)
        {
            if (Type=="A")
            {
                if((RoomId == 12) || (RoomId == 8 && Rooms[12].isOccupiedBy == "A"))
                    ImHome = true;

            }
            if (Type == "B")
            {
                if ((RoomId == 13) || (RoomId == 9 && Rooms[13].isOccupiedBy == "B"))
                    ImHome = true;

            }

            if (Type == "C")
            {
                if ((RoomId == 14) || (RoomId == 10 && Rooms[14].isOccupiedBy == "C"))
                    ImHome = true;

            }
            if (Type == "D")
            {
                if ((RoomId == 15) || (RoomId == 11 && Rooms[15].isOccupiedBy == "D"))
                    ImHome = true;

            }

        }
    }

    public class Program
    {
        // Answers for Data_p.txt  Part 1:      Part 2: 
        static readonly string filePath = @".\..\..\..\Data_t.txt";
        static List<int> InputData = new List<int>();

        //static List<string> MovingOption = new List<string>();
        //static List<string> MovingOptionBest = new List<string>();

        static int nStepCountBest = int.MaxValue;
        static int nStepScoreBest =int.MaxValue;
        static int LastMovedShrimp = 0;
        static Stopwatch stopwatch;

        static Dictionary<int, long> _memo = new() { { 0, 0 }, { 1, 1 } };


        public static void Main(string[] args)
        {
            //ParsingInputData();
            //RoomsEtalon = InitRooms();
            List<Room> Rooms = InitRooms();
            List<Shrimp> Shrimps = InitShrimps();

            foreach (Shrimp shrimp in Shrimps)
                shrimp.CheckHome(Rooms);

            int a, b;

            foreach (Room RStart in Rooms.Where(r => r.Id != 0))
                foreach (Room REnd in Rooms.Where(r => r.Id != 0))
                    if (RStart.Id != REnd.Id)
                    {
                        List<int> tmp = GetFullPath(RStart, REnd, Rooms);
                        tmp.Reverse();
                        RStart.Paths.Add(REnd.Id, tmp);

                        foreach (Room R in Rooms) R.isVisited = false;
                    }
            

            stopwatch = new Stopwatch();
            stopwatch.Start();
            

            startMoving(Shrimps, Rooms, 0, 0 , out a, out b);

            //foreach (string S in MovingOptionBest)
            //    Console.WriteLine(S);

            Console.WriteLine("Total time {0,8:0.000} Seconds", stopwatch.ElapsedMilliseconds / 1000.0);

            Console.WriteLine("Part one: {0, 10:0}", nStepCountBest);
            Console.WriteLine("Part two: {0, 10:0}", nStepScoreBest);
        }




        private static List<Shrimp> InitShrimps()
        {
            List < Shrimp > Shrimps = new List<Shrimp>();

            for (int i = 0; i < 8; i++)
            {
                string Type = string.Empty;
                if (i == 0 || i == 1) Type = "A";
                if (i == 2 || i == 3) Type = "B";
                if (i == 4 || i == 5) Type = "C";
                if (i == 6 || i == 7) Type = "D";
                Shrimp S = new Shrimp(Type);
                Shrimps.Add(S);
            }



            Shrimps[0].RoomId = 12;
            Shrimps[1].RoomId = 15;
            Shrimps[2].RoomId = 8;
            Shrimps[3].RoomId = 10;
            Shrimps[4].RoomId = 9;
            Shrimps[5].RoomId = 14;
            Shrimps[6].RoomId = 13;
            Shrimps[7].RoomId = 11;


            // the set below is for testing A changed with B

            //Shrimps[0].RoomId = 9;
            //Shrimps[1].RoomId = 12;
            //Shrimps[2].RoomId = 8;
            //Shrimps[3].RoomId = 13;
            //Shrimps[4].RoomId = 10;
            //Shrimps[5].RoomId = 14;
            //Shrimps[6].RoomId = 11;
            //Shrimps[7].RoomId = 15;


            return Shrimps;
        }

        private static List<Room> InitRooms()
        {
            List<Room> Rooms = new List<Room>();

            for (int i = 0; i < 20; i++)
            {
                Room R = new Room();
                Rooms.Add(R);
            }

            Rooms[8].OpenFor = Rooms[12].OpenFor = "A";
            Rooms[9].OpenFor = Rooms[13].OpenFor = "B";
            Rooms[10].OpenFor = Rooms[14].OpenFor = "C";
            Rooms[11].OpenFor = Rooms[15].OpenFor = "D";


            Rooms[12].isOccupiedBy = "A";
            Rooms[15].isOccupiedBy = "A";
            Rooms[8].isOccupiedBy = "B";
            Rooms[10].isOccupiedBy = "B";
            Rooms[9].isOccupiedBy = "C";
            Rooms[14].isOccupiedBy = "C";
            Rooms[13].isOccupiedBy = "D";
            Rooms[11].isOccupiedBy = "D";

            // the set below is for testing A changed with B

            //Rooms[9].isOccupiedBy = "A";
            //Rooms[12].isOccupiedBy = "A";
            //Rooms[8].isOccupiedBy = "B";
            //Rooms[13].isOccupiedBy = "B";
            //Rooms[10].isOccupiedBy = "C";
            //Rooms[14].isOccupiedBy = "C";
            //Rooms[11].isOccupiedBy = "D";
            //Rooms[15].isOccupiedBy = "D";


            return Rooms;
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

        private static bool IsRightDistination(Shrimp S, Room Rend, List<Room> Rooms)
        {
            if (S.Type == "A" && (Rend.Id ==  8 && Rooms[12].isOccupiedBy == "A") || Rend.Id == 12) return true;
            if (S.Type == "B" && (Rend.Id ==  9 && Rooms[13].isOccupiedBy == "B") || Rend.Id == 13) return true;
            if (S.Type == "C" && (Rend.Id == 10 && Rooms[14].isOccupiedBy == "C") || Rend.Id == 14) return true;
            if (S.Type == "D" && (Rend.Id == 11 && Rooms[15].isOccupiedBy == "D") || Rend.Id == 15) return true;

            return false;
        }
        public static bool isMovable(Shrimp S, int toRoom, List<Room> Rooms)
        {
            if (toRoom == 0 || S.RoomId == toRoom || Rooms[toRoom].isOccupiedBy != "") 
                return false;

            if (Rooms[S.RoomId].OpenFor == "ABCD" && !IsRightDistination(S, Rooms[toRoom], Rooms)) 
                return false;

            if (Rooms[toRoom].Connections.Count == 3) 
                return false;

            if (Rooms[toRoom].OpenFor.Contains(S.Type) == false) 
                return false;

            foreach (int id in Rooms[S.RoomId].Paths[toRoom])
                if (Rooms[id].isOccupiedBy != "") return false;

            return true;
        }

        internal static string MoveTo(Shrimp S, int toRoom, List<Shrimp> Shrimps, List<Room> Rooms)
        {
            //foreach(Shrimp SS in Shrimps)
            //SS.JustMoved = false;

            //S.JustMoved = true;
            LastMovedShrimp = S.Id;

            Room Rstart = Rooms[S.RoomId];
            Room Rend = Rooms[toRoom];

            string Res = S.Type.ToString() + S.Id.ToString() + " moves from " + Rstart.Id + " to "  + Rend.Id;

            Rstart.isOccupiedBy = "";
            Rend.isOccupiedBy = S.Type;

            if (Rend.OpenFor != "ABCD") S.ImHome = true;

            S.RoomId = Rend.Id;

            return Res;
        }


        private static int startMoving(List<Shrimp> Shrimps, List<Room> Rooms, int recStepCount, int recScore, out int outStepCount, out int outScore)
        {
            outStepCount = 0;
            outScore = 0;

            if (nStepScoreBest <= recScore)
                return 1;


            //int rDestination;
            if (isCorrectOrder(Shrimps))
            {
                outStepCount = recStepCount;
                outScore = recScore;

                Console.WriteLine("Best score: {0}   - time {1,8:0.000} Seconds", recScore, stopwatch.ElapsedMilliseconds / 1000.0);
                return 0;
            }

            
            foreach(Shrimp S in Shrimps.Where(s=>s.ImHome == false && s.Id != LastMovedShrimp))
            {
                Room RStart = Rooms[S.RoomId];

                foreach (Room REnd in Rooms)
                {
                    //int res;
                    

                    if (isMovable(S, REnd.Id, Rooms))
                        {

                        int res = RStart.Paths[REnd.Id].Count();
                        if (res > 0)
                        {

                            List<Room> RoomsTmp = new List<Room>();
                            foreach (Room RR in Rooms)
                                RoomsTmp.Add((Room)RR.Clone());

                            List<Shrimp> ShrimpsTmp = new List<Shrimp>();
                            foreach (Shrimp SS in Shrimps)
                                ShrimpsTmp.Add((Shrimp)SS.Clone());

                            string output = MoveTo(ShrimpsTmp[S.Id], REnd.Id, ShrimpsTmp, RoomsTmp);

                            //MovingOption.Add(output);


                            int recStepCountTMP = recStepCount + res;
                            int recScoreTMP = recScore + res * S.MoveCost;

                            if (nStepScoreBest < recScoreTMP)
                                return 1;

                            if (startMoving(ShrimpsTmp, RoomsTmp, recStepCountTMP, recScoreTMP, out recStepCountTMP, out recScoreTMP) == 0)
                            {

                                if (recScoreTMP < nStepScoreBest && recScoreTMP > 0)
                                {
                                    nStepCountBest = recStepCountTMP;
                                    nStepScoreBest = recScoreTMP;
                                    //MovingOptionBest.Clear();
                                    //MovingOptionBest.AddRange(MovingOption);
                                }


                                //MovingOption.Clear();
                                return 1;
                            }
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
            using (StreamReader file = new(filePath))
                while (!file.EndOfStream)
                {
                    string line = file.ReadLine();
                    string[] parts = line.Split(",");

                    foreach (string s in parts)
                        InputData.Add(int.Parse(s));
                }
        }
    }
}
