namespace MyApp
{
    public class Program
    {
        // Answers for Data_p.txt  Part 1: 920580     Part 2: 
        static readonly string filePath = @".\..\..\..\Data_t.txt";
        static int nDiceRollsNumber = 0;
        public static void Main(string[] args)
        {
            (int Position, int Score) PlayerOne = (0, 0);
            (int Position, int Score) PlayerTwo = (0, 0);

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

            int nLowerScore = Math.Min(PlayerOne.Score, PlayerTwo.Score);

            Console.WriteLine("nLoserScore:         {0, 10:0}", nLowerScore);
            Console.WriteLine("nDiceRollsNumber:    {0, 10:0}", nDiceRollsNumber);
            Console.WriteLine("Part one:            {0, 10:0}", nLowerScore * nDiceRollsNumber);
        }
        private static (int,int) PlayRound((int Position, int Score) Player)
        {
            nDiceRollsNumber += 3;
            int RollDice = 3 * ((nDiceRollsNumber - 1) % 100);

            Player.Position = (Player.Position - 1 + RollDice) % 10 + 1;
            Player.Score += Player.Position;

            return Player;
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