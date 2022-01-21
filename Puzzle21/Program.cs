namespace MyApp
{
    public class Program
    {
        // Answers for Data_p.txt  Part 1:      Part 2: 
        static readonly string filePath = @".\..\..\..\Data_p.txt";
        static int[] nPlayersPosition = { 0, 0 };
        static int[] nPlayersScore = { 0, 0 };


        static int nDiceLastNumber = 0;
        static int nDiceRollsNumber = 0;

        public static void Main(string[] args)
        {
            ParsingInputData();
            //for(int x = 0 ; x < 310; x++)
            //{
            //    Console.WriteLine(RollDice());
            //}

            //nPlayersScore[0] < 1000 && nPlayersScore[1] < 1000
            while (true)
            {
                if (!PlayTheGame())
                    break;
            }
            
            int nLoserScore = Math.Min(nPlayersScore[0], nPlayersScore[1]); 


            Console.WriteLine("nLoserScore:         {0, 10:0}", nLoserScore);
            Console.WriteLine("nDiceRollsNumber:    {0, 10:0}", nDiceRollsNumber);
            Console.WriteLine("Part one:            {0, 10:0}", nLoserScore*nDiceRollsNumber);
        }

        private static bool PlayTheGame()
        {
            bool bRes = false;

            for (int nPlayer = 0; nPlayer < 2; nPlayer++)
            {
                string Roll;
                nPlayersPosition[nPlayer] = (nPlayersPosition[nPlayer] + RollDice(out Roll)) % 10;
                if (nPlayersPosition[nPlayer] == 0)
                    nPlayersPosition[nPlayer] = 10;
                nPlayersScore[nPlayer] += nPlayersPosition[nPlayer];
                Console.WriteLine("Player {0} rolls {1} and moves to space {2} for a total score of {3}.", nPlayer + 1, Roll, nPlayersPosition[nPlayer], nPlayersScore[nPlayer]);
                if (nPlayersScore[0] >= 1000 || nPlayersScore[1] >= 1000)
                {
                    return false;
                }
            }
            
           return true;
        }

        private static int RollDice(out string Roll)
        {
            Roll = string.Empty;
            for (int i = 0; i<3; i++)
            {
                nDiceLastNumber = (nDiceLastNumber + 1) % 100;
                if(nDiceLastNumber == 0) nDiceLastNumber = 100;
                
                Roll += " " + nDiceLastNumber.ToString();
                nDiceRollsNumber++;
            }
            //Roll = (nDiceLastNumber - 2).ToString() + "+" + (nDiceLastNumber - 1).ToString() + "+" + (nDiceLastNumber).ToString();

            return nDiceLastNumber*3 - 3;

        }

        private static void ParsingInputData()
        {
            StreamReader file = new(filePath);
            string line = file.ReadLine();
            string[] parts = line.Split("position: ");
            nPlayersPosition[0] = int.Parse(parts[1]);

            line = file.ReadLine();
            parts = line.Split("position: ");
            nPlayersPosition[1] = int.Parse(parts[1]);

        }
    }
}
