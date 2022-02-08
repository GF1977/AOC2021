namespace MyApp
{
    public class Room
    {
        private static int IDcounter = 1;
        public int Id { get; set; }
        public string OpenFor { get; set; } // ABCD
        public bool isImpty { get; set; }
        public List<int> Connections { get; set; }

        public Room()
        {
            this.Id = IDcounter++;
            this.isImpty = false;

            if (Id == 1) Connections = new List<int>() { 2};
            if (Id == 2) Connections = new List<int>() { 1, 3, 8};
            if (Id == 3) Connections = new List<int>() { 2, 8, 4, 9 };
            if (Id == 4) Connections = new List<int>() { 3, 9, 5, 10};
            if (Id == 5) Connections = new List<int>() { 4,10, 6, 11 };
            if (Id == 6) Connections = new List<int>() { 5, 11, 7 };
            if (Id == 7) Connections = new List<int>() { 6 };
            if (Id == 8) Connections = new List<int>() { 2, 3, 12 };
            if (Id == 9) Connections = new List<int>() {3, 4, 13 };
            if (Id == 10) Connections = new List<int>() {4, 5, 14  };
            if (Id == 11) Connections = new List<int>() {5, 6, 15 };
            if (Id == 12) Connections = new List<int>() {8 };
            if (Id == 13) Connections = new List<int>() {9 };
            if (Id == 14) Connections = new List<int>() {10 };
            if (Id == 15) Connections = new List<int>() {11 };
        }   
    }

    public class Shrimp
    {
        public string Type { get; set; } // ABCD
        public int Id { get; set; }
        public int MoveCount { get; set; }
        public int MoveCost { get; set; } // 1 - 1000
        public int RoomId { get; set; }

        public Shrimp(string Type)
        {
            this.Type = Type;
            this.MoveCount = 0;
            this.RoomId = RoomId;
            this.MoveCost = (Type == "A") ? 1 : (Type == "B") ? 10 : (Type == "C") ? 100 : (Type == "D") ? 1000 : -1;
        }
    }

    public class Program
    {
        // Answers for Data_p.txt  Part 1:      Part 2: 
        static readonly string filePath = @".\..\..\..\Data_t.txt";
        static List<int> InputData = new List<int>();
        public static void Main(string[] args)
        {
            //ParsingInputData();
            List<Room> Rooms = new List<Room>();
            List<Shrimp> Shrimps = new List<Shrimp>();

            for (int i = 0; i < 15; i++)
            {
                Room R = new Room();
                Rooms.Add(R);
            }

            for(int i = 0; i< 8; i++)
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


            Console.WriteLine("Part one: {0, 10:0}", 1);
            Console.WriteLine("Part one: {0, 10:0}", 2);
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
