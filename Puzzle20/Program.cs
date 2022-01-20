namespace MyApp
{
    public class Field
    {
        private char[,] field;
        public int GetSize() =>  field.GetLength(0);
        public void Set(int x, int y, char value) => field[x + 1, y + 1] = value; // + 1  to keep the frame (1 pixel around the image) empty
        public char Get(int x, int y) => field[x, y];
        public Field(int n) => field = new char[n,n];
    }
    public class Program
    {
        // Answers for Data_t.txt   Part 1:   35     Part 2:  3351
        // Answers for Data_p.txt   Part 1: 5391     Part 2: 16383 
        // Answers for Data_p0.txt  Part 1: 5682     Part 2: 17628 
        static readonly string filePath = @".\..\..\..\Data_p.txt";
        static string IEA;  //image enhancement algorithm

        static Field FieldA;
        static Field FieldB;
        static int nEnabledPixels = 0;
        static int Space = 0; // this is to emulate the infinite space around the image. 0 means "."   1 means "#"

        public static void Main(string[] args)
        {
            EnhanceImage(2);
            Console.WriteLine("Part one: {0, 10:0}", nEnabledPixels);

            EnhanceImage(50);
            Console.WriteLine("Part one: {0, 10:0}", nEnabledPixels);
        }
        public static void EnhanceImage(int nRepeats)
        {
            ParsingInputData();
            for (int i = 0; i < nRepeats; i++)
            {
                nEnabledPixels = 0;
                FieldB = new Field(FieldA.GetSize() + 2); // 2 for the frame of empty pixels around the main picture

                for (int x = 0; x < FieldA.GetSize(); x++)
                    for (int y = 0; y < FieldA.GetSize(); y++)
                        FieldB.Set(y, x, CalculateNewPixel(x, y)); 

                FieldA = FieldB;
                Space = 1 - Space; // to switch the value from 0 to 1 and from 1 to 0  to show the space around is filled by 0 or 1
            }
        }
        private static char CalculateNewPixel(int xCenter, int yCenter)
        {
            string sRes = string.Empty;
            for (int y = yCenter - 1; y <= yCenter + 1; y++)     
                for (int x = xCenter - 1; x <= xCenter + 1; x++)
                {
                    // if the x or y outside the image (in the space or in the frame)
                    if (x < 1 || x >= FieldA.GetSize() - 1 || y < 1 || y >= FieldA.GetSize() - 1)
                    {
                        // IEA[0] is important; 9 empty pixel will generate "."  , 9 enabled pixels will generate "#"
                        if(IEA[0] == '.' || Space == 0)
                            sRes += "0";

                        if (IEA[0] == '#' && Space == 1)
                            sRes += "1";
                    }
                    else
                        sRes += (FieldA.Get(y, x) == '#') ? "1" : "0";
                }

            int nIndex = Convert.ToInt32(sRes, 2);
            if (IEA[nIndex] == '#')
                nEnabledPixels++;

            return IEA[nIndex];
        }
        private static void ParsingInputData()
        {
            StreamReader file = new(filePath);
            IEA = file.ReadLine();
            file.ReadLine();  // empty one - ignoring

            List<string> InputData = new List<string>();
            while (!file.EndOfStream)
                InputData.Add(file.ReadLine());

            int nFieldSize = InputData.First().Length;
             FieldA = new Field(nFieldSize + 2); // frame

            for (int x = 0;x < nFieldSize;x++)
                for(int y = 0;y < nFieldSize;y++)
                    FieldA.Set(y, x, InputData[y][x]);
        }
    }
}