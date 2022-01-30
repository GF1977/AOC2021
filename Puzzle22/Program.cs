﻿namespace MyApp
{
    public class Command
    {
        public string cmd;
        public  (int Start, int End) X;
        public  (int Start, int End) Y;
        public  (int Start, int End) Z;
        public bool isValid()
        {
            return (X.Start >= -50 && X.End <= 50 && Y.Start >= -50 && Y.End <= 50 && Z.Start >= -50 && Z.End <= 50);
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
        }
        private static int PartOne()
        {
            int nRes = 0;
            foreach(Command cmd in commands)
                if (cmd.isValid())
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
                    Command command = new Command();

                    string line = file.ReadLine();
                    command.cmd = line.Split(" ")[0];

                    string[] parts = line.Split(",");
                   
                    string[] StartEnd = parts[0].Split("=")[1].Split("..");
                    command.X = (int.Parse(StartEnd[0]), int.Parse(StartEnd[1]));

                    StartEnd = parts[1].Split("=")[1].Split("..");
                    command.Y = (int.Parse(StartEnd[0]), int.Parse(StartEnd[1]));

                    StartEnd = parts[2].Split("=")[1].Split("..");
                    command.Z = (int.Parse(StartEnd[0]), int.Parse(StartEnd[1]));

                    commands.Add(command);  
                }
        }
    }
}
