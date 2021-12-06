using System;
using System.Collections.Generic;
using System.Linq;
using static System.Math;

namespace MyApp // Note: actual namespace depends on the project name.
{
    public struct Coordinates
    {
        public int X;
        public int Y;

        public Coordinates(int x, int y)
        {
            X = x; Y = y; 
        }
    }
    public class Segment
    {
        public Coordinates Start;
        public Coordinates End;
        
        // h = horizontal, v = vertical, d = diagonal
        public string? sType;

        public Segment(Coordinates S, Coordinates E)
        {
            this.Start = S; this.End = E;
            this.sType = "d";
            // d = diagonal, v=vertical, h=horizontal
            if (S.Y == E.Y)  this.sType = "h";
            if (S.X == E.X)  this.sType = "v";
        }
    }
    public class Program
    {
        const int ARRAY_SIZE = 1000;
        // Answers for Data_p.txt  Part 1: 5585     Part 2: 17193
        static readonly string filePath = @".\..\..\..\Data_p.txt";

        public static List<Segment> segments = new List<Segment>();
        public static int[,] data = new int[ARRAY_SIZE, ARRAY_SIZE];
        public static void Main(string[] args)
        {
            ParsingInputData();
           
            SolveThePuzzle(1);
            Console.WriteLine("Part one: {0, 6:0}", GetOverlaps());

            data = new int[ARRAY_SIZE, ARRAY_SIZE];
            SolveThePuzzle(2);
            Console.WriteLine("Part two: {0, 6:0}", GetOverlaps());
        }

        private static int GetOverlaps()
        {
            int res = 0;
            for (int x = 0; x < ARRAY_SIZE; x++)
                for (int y = 0; y < ARRAY_SIZE; y++)
                    if (data[y, x] > 1)
                        res++;

            return res;
        }
        private static void SolveThePuzzle(int nPart = 1)
        {
            foreach (Segment seg in segments)
            {
                if(seg.sType == "h")
                    for (int x = Min(seg.Start.X, seg.End.X); x <= Max(seg.Start.X, seg.End.X); x++)
                        data[x, seg.Start.Y]++;

                if (seg.sType == "v")
                    for (int y = Min(seg.Start.Y, seg.End.Y); y <= Max(seg.Start.Y, seg.End.Y); y++)
                        data[seg.Start.X,y]++;
                
                if (seg.sType == "d" && nPart == 2)
                {
                    int deltaX = -1, deltaY = -1;
                    
                    if (seg.Start.X < seg.End.X) deltaX = 1; 
                    if (seg.Start.Y < seg.End.Y) deltaY = 1;

                    for (int i = 0; i <= Abs(seg.End.X - seg.Start.X); i++)
                        data[seg.Start.X + (deltaX * i), seg.Start.Y + (deltaY * i)]++;
                }
            }
        }
        private static void ParsingInputData()
        {
            List<string> InputData = new();
            using (StreamReader file = new(filePath))
                while (!file.EndOfStream)
                { 
                    string line = file.ReadLine();
                    string[] sParts  = line.Split(" -> ");
                    string[] sStart  = sParts[0].Split(",");
                    string[] sEnd    = sParts[1].Split(",");

                    Coordinates startXY = new Coordinates(int.Parse(sStart[0]), int.Parse(sStart[1]));
                    Coordinates endXY   = new Coordinates(int.Parse(sEnd[0]), int.Parse(sEnd[1]));

                    Segment segment = new Segment(startXY, endXY);
                    segments.Add(segment);
                }
        }
    }
}