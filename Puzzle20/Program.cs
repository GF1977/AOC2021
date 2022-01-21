namespace MyApp
{
    public class Field
    {
        private char[,] field;
        public static string IEA;  //image enhancement algorithm
        public static int nEnabledPixels = 0; // Number of enabled (lit) pixels
        public static int Space = 0; // this is to emulate the infinite space around the image. 0 means space aroudn is filled by "."   1 means by "#"
        public int GetSize() =>  field.GetLength(0);
        public void Set(int x, int y, char value) => field[x + 1, y + 1] = value; // + 1  to keep the frame (1 pixel around the image) empty
        public char Get(int x, int y) => field[x, y];
        public Field(int n) => field = new char[n + 2,n + 2];  // 2 for the frame of empty pixels around the main picture
        public Field() => field = new char[GetSize() + 2, GetSize() + 2];  // 2 for the frame of empty pixels around the main picture
        public int SpaceFlip() => Space = 1 - Space; // to switch the value from 0 to 1 and from 1 to 0  to emulate the space around is filled by 0 or 1
    }
    public class Program
    {
        // Answers for Data_t.txt   Part 1:   35     Part 2:  3351
        // Answers for Data_p.txt   Part 1: 5391     Part 2: 16383 
        // Answers for Data_p1.txt  Part 1: 5682     Part 2: 17628 
        static readonly string filePath = @".\..\..\..\Data_t.txt";
        static Field Field;
        

        public static void Main(string[] args)
        {
            EnhanceImage(2);
            Console.WriteLine("Part one: {0, 10:0}", Field.nEnabledPixels);

            EnhanceImage(50);
            Console.WriteLine("Part one: {0, 10:0}", Field.nEnabledPixels);
        }
        public static void EnhanceImage(int nRepeats)
        {
            ParsingInputData();
            for (int i = 0; i < nRepeats; i++)
            {
                Field.nEnabledPixels = 0;
                Field FieldNew = new Field(Field.GetSize());

                for (int x = 0; x < Field.GetSize(); x++)
                    for (int y = 0; y < Field.GetSize(); y++)
                        FieldNew.Set(y, x, CalculateNewPixel(x, y)); 

                Field = FieldNew;
                Field.SpaceFlip(); 
            }
        }
        private static char CalculateNewPixel(int xCenter, int yCenter)
        {
            string sRes = string.Empty;
            for (int y = yCenter - 1; y <= yCenter + 1; y++)     
                for (int x = xCenter - 1; x <= xCenter + 1; x++)
                {
                    // if the x or y outside the image (in the space or in the frame)
                    if (x < 1 || x >= Field.GetSize() - 1 || y < 1 || y >= Field.GetSize() - 1)
                    {
                        // IEA[0] is important; if IEA[0] == '.' 9 empty pixel will generate "."  ,  if IEA[0] == '#' 9 empty pixels will generate "#"
                        if (Field.IEA[0] == '.' || Field.Space == 0)
                            sRes += "0";
                        else
                            sRes += "1";
                    }
                    else
                        sRes += (Field.Get(y, x) == '#') ? "1" : "0";
                }

            int nIndex = Convert.ToInt32(sRes, 2);
            
            if (Field.IEA[nIndex] == '#') Field.nEnabledPixels++;

            return Field.IEA[nIndex];
        }
        private static void ParsingInputData()
        {
            StreamReader file = new(filePath);
            string IEA = file.ReadLine();
            file.ReadLine();  // empty one - ignoring

            List<string> InputData = new List<string>();
            while (!file.EndOfStream)
                InputData.Add(file.ReadLine());

            int nFieldSize = InputData.First().Length;
            Field = new Field(nFieldSize);
            Field.IEA = IEA;

            for (int x = 0;x < nFieldSize;x++)
                for(int y = 0;y < nFieldSize;y++)
                    Field.Set(y, x, InputData[y][x]);
        }
    }
}