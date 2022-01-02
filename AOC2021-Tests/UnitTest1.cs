using System;
using NUnit.Framework;
using BITS_Decoder;

namespace AOC2021_Tests
{
    public class Tests
    {
        [Test]
        [TestCase("8A004A801A8002F478",             ExpectedResult = 16)]
        [TestCase("620080001611562C8802118E34",     ExpectedResult = 12)]
        [TestCase("C0015000016115A2E0802F182340",   ExpectedResult = 23)]
        [TestCase("A0016C880162017C3686B18A3D4780", ExpectedResult = 31)]
        [TestCase("A0016C880162017C3686B18A3D4780", ExpectedResult = 31)]

        public static int BITS_Version_Test(String testcase)
        {
            // Arrange
            string binaryInput = Program.ParsingInputData(testcase);

            // Act
            Packet P = new Packet(binaryInput);
            var result = P.GetVersionSum();

            // Assert
            return result;
        }

        [Test]
        [TestCase("C200B40A82",                 ExpectedResult =  3)]
        [TestCase("04005AC33890",               ExpectedResult = 54)]
        [TestCase("880086C3E88112",             ExpectedResult =  7)]
        [TestCase("CE00C43D881120",             ExpectedResult =  9)]
        [TestCase("D8005AC2A8F0",               ExpectedResult =  1)]
        [TestCase("F600BC2D8F",                 ExpectedResult =  0)]
        [TestCase("9C005AC2F8F0",               ExpectedResult =  0)]
        [TestCase("9C0141080250320F1802104A08", ExpectedResult =  1)]
        public static long BITS_Processing_Test(String testcase)
        {
            // Arrange
            string binaryInput = Program.ParsingInputData(testcase);

            // Act
            Packet P = new Packet(binaryInput);
            var result = P.ProcessPacket();

            // Assert
            return result;
        }
    }
}