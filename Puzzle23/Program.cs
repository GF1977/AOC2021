namespace MyApp
{
    public class Room
    {
        private static int IDcounter = 0;
        public int Id { get;  }
        public string OpenFor { get; set; } // ABCD
        public string isOccupied { get; set; }
        public List<int> Connections { get;  }
        public bool isVisited { get; set; }

        public Room()
        {
            this.Id = IDcounter++;
            this.isOccupied = "";
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
        { return this.MemberwiseClone(); }

    }

    public class Shrimp
    {
        private static int IDcounter = 0;
        public string Type { get; set; } // ABCD
        public int Id { get; set; }
        public int MoveCount { get; set; }
        public int LastMoveCount { get; set; }
        public int MoveCost { get; set; } // 1 - 1000
        public int RoomId { get; set; }
        public int LastRoomId { get; set; }
        public bool JustMoved { get; set; }
        public bool ImHome { get; set; }

        public Shrimp(string Type)
        {
            this.Id = IDcounter++;
            this.ImHome = false;
            this.JustMoved = false;
            this.Type = Type;
            this.MoveCount = 0;
            this.RoomId = RoomId;
            this.LastRoomId = -1;
            this.LastMoveCount = int.MaxValue;
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
                if((RoomId == 12) || (RoomId == 8 && Rooms[12].isOccupied == "A"))
                    ImHome = true;

            }
            if (Type == "B")
            {
                if ((RoomId == 13) || (RoomId == 9 && Rooms[13].isOccupied == "B"))
                    ImHome = true;

            }

            if (Type == "C")
            {
                if ((RoomId == 14) || (RoomId == 10 && Rooms[14].isOccupied == "C"))
                    ImHome = true;

            }
            if (Type == "D")
            {
                if ((RoomId == 15) || (RoomId == 11 && Rooms[15].isOccupied == "D"))
                    ImHome = true;

            }

        }
    }

    public class Program
    {
        // Answers for Data_p.txt  Part 1:      Part 2: 
        static readonly string filePath = @".\..\..\..\Data_t.txt";
        static List<int> InputData = new List<int>();

        static List<string> MovingOption = new List<string>();
        static List<string> MovingOptionBest = new List<string>();

        static int NNN = 0;
        static int nStepCountBest = int.MaxValue;
        static int nStepScoreBest =int.MaxValue;

        public static void Main(string[] args)
        {
            //ParsingInputData();
            //RoomsEtalon = InitRooms();
            List<Room> Rooms = InitRooms();
            List<Shrimp> Shrimps = InitShrimps();

            foreach (Shrimp shrimp in Shrimps)
                shrimp.CheckHome(Rooms);
            //Shrimp S = Shrimps[2];
            //bool a = isMovable(S, 16);
            //Console.WriteLine(a);

            //if (startMoving(Shrimps,Rooms) == 1)                Console.WriteLine("No solution");

            //while (true)
            //int x = 1;
            //while(x > 0)
            //{
            //    foreach (Shrimp S in Shrimps.Where(s => s.ImHome == false))
            //        x = startMoving(S, Shrimps, Rooms);
            //}
            int a, b;

            startMoving(Shrimps, Rooms, 0, 0 , out a, out b);

            foreach (string S in MovingOptionBest)
                Console.WriteLine(S);

            //int res = 0;
            //bool a = IsWay(Rooms[17], Rooms[9], out res, Rooms);

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

            //Shrimps[0].RoomId = 12;
            //Shrimps[1].RoomId = 15;
            //Shrimps[2].RoomId = 8;
            //Shrimps[3].RoomId = 10;
            //Shrimps[4].RoomId = 9;
            //Shrimps[5].RoomId = 14;
            //Shrimps[6].RoomId = 13;
            //Shrimps[7].RoomId = 11;

            Shrimps[0].RoomId = 9;
            Shrimps[1].RoomId = 12;
            Shrimps[2].RoomId = 8;
            Shrimps[3].RoomId = 13;
            Shrimps[4].RoomId = 10;
            Shrimps[5].RoomId = 14;
            Shrimps[6].RoomId = 11;
            Shrimps[7].RoomId = 15;


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


            //Rooms[12].isOccupied = "A";
            //Rooms[15].isOccupied = "A";
            //Rooms[8].isOccupied = "B";
            //Rooms[10].isOccupied = "B";
            //Rooms[9].isOccupied = "C";
            //Rooms[14].isOccupied = "C";
            //Rooms[13].isOccupied = "D";
            //Rooms[11].isOccupied = "D";


            Rooms[9].isOccupied = "A";
            Rooms[12].isOccupied = "A";
            Rooms[8].isOccupied = "B";
            Rooms[13].isOccupied = "B";
            Rooms[10].isOccupied = "C";
            Rooms[14].isOccupied = "C";
            Rooms[11].isOccupied = "D";
            Rooms[15].isOccupied = "D";


            return Rooms;
        }

        private static bool IsWay(Room rStart, Room rEnd, out int nStepsResult, List<Room> Rooms, int nSteps = 0)
        {
            nStepsResult = nSteps;
            rStart.isVisited = true;
            if (rStart.Connections.Contains(rEnd.Id) && rEnd.isOccupied == "")
            {
                nStepsResult++;
                return true;
            }

            foreach (int id in rStart.Connections)
            {
                Room R = Rooms.Find(r => r.Id == id);
                if (R.isVisited == false && R.isOccupied == "" && IsWay(R, rEnd, out nStepsResult, Rooms, nStepsResult))
                {
                    nStepsResult++;
                    return true;
                }
            }

            return false;
        }
        public static bool isMovable(Shrimp S, int toRoom, List<Room> Rooms)
        {
            //if (S.JustMoved || S.ImHome) return false;
            if (S.ImHome) return false;

           // Room Rstart = Rooms.Find(r => r.Id == S.RoomId);
            Room Rend = Rooms.Find(r => r.Id == toRoom);

            if(Rend.Id == 16 || Rend.Id == 17 || Rend.Id == 18 || Rend.Id == 19 ) return false;

            if(Rend.Id == S.LastRoomId) return false;

            //if (Rend.Connections.Find(n => n == Rstart.Id) == 0)
            //    return false;

            //int x = Rend.Connections.Find(n => n == Rstart.Id);


            //if (Rend.isOccupied != "") return false;
            //if (Rstart.OpenFor == "ABCD" && Rend.OpenFor == "ABCD")
            //    if(Rstart.Id != 16 && Rstart.Id != 17 && Rstart.Id != 18 && Rstart.Id != 19)
            //        return false;
            
            if (Rend.OpenFor.Contains(S.Type) == false) return false;

            Room R = Rooms.Find(r => r.Id == toRoom);
            if (Rend.Id == 8 && Rooms[12].isOccupied != "A") return false;

            if (Rend.Id == 9 && Rooms[13].isOccupied != "B") return false;

            if (Rend.Id == 10 && Rooms[14].isOccupied != "C") return false;

            if (Rend.Id == 11 && Rooms[15].isOccupied != "D") return false;

            if (Rooms.Find(r => r.Id == S.RoomId).OpenFor == "ABCD" && Rend.OpenFor == "ABCD") return false;

            return true;
        }

        internal static string MoveTo(Shrimp S, int toRoom, List<Shrimp> Shrimps, List<Room> Rooms)
        {
            foreach(Shrimp SS in Shrimps)
                SS.JustMoved = false;
            
            S.JustMoved = true;
            Room Rstart = Rooms.Find(r => r.Id == S.RoomId);
            Room Rend = Rooms.Find(r => r.Id == toRoom);

            string Res = S.Type.ToString() + S.Id.ToString() + " moves from " + Rstart.Id + " to "  + Rend.Id;
            //Console.WriteLine("{0}{1,2:0} moves from {2, 3:0} to {3, 3:0}", S.Type, S.Id, Rstart.Id, Rend.Id);
           // Console.WriteLine(Res);

            Rstart.isOccupied = "";
            Rend.isOccupied = S.Type;

            if (Rend.OpenFor != "ABCD") S.ImHome = true;
            int nMoves = 1;


            //LastMovedShrimp = S.Id;

            S.LastRoomId = S.RoomId;
            S.RoomId = Rend.Id;
            //S.MoveCount += nMoves;

            return Res;
        }


        private static int startMoving(List<Shrimp> Shrimps, List<Room> Rooms, int recStepCount, int recScore, out int outStepCount, out int outScore)
        {

            outStepCount = 0;
            outScore = 0;

            //int rDestination;
            if (isCorrectOrder(Rooms))
            {
                outStepCount = recStepCount;
                outScore = recScore;
                return 0;
            }

            
            foreach(Shrimp S in Shrimps)
            {
                Room RStart = Rooms[S.RoomId];

                foreach (Room REnd in Rooms)
                {
                    int res;
                    foreach (Room R in Rooms) R.isVisited = false;

                    if(IsWay(RStart, REnd, out res, Rooms))
                    if (isMovable(S, REnd.Id, Rooms))
                    {

                            //S.MoveCount = res;

                            List<Room> RoomsTmp = new List<Room>();
                            foreach (Room RR in Rooms)
                                RoomsTmp.Add((Room)RR.Clone());

                            List<Shrimp> ShrimpsTmp = new List<Shrimp>();
                            foreach (Shrimp SS in Shrimps)
                                ShrimpsTmp.Add((Shrimp)SS.Clone());

                            ShrimpsTmp[S.Id].MoveCount = res;
                            //recStepCount += res;
                            //recScore += res * S.MoveCost;



                            string output = MoveTo(ShrimpsTmp[S.Id], REnd.Id, ShrimpsTmp, RoomsTmp);

                            MovingOption.Add(output);


                            int recStepCountTMP = recStepCount + res;
                            int recScoreTMP = recScore + res * S.MoveCost;

                            if (startMoving(ShrimpsTmp, RoomsTmp, recStepCountTMP, recScoreTMP,out recStepCountTMP, out recScoreTMP) == 0)
                            {
                                //outStepCount = recStepCountTMP;
                                //outScore = recScoreTMP;
                                //Console.WriteLine("--------- {0} - {1} ---------", recStepCountTMP, recScoreTMP);

                                int stepCount = 0;
                                int nRes = 0;


                                if (recScoreTMP < nStepScoreBest && MovingOption.Count > 0)
                                {
                                    nStepCountBest = recStepCountTMP;
                                    nStepScoreBest = recScoreTMP;
                                    MovingOptionBest.Clear();
                                    MovingOptionBest.AddRange(MovingOption);
                                }

                                MovingOption.Clear();
                                return 1;
                            }

                            //MoveTo(S, REnd.Id, Shrimps, Rooms);

                            //if (startMoving(Shrimps, Rooms) == 0)
                            //    return 1;


                        }
                }

                        
            }

            //int stepCount = 0;
            //int nRes = 0;

            //foreach (Shrimp Ss in MovingOption)
            //{
            //    stepCount += Ss.MoveCount;
            //    nRes += Ss.MoveCount * Ss.MoveCost;
            //}

            //if(nRes < nStepScoreBest && MovingOption.Count > 0)
            //{
            //    nStepCountBest = stepCount;
            //    nStepScoreBest = nRes;
            //    MovingOptionBest.Clear();
            //    MovingOptionBest.AddRange(MovingOption);
            //}

            //MovingOption.Clear();

            //Console.WriteLine("");
            //Console.WriteLine("Count: {0}", stepCount);
            //Console.WriteLine("Cost:  {0}", nRes);

            //Console.WriteLine("======================");
    

            outStepCount = 0;
            outScore = 0;

            return 1;
        }

        private static bool isCorrectOrder(List<Room> Rooms)
        {
            if (Rooms[ 8].isOccupied != "A") return false;
            if (Rooms[ 9].isOccupied != "B") return false;
            if (Rooms[10].isOccupied != "C") return false;
            if (Rooms[11].isOccupied != "D") return false;
            if (Rooms[12].isOccupied != "A") return false;
            if (Rooms[13].isOccupied != "B") return false;
            if (Rooms[14].isOccupied != "C") return false;
            if (Rooms[15].isOccupied != "D") return false;

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
