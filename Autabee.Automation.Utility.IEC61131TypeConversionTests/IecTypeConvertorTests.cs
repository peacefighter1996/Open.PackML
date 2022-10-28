using Xunit;
using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Linq;
using Xunit.Abstractions;
using Microsoft.VisualStudio.TestTools.UnitTesting.Logging;
using System.Net.Sockets;
using System.Windows.Markup;
using Autabee.Utility.IEC61131TypeConversion;
using Autabee.Utility.IEC61131TypeConversionTests.TestObjects;

namespace Autabee.Utility.IEC61131TypeConversionTests
{
    public class IecTypeConvertorTests
    {
        ITestOutputHelper logger;
        public IecTypeConvertorTests(ITestOutputHelper output)
        {
            logger = output;
        }

        //[Theory]
        //[InlineData(1, "BOOL")]
        //[InlineData(8, "BYTE")]
        //[InlineData(16, "WORD")]
        //[InlineData(32, "DWORD")]
        //[InlineData(64, "LWORD")]
        //public void GetIecBitArraysTest(int i, string expected)
        //{

        //    Assert.Equal(expected, IecTypeConvertor.GetIecTypeString(new BitArray(i, false)));

        //}
        [Fact]
        public void GetIecBitArrayTest()
        {
            for (var j = 0; j < 100; j++)
            {
                Assert.Equal($"BOOL[]", IecTypeConvertor.GetIecTypeString(new BitArray(j, false)));
            }

        }
        [Fact()]
        public void GetIecByteArrayTest()
        {
            Assert.Equal("STRING[]", IecTypeConvertor.GetIecTypeString(new char[3]));

        }




        [Fact()]
        public void GetIecArrayTest()
        {

            Assert.Equal("DINT[]", IecTypeConvertor.GetIecTypeString(new int[3]));

        }

        [Theory]
        [InlineData(false, "BOOL")]
        [InlineData(true, "BOOL")]
        [InlineData((byte.MaxValue), "USINT")]
        [InlineData((ushort.MaxValue), "UINT")]
        [InlineData((uint.MaxValue), "UDINT")]
        [InlineData((ulong.MaxValue), "ULINT")]

        public void GetIecTypeTest<T>(T obj, string expectedResult)
        {
            Assert.Equal(expectedResult, IecTypeConvertor.GetIecTypeString(obj));
        }

        [Fact()]
        public void GetIecObjectTypeTest()
        {
            Guid guid = new Guid();
            var person = new Person() { Name = guid.ToString(), PasswordHash = guid.GetHashCode() };
            Assert.Equal("UDT_Autabee_Utility_IEC61131TypeConversionTests_TestObjects_Person", person.GetIecTypeString());
        }


        [Fact()]
        public void GetIecObjectTypeStructureTest()
        {
            Guid guid = new Guid();
            var person = new Person() { Name = guid.ToString(), PasswordHash = guid.GetHashCode() };
            logger.WriteLine(person.GetIecTypeStructure(5).OuterXml);
        }
    }


}