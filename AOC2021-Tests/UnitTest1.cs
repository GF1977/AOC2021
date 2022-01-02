using System;
using NUnit.Framework;
using BITS_Decoder;

namespace AOC2021_Tests
{
    public class Tests
    {
        [Test]
        [TestCase("8A004A801A8002F478", ExpectedResult = 16)]
        [TestCase("620080001611562C8802118E34", ExpectedResult = 12)]
        [TestCase("C0015000016115A2E0802F182340", ExpectedResult = 23)]
        [TestCase("A0016C880162017C3686B18A3D4780", ExpectedResult = 31)]
        [TestCase("A0016C880162017C3686B18A3D4780", ExpectedResult = 31)]

        public static int BITS_Test(String testcase)
        {
            // Arrange
            string binaryInput = Program.ParsingInputData(testcase);

            // Act
            Packet P = new Packet(binaryInput);
            var result = P.GetVersionSum();

            // Assert
            return result;
        }
    }
}