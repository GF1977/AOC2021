using Microsoft.Extensions.Caching.Memory;

namespace MyApp
{
    public struct Player
    {
        public int Position = 0;
        public int Score = 0;
     }
    public class Program
    {
        // Answers for Data_p.txt  Part 1: 920580     Part 2: 647920021341197
        static Dictionary<int, int> Probabilities = new Dictionary<int, int>() { { 3, 1 }, { 4, 3 }, { 5, 6 }, { 6, 7 }, { 7, 6 }, { 8, 3 }, { 9, 1 } };
        static readonly string filePath = @".\..\..\..\Data_p.txt";
        static int nDiceRollsNumber = 0;
        public static void Main(string[] args)
        {
            // Part One
            Player PlayerOne = new Player();
            Player PlayerTwo = new Player();

            ParsingInputData(out PlayerOne.Position, out PlayerTwo.Position);
            Console.WriteLine("Part one:            {0, 20:0}", PlayPartOne(PlayerOne, PlayerTwo) * nDiceRollsNumber);

            // Part Two;
            PlayerOne = new Player();
            PlayerTwo = new Player();

            ParsingInputData(out PlayerOne.Position, out PlayerTwo.Position);

            (long , long) Universes = PlayPartTwo(0, PlayerOne,PlayerTwo);
            Console.WriteLine("Part Two:            {0, 20:0}", Math.Max(Universes.Item1, Universes.Item2));
        }
        private static Player MovePawn(Player P, int roll)
        {
            P.Position = (P.Position - 1 + roll) % 10 + 1;
            P.Score = P.Score + P.Position;
            return P;
        }
        private static int PlayPartOne(Player P1, Player P2)
        {
            while (true)
            {
                nDiceRollsNumber += 3;
                P1 = MovePawn(P1, 3 * ((nDiceRollsNumber - 1) % 100));
                if (P1.Score >= 1000) 
                    return P2.Score;

                nDiceRollsNumber += 3;
                P2 = MovePawn(P2, 3 * ((nDiceRollsNumber - 1) % 100));
                if (P2.Score >= 1000)
                    return P1.Score;
            }
        }
        // Part Two is inspired by https://www.reddit.com/r/adventofcode/comments/rl6p8y/comment/hph4r3k
        private static (long, long) PlayPartTwo(int Player, Player P1, Player P2)
        {
            if (P1.Score >= 21)
                return (1, 0);

            if (P2.Score >= 21)
                return (0, 1);

            (long P1Universes, long P2Universes) Universes = (0, 0);
            (long P1Universes, long P2Universes) UniversesTemp = (0, 0);

            foreach (var Prob in Probabilities)
            {
                if (Player == 0)
                    UniversesTemp = PlayPartTwo(1, MovePawn(P1, Prob.Key), P2);
                else
                    UniversesTemp = PlayPartTwo(0, P1, MovePawn(P2, Prob.Key));

                Universes.P1Universes += UniversesTemp.P1Universes * Prob.Value;
                Universes.P2Universes += UniversesTemp.P2Universes * Prob.Value;
            }
            return Universes;
        }
        private static void ParsingInputData(out int P1Position, out int P2Position)
        {
            StreamReader file = new(filePath);
            string[] res = file.ReadToEnd().Split("\r\n");
            P1Position = int.Parse(res[0].Split(": ")[1]);
            P2Position = int.Parse(res[1].Split(": ")[1]);
        }
    }
}