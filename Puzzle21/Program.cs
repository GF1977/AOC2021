using Microsoft.Extensions.Caching.Memory;

namespace MyApp
{
    public struct Player
    {
        public int Number = 0;
        public int Position = 0;
        public int Score = 0;
        public Player(int n) { Number = n; }
    }

    public class Program
    {
        // Answers for Data_p.txt  Part 1: 920580     Part 2: 647920021341197
        static readonly string filePath = @".\..\..\..\Data_p.txt";

        static Dictionary<int, int> Probabilities = new Dictionary<int, int>() { { 3, 1 }, { 4, 3 }, { 5, 6 }, { 6, 7 }, { 7, 6 }, { 8, 3 }, { 9, 1 } };

        static int nDiceRollsNumber = 0;
        public static void Main(string[] args)
        {
            Player PlayerOne = new Player(0);
            Player PlayerTwo = new Player(1);

            // Part One
            ParsingInputData(out PlayerOne.Position, out PlayerTwo.Position);

            while (true)
            {
                PlayerOne = PlayRound(PlayerOne);
                if (PlayerOne.Score >= 1000)
                    break;

                PlayerTwo = PlayRound(PlayerTwo);
                if (PlayerTwo.Score >= 1000)
                    break;
            }

            Console.WriteLine("Part one:            {0, 20:0}", Math.Min(PlayerOne.Score, PlayerTwo.Score) * nDiceRollsNumber);

            // Part Two;

            PlayerOne = new Player(0);
            PlayerTwo = new Player(1);
            ParsingInputData(out PlayerOne.Position, out PlayerTwo.Position);

            (long , long) wins = PlayPartTwo(0, PlayerOne.Position, 0, PlayerTwo.Position, 0);

            Console.WriteLine("Part Two:            {0, 20:0}", Math.Max(wins.Item1, wins.Item2));
        }

        private static Player PlayRound(Player P)
        {

            nDiceRollsNumber += 3;
            int RollDice = 3 * ((nDiceRollsNumber - 1) % 100);

            P.Position = (P.Position - 1 + RollDice) % 10 + 1;
            P.Score += P.Position;

            return P;
        }

        private static (int, int) PlayRoundPartTwo(int Position, int Score, int roll)
        {
            Position = (Position - 1 + roll) % 10 + 1;
            Score = Score + Position;
            return (Position, Score);
        }

        
        private static (long, long) PlayPartTwo(int Player, int Position0,  int Score0, int Position1 , int Score1)
        {
            if (Score0 >= 21)
                return (1, 0);
            else if (Score1 >= 21)
                return (0, 1);

            (long win0, long win1) wins = (0, 0);

            (long win0, long win1) winsTemp = (0, 0);

            foreach(var P in Probabilities)
            {
                    if (Player == 0)
                    {
                        (int new_pos, int new_score) A = PlayRoundPartTwo(Position0, Score0, P.Key);
                        winsTemp = PlayPartTwo(1, A.new_pos, A.new_score, Position1, Score1);
                    }
                    else
                    {
                        (int new_pos, int new_score) B = PlayRoundPartTwo(Position1, Score1, P.Key);
                        winsTemp = PlayPartTwo(0, Position0, Score0, B.new_pos, B.new_score) ;
                    }


                wins.win0 += winsTemp.win0 * P.Value;
                wins.win1 += winsTemp.win1 * P.Value;
            }





            return wins;
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