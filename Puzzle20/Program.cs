namespace MyApp
{
    public class Program
    {
        // Answers for Data_p.txt  Part 1: 5391     Part 2: 
        static readonly string filePath = @".\..\..\..\Data_p1.txt";
        static List<string> InputData = new List<string>();
        static char[] IEA;  //image enhancement algorithm
        static char[,] field_current;
        static char[,] field_new;
        static int nIteration = 0;

        public static void Main(string[] args)
        {
            
            ParsingInputData();
            //DrawField();
            EnhanceInmage();
            //DrawField();
            
            EnhanceInmage();
            //DrawField();
            int a = GetEnabledPixels();

            for (int i = 0; i < 48; i++)
            {
                 EnhanceInmage();
               // DrawField();

            }
            int b = GetEnabledPixels();
            //DrawField();

            Console.WriteLine("Part one: {0, 10:0}", a);
            Console.WriteLine("Part one: {0, 10:0}", b);
        }

        public static void EnhanceInmage()
        {
            int nFieldSize = field_current.GetLength(0);


            field_new = new char[nFieldSize + 2, nFieldSize + 2];

            for (int x = 0; x < nFieldSize ; x++)
                for (int y = 0; y < nFieldSize; y++)
                {
                    field_new[y + 1, x + 1] = CalculateNewPixel(x, y);
                }
            SwapTheFileds();
            nIteration = 1 - nIteration;
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


            for (int x = 0; x < nFieldSize; x++)
                for (int y = 0; y < nFieldSize; y++)
                    if (field_current[x, y] == '#') nRes++;

            return nRes;
        }

        private static char CalculateNewPixel(int xCenter, int yCenter)
        {
            string sRes = string.Empty;
            int nFieldSize = field_current.GetLength(0);
            for (int y = yCenter - 1; y <= yCenter + 1; y++)
            {
                for (int x = xCenter - 1; x <= xCenter + 1; x++)
                {
                    if (x < 1 || x >= nFieldSize - 1 || y < 1 || y >= nFieldSize  - 1)
                    {

                        if(IEA[0] == '.' || nIteration == 0)
                            sRes += "0";

                        if (IEA[0] == '#'  && nIteration == 1)
                            sRes += "1";
                        
                            
                    }
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
            field_current = new char[nFieldSize + 2, nFieldSize + 2];
            int x = 1;
            int y = 1;
            foreach (string row in InputData)
            {
                foreach (char c in row)
                {
                    field_current[y, x] = c;
                    x++;
                }
                x = 1;
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
