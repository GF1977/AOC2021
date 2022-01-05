using System.Text.RegularExpressions;

namespace MyApp
{
    public class SFNum
    {
        private string number { get; set; }
        private Dictionary<string, string> pairs = new Dictionary<string, string>();
        public SFNum() { }
        public SFNum(string s)
        {
            number = s;
            int nKey = 0;
            while (true)
            {
                int nPointerStart = 0;
                int nPointerEnd = 0;
                for(int i = 0; i < s.Length; i++)
                {
                    if (s[i] == '[') nPointerStart = i;
                    if (s[i] == ']')
                    {
                        nPointerEnd = i;
                        break;
                    }
                }
                if (nPointerEnd == 0)
                    break;

                string sPair = s.Substring(nPointerStart, nPointerEnd - nPointerStart + 1);
                string sKey = "K" + nKey.ToString();
                pairs.Add(sKey, sPair);

                s = s.Remove(nPointerStart, nPointerEnd - nPointerStart + 1);
                s = s.Insert(nPointerStart, sKey);

                nKey++;
            }
        }
        public static SFNum operator +(SFNum A, SFNum B)
        {
            if (A.number == null) return B;
            if (B.number == null) return A;

            return new SFNum("[" + A.number + "," + B.number + "]");
        }
        private SFNum CheckAndSplit()
        {
            string number_for_parsing = number.Replace("[", ".[.").Replace("]", ".].").Replace(",", ".,.").Replace("..", ".");
            string[] allElements = number_for_parsing.Split(".");

            for (int i = 0; i < allElements.Length; i++)
                if(int.TryParse(allElements[i], out int N))
                    if(N >= 10)
                    {
                        int nNewLeft  = (int)Math.Floor(N / 2.0);
                        int nNewRight = (int)Math.Ceiling(N / 2.0);
                        allElements[i] = "[" + nNewLeft + "," + nNewRight + "]";

                        string sFinalNumber = string.Empty;
                        for (int j = 0; j < allElements.Length; j++)
                            sFinalNumber += allElements[j];

                        return new SFNum(sFinalNumber);
                    }

            return new SFNum();
        }
        private void UpdateNumber(string sKeyToUpdate, string sNewValue)
        {
            pairs[sKeyToUpdate] = sNewValue;
            string s = pairs.Last().Value;
            for(int i = pairs.Count - 1; i >= 0;  i--)
            {
                string sKey = "K"+i.ToString();
                s = s.Replace(sKey, pairs[sKey]);
            }
            number = s;
        }
        private SFNum Explode(string sKeyToExplode)
        {
            string sToExplode = pairs[sKeyToExplode];
            this.UpdateNumber(sKeyToExplode,"X");
            string[] twoInts = sToExplode.Substring(1, sToExplode.Length - 2).Split(",");

            int nLeft  = int.Parse(twoInts[0]);
            int nRight = int.Parse(twoInts[1]);
            
            string number_for_parsing = number.Replace("[", ".[.").Replace("]", ".].").Replace(",", ".,.").Replace("..", ".");
            string[] allElements = number_for_parsing.Split(".");

            for (int i = 0; i < allElements.Length; i++)
                if (allElements[i] == "X")
                {
                    for (int a = i; a > 0; a--)
                        if (int.TryParse(allElements[a], out int N))
                        {
                            allElements[a] = (N + nLeft).ToString();
                            break;
                        }

                    for (int a = i; a < allElements.Length; a++)
                        if (int.TryParse(allElements[a], out int N))
                        {
                            allElements[a] = (N + nRight).ToString();
                            break;
                        }
                }

            string sFinalNumber = string.Empty;
            for (int i = 0; i < allElements.Length; i++)
                sFinalNumber += allElements[i];

            sFinalNumber = sFinalNumber.Replace("X", "0");

            return new SFNum(sFinalNumber);
        }
        public SFNum Reduce()
        {
            SFNum FinalNumber = new SFNum(this.number);
            while (true)
            {
                SFNum C1 = FinalNumber.ReduceOneStep();
                if (C1.number == null || C1 == FinalNumber)
                    break;
                FinalNumber = C1;// new SFNum(C1.number);
            }

            return FinalNumber;
        }
        private SFNum ReduceOneStep()
        {
            SFNum Res = new SFNum();
            foreach (var pair in pairs.Where(v=>v.Value.Contains("K") == false))
                if (GetDepthLevel(pair.Key) > 4)
                {
                    Res = Explode(pair.Key);
                    return Res;
                }
            Res = CheckAndSplit();
            return Res;
        }
        private int GetDepthLevel(string sKey)
        {
            int nDepthLevel = 0;
            while(true)
            {
                nDepthLevel++;
                if (sKey != pairs.Last().Key)
                    sKey = pairs.First(v => v.Value.Contains(sKey)).Key;
                else
                    break;
            }
            return nDepthLevel;
        }
        private int GetMagnitudeForPair(string sKey)
        {
            string[] twoInts = pairs[sKey].Substring(1, pairs[sKey].Length - 2).Split(",");

            bool A = int.TryParse(twoInts[0], out int nLeft);
            if (A == false)
                nLeft  = GetMagnitudeForPair(twoInts[0]);

            bool B = int.TryParse(twoInts[1], out int nRight);
            if (B == false)
                nRight = GetMagnitudeForPair(twoInts[1]);

            return nLeft * 3 + nRight * 2;
        }
        public int GetMagnitude()
        {
            return GetMagnitudeForPair(pairs.Last().Key);
        }
        public override string ToString() => $"{number}";
    }
    public class Program
    {
        // Answers for Data_p.txt  Part 1: 4417      Part 2: 4796
        static readonly string filePath = @".\..\..\..\Data_p.txt";
        static List<string> InputData = new List<string>();
        public static void Main(string[] args)
        {
            long startTimeStamp = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            ParsingInputData();
            Console.WriteLine("Part one: {0, 10:0}", GetPartOne());
            ParsingInputData();
            Console.WriteLine("Part one: {0, 10:0}    Time: {1}", GetPartTwo(), DateTimeOffset.Now.ToUnixTimeMilliseconds() - startTimeStamp);
        }
        private static int GetPartTwo()
        {
            int nMaxMagnitude = 0;
            for (int i = 0; i < InputData.Count; i++)
                for (int j = 0; j < InputData.Count; j++)
                {
                    if (i == j) break;

                    SFNum FinalNumber = new SFNum(InputData[i]) + new SFNum(InputData[j]);
                    nMaxMagnitude = Math.Max(nMaxMagnitude, FinalNumber.Reduce().GetMagnitude());
                }
            return nMaxMagnitude;
        }
        private static int GetPartOne()
        {
            SFNum FinalNumber = new SFNum();
            foreach (var SFN in InputData)
                FinalNumber = (FinalNumber + new SFNum(SFN)).Reduce();

            return FinalNumber.GetMagnitude();
        }
        private static void ParsingInputData()
        {
            InputData.Clear();
            using (StreamReader file = new(filePath))
                while (!file.EndOfStream)
                    InputData.Add(file.ReadLine());
        }
    }
}