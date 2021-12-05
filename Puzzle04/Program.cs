using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace MyApp // Note: actual namespace depends on the project name.
{
     public  class BingoCard
    {
        const int CARD_SIZE = 5;
        
        private static int instanceCounter;
        private readonly int instanceId;
        private bool bWin;

        private readonly int[,]  CardNumbers = new int [CARD_SIZE, CARD_SIZE];
        private readonly bool[,] CardChecks  = new bool[CARD_SIZE, CARD_SIZE];

        public BingoCard(int[,] CardData)
        {
            this.instanceId = ++instanceCounter;
            this.bWin = false;
            for (int r = 0; r < CARD_SIZE; r++)
                for (int c = 0; c < CARD_SIZE; c++)
                {
                    CardNumbers[r, c] = CardData[r, c];
                    CardChecks [r, c] = false;
                }
        }
        public bool Winner { get { return bWin; } }
        private bool setWin {  set { bWin = true; } }
        public int GetId { get { return instanceId; } }
        public static int CardSize { get { return CARD_SIZE; } }
        public bool isFiveInRowOrCol(int row, int col)
        {
            int nNumbersInRow = 0;
            int nNumbersInCol = 0;
            for (int i = 0; i < CARD_SIZE; i++)
            {
                if (CardChecks[row, i] == true) nNumbersInRow++;
                if (CardChecks[i, col] == true) nNumbersInCol++;
            }

            if (nNumbersInRow == CARD_SIZE || nNumbersInCol == CARD_SIZE)
            {
                setWin = true;
                return true;
            }

            return false;
        }

        public bool isWinner(int nDrawNumber)
        {
            for (int r = 0; r < CARD_SIZE; r++)
                for (int c = 0; c < CARD_SIZE; c++)
                {
                    if (CardNumbers[r, c] == nDrawNumber)
                    { 
                        CardChecks[r, c] = true;
                        return isFiveInRowOrCol(r,c);
                    }
                }

            return false;
        }

        public int GetSumOfUnmarkedNumbers()
        {
            int res = 0;

            for (int r = 0; r < CARD_SIZE; r++)
                for (int c = 0; c < CARD_SIZE; c++)
                {
                    if (CardChecks[r, c] == false)
                        res+= CardNumbers[r, c];
                }

            return res;
        }
    }
    public class Program
    {
        // Answer for Data_p.txt  Part 1: 25023     Part 2: 2634
        static readonly string filePath = @".\..\..\..\Data_p.txt";
        static int[]? nDrawNumbers;
        static List<BingoCard> BingoCardsList = new List<BingoCard>();
        public static void Main(string[] args)
        {
            // parsing the input file
            List<string> InputData = new();
            using (StreamReader file = new(filePath))
            {
                GetDrawNumbers(file.ReadLine());
                while (!file.EndOfStream)
                {
                    string? line = file.ReadLine();
                    InputData.Add(line);
                }
            }

            CreateBingoCards(InputData);
            PlayTheBingo();
        }

        private static void PlayTheBingo()
        {
            int nBingoCardId = 0;
            foreach (int num in nDrawNumbers)
                foreach (BingoCard bingoCard in BingoCardsList)
                {
                    // ignore the card which has won already
                    if (bingoCard.Winner)
                        continue;

                    if (bingoCard.isWinner(num))
                    {
                        int nSumm = bingoCard.GetSumOfUnmarkedNumbers();
                        // Showing the first card and the last card
                        if (nBingoCardId == 0 || nBingoCardId == BingoCardsList.Count - 1)
                            Console.WriteLine("Bingo! Card Id: {3,2:0}   Summ: {0,4:0}   Last Num: {1,2:0}   Answer: {2,6:0}", nSumm, num, nSumm * num, bingoCard.GetId);

                        nBingoCardId++;
                    }
                }
        }

        // Parsing the Input Data and populating BingoCardsList with the instances of the BingoCard
        private static void CreateBingoCards(List<string> inputData)
        {
            int nRow = 0;
            int[,] CardNumbers = new int[BingoCard.CardSize, BingoCard.CardSize];
            foreach (string cardRow in inputData)
            {
                if (cardRow == "")
                    continue;

                // Replacing double space (before single digit number) by single space
                // Removing the leading space
                string sFancyCardRow = cardRow.Replace("  ", " ").Trim();

                string[] sRow = sFancyCardRow.Split(" ");

                for(int c = 0; c < BingoCard.CardSize; c++)
                    CardNumbers[nRow, c] = Int32.Parse(sRow[c]);

                nRow++;

                if(nRow == BingoCard.CardSize)
                {
                    BingoCardsList.Add(new BingoCard(CardNumbers));
                    nRow = 0;
                }
            }
        }

        private static void GetDrawNumbers(string sDrawNumbers)
        {
            string[] sDrawNumbersArray = sDrawNumbers.Split(",");
            nDrawNumbers = new int[sDrawNumbersArray.Length];

            int nIndex = 0;
            foreach (string s in sDrawNumbersArray)
                nDrawNumbers[nIndex++] = int.Parse(s);
        }
    }
}