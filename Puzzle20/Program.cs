namespace MyApp
{
    public class Program
    {
        static readonly int BORDER = 6;
        // Answers for Data_p.txt  Part 1: 5391     Part 2: 
        static readonly string filePath = @".\..\..\..\Data_P.txt";
        static List<string> InputData = new List<string>();
        static char[] IEA;  //image enhancement algorithm
        static char[,] field_current;
        static char[,] field_new;
        static int nIteration;
        public static void Main(string[] args)
        {
            
            ParsingInputData();
            //DrawField();
            nIteration = 1;
            EnhanceInmage();
            DrawField();
            int a =GetEnabledPixels();
            
            nIteration++;
            EnhanceInmage();
            DrawField();
            int b = GetEnabledPixels();

            //for (int i = 0; i < 60; i++)
            //{
            //    Console.WriteLine("Pixels = {0}", GetEnabledPixels());
            //    EnhanceInmage();
            //}
            

            Console.WriteLine("Part one: {0, 10:0}", a);
            Console.WriteLine("Part one: {0, 10:0}", b);
        }

        public static void EnhanceInmage()
        {
            int nFieldSize = field_current.GetLength(0);
            Console.WriteLine("Scanning area in [{0} ; {1})", 1, nFieldSize - 1);

            int nDelta = 0;

            if (nIteration % 2 == 0)
                nDelta = 2;
            else
                nDelta = 0;

            field_new = new char[nFieldSize + BORDER*2, nFieldSize + BORDER*2];

            for (int x = nDelta; x < nFieldSize - nDelta; x++)
                for (int y = nDelta; y < nFieldSize - nDelta; y++)
                {
                    char newPixel = GetBlockSumm(x, y);

                    field_new[y + BORDER, x + BORDER] = newPixel;
                }
            SwapTheFileds();
        }

        private static void SwapTheFileds()
        {
            int nFieldSize = field_new.GetLength(0);

            field_current = new char[nFieldSize, nFieldSize];
            for (int x = 0; x < nFieldSize; x++)
                for (int y = 0; y < nFieldSize; y++)
                    field_current[x, y] = field_new[x, y];

        }

        private static int GetEnabledPixels()
        {
            int  nRes = 0;
            int nFieldSize = field_current.GetLength(0);

            int nDelta = nIteration * BORDER + 2;

            for (int x = nDelta; x < nFieldSize - nDelta; x++)
                for (int y = nDelta; y < nFieldSize - nDelta; y++)
                    if (field_current[x, y] == '#') nRes++;

            return nRes;
        }

        private static char GetBlockSumm(int xCenter, int yCenter)
        {
            string sRes = string.Empty;
            for (int y = yCenter - 1; y <= yCenter + 1; y++)
            {
                for (int x = xCenter - 1; x <= xCenter + 1; x++)
                {
                    if (x < 0 || x >= field_current.GetLength(0) || y < 0 || y >= field_current.GetLength(0))
                        sRes += "0";
                    else
                        sRes += (field_current[y, x] == '#') ? "1" : "0";
                }
            }
            int nIndex = Convert.ToInt32(sRes, 2);

            return IEA[nIndex];
        }

        private static void DrawField()
        {
            int nFieldSize = field_current.GetLength(0);

            for (int x = 0; x < nFieldSize; x++)
            {
                for (int y = 0; y < nFieldSize; y++)
                {
                    char c = (field_current[x,y] == '#')? '#':'.';
                    Console.Write(c);
                }

                Console.WriteLine();
            }
            Console.WriteLine();
        }

        public static void UpdateField()
        {
            int nFieldSize = InputData.First().Length;
            field_current = new char[nFieldSize + BORDER*2, nFieldSize + BORDER*2];
            int x = BORDER;
            int y = BORDER;
            foreach (string row in InputData)
            {
                foreach (char c in row)
                {
                    field_current[y, x] = c;
                    x++;
                }
                x = BORDER;
                y++;
            }
        }

        private static void ParsingInputData()
        {
            using (StreamReader file = new(filePath))
                while (!file.EndOfStream)
                {
                    string line = file.ReadLine();
                    InputData.Add(line);
                }

            IEA = InputData.First().ToCharArray();
            InputData.RemoveAt(0);
            InputData.RemoveAt(0);
            UpdateField();
        }
    }
}
