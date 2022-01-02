namespace MyApp
{
    public class Packet
    {
        // CONSTANTS
        const int LITERAL_SIZE      =  5;
        const int VERISON_SIZE      =  3;
        const int TYPE_ID_SIZE      =  3;
        const int CONVERT_FROM_BIN  =  2;
        const int TOTAL_LEN_SIZE    = 15;
        const int PACKET_CNT_SIZE   = 11;
        const int LEN_TYPE_ID_SIZE  =  1;

        // PACKETS IDs
        const int TYPE_LITERAL = 4;

        public int Pointer { get; }
        // Header
        public int version { get; }
        int type_id;
        int len_type_id     = -1;   // To indicate which subsequent binary data represents its sub-packets, an operator packet can use one of two modes
        int total_len       = -1;   // len_type_id == 0 -> 15 bits are a number that represents the total length in bits of the sub-packets contained by this packet.
        int packet_count    = -1;   // len_type_id == 1 -> 11 bits are a number that represents the number of sub-packets immediately contained by this packet.
        // Data
        string sLiteralData = String.Empty;
        long nLiteralData = 0;
        // SubPackets
        List<Packet> SubPackets = new List<Packet>();
        public Packet(string input)
        {
            version = Convert.ToInt32(input.Substring(0, VERISON_SIZE), CONVERT_FROM_BIN);
            type_id = Convert.ToInt32(input.Substring(3, TYPE_ID_SIZE), CONVERT_FROM_BIN);
            
            Pointer = VERISON_SIZE + TYPE_ID_SIZE;

            if (type_id == TYPE_LITERAL)
            {

                sLiteralData = input.Substring(Pointer);

                for (int i = 0; i < sLiteralData.Length; i+= LITERAL_SIZE)
                {
                    Pointer += LITERAL_SIZE;
                    string s = sLiteralData.Substring(i, LITERAL_SIZE);
                    nLiteralData = nLiteralData * 16 + Convert.ToInt32(s.Substring(1, LITERAL_SIZE - 1), CONVERT_FROM_BIN);
                    if (s[0] == '0')
                        break;
                }
                //Console.WriteLine("nLiteralData = {0}", nLiteralData);
            }
            else // Operator
            {
                len_type_id = Convert.ToInt32(input.Substring(Pointer, LEN_TYPE_ID_SIZE), CONVERT_FROM_BIN);

                if (len_type_id == 0)
                {
                    Pointer += TOTAL_LEN_SIZE + LEN_TYPE_ID_SIZE;
                    total_len = Pointer + Convert.ToInt32(input.Substring(7, TOTAL_LEN_SIZE), CONVERT_FROM_BIN);
                }
                else
                {
                    Pointer += PACKET_CNT_SIZE + LEN_TYPE_ID_SIZE;
                    packet_count = Convert.ToInt32(input.Substring(7, PACKET_CNT_SIZE), CONVERT_FROM_BIN);
                }

                int packet_number = 0;
                while (packet_number < packet_count || Pointer < total_len)
                {
                    Packet P = new Packet(input.Substring(Pointer));
                    SubPackets.Add(P);
                    Pointer += P.Pointer;
                    packet_number++;
                }
            }
        }
        public int GetVersionSum()
        {
            int nVersionSum = this.version;
            foreach(Packet P in SubPackets)
                nVersionSum += P.GetVersionSum();

            return nVersionSum;
        }
    }
    public class Program
    {
        // Answers for Data_p.txt  Part 1: 854      Part 2: 
        static readonly string filePath = @".\..\..\..\Data_p.txt";
        public static void Main(string[] args)
        {
            string binaryInput = ParsingInputData();
            Packet P = new Packet(binaryInput);
            Console.WriteLine(" ---- Version Sum: {0} ----", P.GetVersionSum());
        }
        private static string ParsingInputData(string? sHexString = null)
        {
            if (sHexString == null)
                using (StreamReader file = new(filePath))
                    sHexString = file.ReadLine();

            string binaryInput = string.Empty;
            for (int i = 0; i < sHexString.Length; i ++)
            {
                string sHex = sHexString.Substring(i, 1);
                binaryInput += Convert.ToString(Convert.ToInt64(sHex, 16), 2).PadLeft(4, '0');
            }

            return binaryInput;
        }
    }
}