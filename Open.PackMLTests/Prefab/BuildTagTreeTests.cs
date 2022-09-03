using Open.PackML.Tags;
using Xunit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;
using Open.PackML.Tags.Builders;
using Open.PackML.Tags.Attributes;

namespace Open.PackML.Prefab.Tests
{
    internal class TestObject1
    {
        public TestObject1()
        {
        }
        [TagType(TagType.Admin)]
        public int Integer1 { get; set; }
        [TagType(TagType.Command)]
        public PmlState PmlState { get; set; }
        //[ArrayTagFixed(size = true, object = true)]
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
        ITestOutputHelper logger;
        public BuildTagTreeTests(ITestOutputHelper output)
        {
            logger = output;
        }
        [Fact]
        public void GetTreeTest()
        {
            var temp = BuildTagTree.GetTree("", new TestObject1());

            var table = temp.BuildTable();
            table.RemoveAt(0);

            logger.WriteLine(table.GetTagTablePrint());
        }

        [Fact]
        public void GetTreeTest1()
        {
            var temp = BuildTagTree.GetTree("Machine1", new TestObject1());
            var table = temp.BuildTable();
            temp = BuildTagTree.GetTree("Machine2", new TestObject2());
            foreach (var item in temp.BuildTable()) table.Add(item);
            var temptable = new TagTable();
            foreach (var item in table) if (!item.TagName.Contains('.')) temptable.Add(item);
            foreach (var item in temptable) table.Remove(item);
            logger.WriteLine(table.GetTagTablePrint());
        }
    }
}