namespace MyApp
{
    public class Packet
    {
        // Header
        int version;
        int type_id;
        int? len_type_id = null;    // To indicate which subsequent binary data represents its sub-packets, an operator packet can use one of two modes
        int? total_len = null;      // len_type_id == 0 -> 15 bits are a number that represents the total length in bits of the sub-packets contained by this packet.
        int? packet_count = null;   // len_type_id == 1 -> 11 bits are a number that represents the number of sub-packets immediately contained by this packet.

        // Data
        string sLiteralData = String.Empty;
        long nLiteralData = 0;

        // SubPackets
        List<Packet> packets = new List<Packet>();

        public Packet(string input)
        {
            version = Convert.ToInt32(input.Substring(0, 3), 2);
            type_id = Convert.ToInt32(input.Substring(3, 3), 2);

            if (type_id == 4) // Literal packet
            {
                sLiteralData = input.Substring(6);
                for(int i = 0; i < sLiteralData.Length; i+=5)
                {
                    int nSizeOfSubstring = (sLiteralData.Length - i) >= 5 ? 5 : sLiteralData.Length - i;

                    string s = sLiteralData.Substring(i, nSizeOfSubstring);
                    nLiteralData = nLiteralData * 16 + Convert.ToInt32(s.Substring(1,4), 2);

                    if (s[0] == '0')
                        break;
                }
            }
            else // Operator
            {
                len_type_id = Convert.ToInt32(input.Substring(6, 1), 2);

                if (len_type_id == 0)
                {
                    total_len = Convert.ToInt32(input.Substring(7, 15), 2);
                }
                else
                {
                    packet_count = Convert.ToInt32(input.Substring(7, 11), 2);
                }

            }

        }

    }
    public class Program
    {
        // Answers for Data_p.txt  Part 1:      Part 2: 
        static readonly string filePath = @".\..\..\..\Data_t.txt";
        static string binaryInput = string.Empty;
        public static void Main(string[] args)
        {
            ParsingInputData();

            Packet P = new Packet(binaryInput);

            //Console.WriteLine("Part one: {0, 80:0}", s8);
            //Console.WriteLine("Part one: {0, 80:0}", r2);
        }


        private static void ParsingInputData()
        {
            string InputData;
            using (StreamReader file = new(filePath))
                InputData = file.ReadLine();

            binaryInput = string.Empty;
            Console.WriteLine("Hex Original {0}", InputData);

            for (int i = 0; i < InputData.Length; i += 2)
            {
                int nSizeOfSubstring = (InputData.Length - i) >= 2 ? 2 : 1;
                string sHex8 = InputData.Substring(i, nSizeOfSubstring);
                string sBin = Convert.ToString(Convert.ToInt64(sHex8, 16), 2).PadLeft(8, '0');
                binaryInput += sBin;

                Console.WriteLine("Hex {0, 10:0}    Bin {1, 80:0}", sHex8, sBin);
            }


            Console.WriteLine("Bin result {0}", binaryInput);

        }
    }
}
