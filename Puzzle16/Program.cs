namespace MyApp
{
    public class Packet
    {
        public int PacketSize { get; }
        // Header
        public int version { get; }
        int type_id;
        int len_type_id = -1;    // To indicate which subsequent binary data represents its sub-packets, an operator packet can use one of two modes
        int  total_len = -1;      // len_type_id == 0 -> 15 bits are a number that represents the total length in bits of the sub-packets contained by this packet.
        int packet_count = -1;   // len_type_id == 1 -> 11 bits are a number that represents the number of sub-packets immediately contained by this packet.
        // Data
        string sLiteralData = String.Empty;
        long nLiteralData = 0;
        // SubPackets
        List<Packet> SubPackets = new List<Packet>();
        public Packet(string input)
        {
            version = Convert.ToInt32(input.Substring(0, 3), 2);
            type_id = Convert.ToInt32(input.Substring(3, 3), 2);

            if (type_id == 4) // Literal packet
            {
                PacketSize = 6;
                sLiteralData = input.Substring(6);
                nLiteralData = 0;
                for (int i = 0; i < sLiteralData.Length; i+=5)
                {
                    PacketSize += 5;
                    string s = sLiteralData.Substring(i, 5);
                    nLiteralData = nLiteralData * 16 + Convert.ToInt32(s.Substring(1,4), 2);
                    if (s[0] == '0')
                        break;
                }
                //Console.WriteLine("nLiteralData = {0}", nLiteralData);
            }
            else // Operator
            {
                len_type_id = Convert.ToInt32(input.Substring(6, 1), 2);

                if (len_type_id == 0)
                {
                    total_len = Convert.ToInt32(input.Substring(7, 15), 2);
                    PacketSize = 22;
                }
                else
                {
                    packet_count = Convert.ToInt32(input.Substring(7, 11), 2);
                    PacketSize = 18;
                }

                int packet_number = 0;
                while (packet_number < packet_count || PacketSize < total_len + 22)
                {
                    Packet P = new Packet(input.Substring(PacketSize));
                    SubPackets.Add(P);
                    PacketSize += P.PacketSize;
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
        static string binaryInput = string.Empty;
        public static void Main(string[] args)
        {
            ParsingInputData();
            Packet P = new Packet(binaryInput);
            Console.WriteLine(" ---- Version Sum: {0} ----", P.GetVersionSum());
        }
        private static void ParsingInputData(string sHexString = null)
        {
            string InputData = sHexString;

            if (sHexString == null)
                using (StreamReader file = new(filePath))
                    InputData = file.ReadLine();

            binaryInput = string.Empty;
            for (int i = 0; i < InputData.Length; i ++)
            {
                string sHex8 = InputData.Substring(i, 1);
                string sBin = Convert.ToString(Convert.ToInt64(sHex8, 16), 2).PadLeft(4, '0');
                binaryInput += sBin;
            }
        }
    }
}