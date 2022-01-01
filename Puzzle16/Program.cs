namespace MyApp
{
    public class Packet
    {
        // Internal info
        public int PacketSize { get; }
        //private static int nPointer = 0;

        // Header
        public int version { get; }
        int type_id;
        int len_type_id = -1;    // To indicate which subsequent binary data represents its sub-packets, an operator packet can use one of two modes
        int  total_len = -1;      // len_type_id == 0 -> 15 bits are a number that represents the total length in bits of the sub-packets contained by this packet.
        int packet_count = -1;   // len_type_id == 1 -> 11 bits are a number that represents the number of sub-packets immediately contained by this packet.

        // Data
        string sLiteralData = String.Empty;
        int nLiteralData = 0;

        // SubPackets
        List<Packet> SubPackets = new List<Packet>();

        public Packet(string input)
        {
            //PacketSize = input.Length;

            version = Convert.ToInt32(input.Substring(0, 3), 2);
            type_id = Convert.ToInt32(input.Substring(3, 3), 2);


            Console.WriteLine("{");
            Console.WriteLine("  Ver : {0}", version);
            Console.WriteLine("  Type: {0}", type_id);

            if (type_id == 4) // Literal packet
            {
                PacketSize = 6;
                sLiteralData = input.Substring(6);
                for(int i = 0; i < sLiteralData.Length; i+=5)
                {
                    nLiteralData = 0;
                    PacketSize += 5;
                    int nSizeOfSubstring = (sLiteralData.Length - i) >= 5 ? 5 : sLiteralData.Length - i;

                    string s = sLiteralData.Substring(i, nSizeOfSubstring);
                    if (s.Length != 5)
                        break;

                    nLiteralData = nLiteralData * 16 + Convert.ToInt32(s.Substring(1,4), 2);

                    Console.WriteLine("  nLiteralData: {0}", nLiteralData);
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
                    PacketSize = 22;
                    int n = 0;
                    Console.WriteLine("  total_len: {0}", total_len);
                    while (n < total_len)
                    {
                        Packet P = new Packet(input.Substring(n + 22));
                        SubPackets.Add(P);
                        n += P.PacketSize;
                        PacketSize += P.PacketSize;
                    }
                    //PacketSize = n;
                    //PacketSize = this.GetPacketSize();
                }
                else
                {
                    packet_count = Convert.ToInt32(input.Substring(7, 11), 2);
                    PacketSize = 18;
                    Console.WriteLine("  packet_count: {0}", packet_count);
                    int p = 0;
                    while (p < packet_count)
                    {
                        Packet P = new Packet(input.Substring(PacketSize));
                        SubPackets.Add(P);
                        PacketSize += P.GetPacketSize();
                        p++;
                    }
                    //PacketSize += this.GetPacketSize();
                }

            }
            Console.WriteLine("}");
        }

        public int GetVersionSum()
        {
            int nVersionSum = this.version;

            foreach(Packet P in SubPackets)
            {
                nVersionSum += P.GetVersionSum();
            }

            return nVersionSum;
        }

        public int GetPacketSize()
        {
            int nPacketSize = this.PacketSize;
            return nPacketSize;
            if (len_type_id == 0)
            {
                return nPacketSize;
            }

            foreach (Packet P in SubPackets)
            {
                nPacketSize += P.GetPacketSize();
            }

            return nPacketSize;
        }

    }
    public class Program
    {
        // Answers for Data_p.txt  Part 1:      Part 2: 
        static readonly string filePath = @".\..\..\..\Data_p.txt";
        static string binaryInput = string.Empty;
        public static void Main(string[] args)
        {
            Packet P;

            ParsingInputData("8A004A801A8002F478");
            P = new Packet(binaryInput);
            Console.WriteLine(" ---- Version Sum: {0} ----", P.GetVersionSum());

            ParsingInputData("620080001611562C8802118E34");
            P = new Packet(binaryInput);
            Console.WriteLine(" ---- Version Sum: {0} ----", P.GetVersionSum());

            ParsingInputData("C0015000016115A2E0802F182340");
            P = new Packet(binaryInput);
            Console.WriteLine(" ---- Version Sum: {0} ----", P.GetVersionSum());

            ParsingInputData("A0016C880162017C3686B18A3D4780");
            P = new Packet(binaryInput);
            Console.WriteLine(" ---- Version Sum: {0} ----", P.GetVersionSum());


            ParsingInputData();
            P = new Packet(binaryInput);
            Console.WriteLine(P.GetVersionSum());

            //Console.WriteLine("Part one: {0, 80:0}", s8);
            //Console.WriteLine("Part one: {0, 80:0}", r2);
        }


        private static void ParsingInputData(string sHexString = null)
        {
            string InputData = sHexString;

            if (sHexString == null)
            {
                using (StreamReader file = new(filePath))
                    InputData = file.ReadLine();
            }

            binaryInput = string.Empty;
           // Console.WriteLine("Hex Original {0}", InputData);

            for (int i = 0; i < InputData.Length; i ++)
            {
                //int nSizeOfSubstring = (InputData.Length - i) >= 2 ? 2 : 1;
                string sHex8 = InputData.Substring(i, 1);
                string sBin = Convert.ToString(Convert.ToInt64(sHex8, 16), 2).PadLeft(4, '0');
                binaryInput += sBin;

               // Console.WriteLine("Hex {0, 10:0}    Bin {1, 80:0}", sHex8, sBin);
            }


            //Console.WriteLine("Bin result {0}", binaryInput);

        }
    }
}
