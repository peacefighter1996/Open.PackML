using Xunit;
using Open.PackML.Prefab;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Open.PackML.Prefab.Tests
{
    internal class TestObject1
    {
        public TestObject1()
        {
        } 

        public int Integer1 { get; set; }
        public PmlState PmlState { get; set; }
        public int[] IntegerArray { get; } = new int[3];
        public int[] IntegerArray2 { get; set; } = new int[4];
        public TestObject2[] ObjectArray { get; } = new TestObject2[5];
    }

    internal class TestObject2
    {
        public int Integer1 { get; }
        public PmlState PmlState { get; }
    }

    public class BuildTagTreeTests
    {
        [Fact()]
        public void GetTreeTest()
        {
            var temp = BuildTagTree.GetTree("Test1", new TestObject1());
        }
    }
}