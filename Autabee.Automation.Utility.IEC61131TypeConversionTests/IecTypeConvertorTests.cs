using Xunit;
using Autabee.Automation.Utility.IEC61131TypeConversion;
using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Linq;

namespace Autabee.Automation.Utility.IEC61131TypeConversion.Tests
{
    public class TestObject
    {
        public int[] Warnings = new int[3];
    }

    public class IecTypeConvertorTests
    {
        [Theory]
        [InlineData(1, "BOOL")]
        [InlineData(8, "BYTE")]
        [InlineData(16, "WORD")]
        [InlineData(32, "DWORD")]
        [InlineData(64, "LWORD")]
        public void GetIecBitArraysTest(int i, string expected)
        {

            Assert.Equal(expected, IecTypeConvertor.GetIecType(new BitArray(i, false)));

        }
        [Theory]
        [InlineData(new int[] {1, 8, 16, 32, 64})]
        public void GetIecBitArrayTest(int[] i)
        {
            for (var j = 0; j < 100; j++)
            {
                if (i.Contains(j))
                {

                    Assert.NotEqual("BOOL[]", IEC61131TypeConversion.IecTypeConvertor.GetIecType(new BitArray(j, false)));
                }
                else
                {
                    Assert.Equal("BOOL[]", IEC61131TypeConversion.IecTypeConvertor.GetIecType(new BitArray(j, false)));
                }
            }

        }
        [Fact()]
        public void GetIecByteArrayTest()
        {

            Assert.Equal("STRING", IEC61131TypeConversion.IecTypeConvertor.GetIecType(new byte[3]));

        }


        [Fact()]
        public void GetIecArrayTest()
        {

            Assert.Equal("DINT[]", IEC61131TypeConversion.IecTypeConvertor.GetIecType(new int[3]));

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
            Assert.Equal(expectedResult, IecTypeConvertor.GetIecType(obj));
        }
    }
}