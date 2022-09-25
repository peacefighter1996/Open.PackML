using Xunit;
using Open.PackML.Tags;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Open.PackMLTests
{
    public class TagConfigTests
    {
        [Theory]
        [InlineData("Machine1.ObjectArray.Integer1", "Machine1.ObjectArray[0..5].Integer1")]
        [InlineData("Machine1.IntegerArray2", "Machine1.IntegerArray2[#]")]
        public void TagStringToSearchTest(string expected, string tagString)
        {
            Assert.Equal(expected, TagConfig.TagStringToSearch(tagString));
        }

    }
}