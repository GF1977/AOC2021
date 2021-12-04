using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace MyApp // Note: actual namespace depends on the project name.
{
     public  class BingoCard
    {
        const int CARD_SIZE = 5;
        public static int CardSize { get { return CARD_SIZE; } } 
        private readonly int[,] CardNumbers = new int[CARD_SIZE, CARD_SIZE];
        private readonly bool[,] CardChecks = new bool[CARD_SIZE, CARD_SIZE];

        public BingoCard(int[,] CardData)
        {
            for (int r = 0; r < CARD_SIZE; r++)
                for (int c = 0; c < CARD_SIZE; c++)
                {
                    CardNumbers[r, c] = CardData[r, c];
                    CardChecks[r, c] = false;
                }
        }

        public int CheckRows()
        {
            int res;
            for (int r = 0; r < CARD_SIZE; r++)
            {
                res = 0;
                for (int c = 0; c < CARD_SIZE; c++)
                    if (CardChecks[r, c] == true)
                        res++;

                if (res == CARD_SIZE)
                    return r;
            }
            return -1;
        }

        public int CheckColumns()
        {
            int res;
            for (int c = 0; c < CARD_SIZE; c++)
            {
                res = 0;
                for (int r = 0; r < CARD_SIZE; r++)
                    if (CardChecks[r, c] == true)
                        res++;

                if (res == CARD_SIZE)
                    return c;
            }
            return -1;
        }

        public void MarkNumer(int nDrawNumber)
        {
            for (int r = 0; r < CARD_SIZE; r++)
                for (int c = 0; c < CARD_SIZE; c++)
                {
                    if (CardNumbers[r, c] == nDrawNumber)
                        CardChecks[r, c] = true;
                }
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
        // Answer for Data_p.txt  Part 1: 25023     Part 2: 
        static readonly string filePath = @".\..\..\..\Data_p.txt";
        static int[] nDrawNumbers;
        static List<BingoCard> BingoCardsList = new List<BingoCard>();
        public static void Main(string[] args)
        {
            // parsing the input file
            List<string> InputData = new();
            using (StreamReader file = new(filePath))
                while (!file.EndOfStream)
                {
                    string? line = file.ReadLine();
                    if (line != null)
                        InputData.Add(line);
                }

            GetDrawNumbers(InputData);
            // Remove Draw Numbers line and empty line after it
            InputData.RemoveAt(0);
            InputData.RemoveAt(0);
            // Added empty line at the end of input as terminator
            InputData.Add("");
            CreateBingoCards(InputData);

            foreach(int num in nDrawNumbers)
                foreach(BingoCard bingoCard in BingoCardsList)
                    {
                        bingoCard.MarkNumer(num);
                        int nWinnerRow = bingoCard.CheckRows();
                        int nWinnerCol = bingoCard.CheckColumns();

                        if(nWinnerRow >= 0 || nWinnerCol >= 0)
                            {
                                int nSumm = bingoCard.GetSumOfUnmarkedNumbers();
                                Console.WriteLine("Bingo! Summ = {0}    Last Num = {1}   Answer = {2}", nSumm, num, nSumm * num);
                                return;
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
                {
                    BingoCard bingoCard = new BingoCard(CardNumbers);
                    BingoCardsList.Add(bingoCard);
                    nRow = 0;
                    continue;
                }

                // Replacing double space (before single digit number) by single space
                // Removing the leading space
                string sFancyCardRow = cardRow.Replace("  ", " ").Trim();

                string[] sRow = sFancyCardRow.Split(" ");
                for(int c = 0; c < BingoCard.CardSize; c++)
                {
                    CardNumbers[nRow, c] = Int32.Parse(sRow[c]);
                }
                nRow++;
            }

        }

        private static void GetDrawNumbers(List<string> inputData)
        {
            string[] sDrawNumbers = inputData.First().Split(",");
            nDrawNumbers = new int[sDrawNumbers.Length];

            int nIndex = 0;
            foreach (string s in sDrawNumbers)
            {
                nDrawNumbers[nIndex++] = int.Parse(s);
            }
        }
    }
}