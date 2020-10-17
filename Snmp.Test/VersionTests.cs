using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Snmp.Model.Enums;
using Snmp.Serializer.ParsingExtensions;

namespace Snmp.Test
{
    [TestClass]
    public class VersionTests
    {

        [TestMethod]
        public void TestVersion1()
        {
            var expected = new byte[] {2, 1, 0};
            var actual = SnmpVersion.v1.VersionEncode().ToArray();

            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestVersion2()
        {
            var expected = new byte[] { 2, 1, 1 };
            var actual = SnmpVersion.v2c.VersionEncode().ToArray();

            CollectionAssert.AreEqual(expected, actual);
        }


        [TestMethod]
        public void TestVersion3()
        {
            var expected = new byte[] { 2, 1, 3 };
            var actual = SnmpVersion.v3.VersionEncode().ToArray();

            CollectionAssert.AreEqual(expected, actual);
        }

    }
}
