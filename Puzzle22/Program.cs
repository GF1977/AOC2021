namespace MyApp
{
    public class Command
    {
        public string cmd;
        public (int Start, int End) X { get; }
        public (int Start, int End) Y { get; }
        public (int Start, int End) Z { get; }
        public long Size = 0;

        public Command() { }
        public Command((int Start, int End) x, (int Start, int End) y, (int Start, int End) z, string cmd = "")
        {
            this.X = x;
            this.Y = y;
            this.Z = z;

            this.cmd = cmd;

            Size = GetCuboidSize();
        }
        private long GetCuboidSize()
        {
            if (!isValid()) 
                return 0;

            long A = (Math.Abs(X.Start - X.End) + 1);
            A *= (Math.Abs(Y.Start - Y.End) + 1);
                   A*=(Math.Abs(Z.Start - Z.End) + 1);


            return A;
        }
        public bool isValid(bool limit = false)
        {
            bool a = false;
            if (limit)
                return (X.Start >= -50 && X.End <= 50 && Y.Start >= -50 && Y.End <= 50 && Z.Start >= -50 && Z.End <= 50);
            else
                return (X.Start <= X.End) && (Y.Start <= Y.End) && (Z.Start <= Z.End);
        }

        public static (int Start, int End) IntersectionCheck((int Start, int End) C1, (int Start, int End) C2)
        {
            if ((C1.Start < C2.Start && C1.End < C2.Start) || (C1.Start > C2.End && C1.End > C2.End))
                return (1, -1); // not possible value = the intersection doesn't exist

            (int Start, int End) Result;
            if (C1.Start <= C2.Start && C2.Start <= C1.End)
            {
                if (C1.End <= C2.End)
                    Result = (C2.Start, C1.End);
                else
                    Result = (C2.Start, C2.End);
            }
            else
            {
                Result = (C1.Start, C2.End);
                if (C1.End <= C2.End)
                    Result = (C1.Start, C1.End);
                else
                    Result = (C1.Start, C2.End);
            }

            return Result;
        }
        public static Command operator^ (Command C1, Command C2)
        {
            Command Result = new Command(IntersectionCheck(C1.X, C2.X), IntersectionCheck(C1.Y, C2.Y), IntersectionCheck(C1.Z, C2.Z));

            Result.Size = Result.GetCuboidSize();

            if (C1.cmd ==  "on" && C2.cmd ==  "on") Result.cmd = "off";
            if (C1.cmd ==  "on" && C2.cmd == "off") Result.cmd = "off";
            if (C1.cmd == "off" && C2.cmd ==  "on") Result.cmd = "on";
            if (C1.cmd == "off" && C2.cmd == "off") Result.cmd = "on";

            return Result;
        }
    }
    public class Program
    {
        // Answers for Data_p.txt  Part 1: 570915     Part 2: 
        static readonly string filePath = @".\..\..\..\Data_p.txt";
        
        static Dictionary<string,bool> Cuboids = new Dictionary<string,bool>();
        static List<Command> commands = new List<Command>();
        public static void Main(string[] args)
        {
            ParsingInputData();
            Console.WriteLine("Part one: {0, 10:0}", PartOne());
            Console.WriteLine("Part one: {0, 10:0}", 2);



            List<Command> queue = new List<Command>();


            foreach (Command command in commands)
            {
                //if (command.isValid(limit: true) == false) continue;

                if (queue.Contains(command) == false && command.cmd == "on")
                {
                    queue.Add(command);
                }

                List<Command> queue2 = new List<Command>();
                foreach (Command Q in queue)
                {
                    Command NewC = Q ^ command;
                    if(Q != command && NewC.Size > 0)
                        queue2.Add(NewC);

                }

                foreach(Command v in queue2)
                    queue.Add(v);

            }

            long Res = 0;
            foreach (Command C in queue)
            {
               // Console.WriteLine("{0, 3:0} {1, 16:0} = {2, 18:0}  ", C.cmd, C.Size , Res);
                if(C.cmd == "on")
                    Res+=C.Size;
                else
                    Res-=C.Size;


                
            }

            Console.WriteLine("---------------------------------------");
            Console.WriteLine("Res = {0}", Res);


        }
        private static int PartOne()
        {
            int nRes = 0;
            foreach(Command cmd in commands)
                if (cmd.isValid(limit: true))
                    nRes += TurnOnCube(cmd);

            return nRes;
        }




        private static int TurnOnCube(Command cmd)
        {
            int nRes = 0;
            for(int x = cmd.X.Start; x <= cmd.X.End; x++)
                for (int y = cmd.Y.Start; y <= cmd.Y.End; y++)
                    for (int z = cmd.Z.Start; z <= cmd.Z.End; z++)
                    {
                        string key = x.ToString() + ":" + y.ToString() + ":" + z.ToString();
                        Cuboids.TryAdd(key, false);
                        if (cmd.cmd == "on" && Cuboids[key] == false)
                        {
                            Cuboids[key] = true;
                            nRes++;
                        }
                        if (cmd.cmd == "off" && Cuboids[key] == true)
                        {
                            Cuboids[key] = false;
                            nRes--;
                        }
                    }

            return nRes;
        }
        private static void ParsingInputData()
        {
            using (StreamReader file = new(filePath))
                while (!file.EndOfStream)
                {
                    string line = file.ReadLine();
                    string cmd = line.Split(" ")[0];

                    string[] parts = line.Split(",");
                   
                    string[] StartEnd = parts[0].Split("=")[1].Split("..");
                    (int,int) X = (int.Parse(StartEnd[0]), int.Parse(StartEnd[1]));

                    StartEnd = parts[1].Split("=")[1].Split("..");
                    (int, int) Y = (int.Parse(StartEnd[0]), int.Parse(StartEnd[1]));

                    StartEnd = parts[2].Split("=")[1].Split("..");
                    (int, int) Z = (int.Parse(StartEnd[0]), int.Parse(StartEnd[1]));

                    commands.Add(new Command(X,Y,Z, cmd));  
                }
        }
    }
}
