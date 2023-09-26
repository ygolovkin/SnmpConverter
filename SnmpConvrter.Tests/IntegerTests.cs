namespace SnmpConverter.Tests
{
    public class IntegerTests
    {
        [Theory]
        [InlineData(123, new byte[] { 0x02, 0x01, 0x7b } )]
        [InlineData(-653, new byte[] { 0x02, 0x02, 0xfd, 0x73 })]
        [InlineData(0, new byte[] { 0x02, 0x01, 0x0 })]
        public void ConvertIntegerToArrayTheory(int input, byte[] expected)
        {
            var actual = input.ToIntArray();

            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(new byte[] { 0x02, 0x01, 0x7b }, 123)]
        [InlineData(new byte[] { 0x02, 0x02, 0xfd, 0x73 }, -653)]
        [InlineData(new byte[] { 0x02, 0x01, 0x0 }, 0)]
        public void ConvertIntegerToNumberTheory(byte[] input, int expected)
        {
            var offset = 0;
            var actual = input.ToInt(ref offset);

            Assert.Equal(input.Length, offset);
            Assert.Equal(expected, actual);
        }
    }
}