namespace MyApp
{
    public class Program
    {
        // Answers for Data_p.txt  Part 1: 2701     Part 2: 1070
        // target area: x=281..311, y=-74..-54
        static int Xmin = 281;
        static int Xmax = 311;
        static int Ymin = -74;
        static int Ymax = -54;
        public static void Main(string[] args)
        {
            int nResOne = 0;
            int nRestwo = 0;
            int n = 0;
            for (int x = 0; x <= Xmax; x++)
                for (int y = -1000; y < 1000; y++)
                    if (LaunchProbe(x, y, out n))
                    {
                        nRestwo++;
                        if(n > nResOne) nResOne = n;
                    }

            Console.WriteLine("Part one: {0, 10:0}", nResOne);
            Console.WriteLine("Part two: {0, 10:0}", nRestwo);
        }
        private static bool LaunchProbe(int xVelocity, int yVelocity, out int highestY)
        {
            int x = 0;
            int y = 0;
            highestY = 0;

            while (x < Xmax && y > Ymin)
            {
                x += xVelocity;
                y += yVelocity;

                if (y > highestY) highestY = y;

                if (xVelocity > 0) xVelocity--;
                if (xVelocity < 0) xVelocity++;

                yVelocity--;

                if (IsBetween(x, Xmin, Xmax) && IsBetween(y, Ymin, Ymax))
                    return true;
            }

            return false;
        }
        private static bool IsBetween(int numberToCheck, int Nmin, int nMax)
        {
            return (numberToCheck >= Nmin && numberToCheck <= nMax);
        }
    }
}
