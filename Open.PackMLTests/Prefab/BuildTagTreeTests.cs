using Moq;
using Open.PackML;
using Open.PackML.Prefab;
using Open.PackML.Tags;
using Open.PackML.Tags.Builders;
using Open.PackMLTests.TestObjects;
using System;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace Open.PackMLTests.Prefab
{
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
            var temp = TagTreeBuilder.GetTree(string.Empty, new TestObject1());
            var table = temp.BuildTable();
            logger.WriteLine(table.GetTagTablePrint());
        }

        [Fact]
        public void GetTreeIecTest()
        {
            var temp = TagTreeBuilder.GetTree(string.Empty, new TestObject1(), Iec: true);
            var table = temp.BuildTable();
            logger.WriteLine(table.GetTagTablePrint());
        }

        [Fact]
        public void GetTreeTest1()
        {
            var temp1 = TagTreeBuilder.GetTree("Machine1", new TestObject1(), Iec: true);
            var temp2 = TagTreeBuilder.GetTree("Machine2", new TestObject2(), Iec: true);
            var table = TagTableBuilder.BuildTable(new TagDetail[2] { temp1, temp2 });
            logger.WriteLine(table.GetTagTablePrint());

        }


        [Fact]
        public void GetTreeTestTr88()
        {
            var moqController = new Mock<IPmlController>();
            var eventStore = new PmlEventStore();
            var temp = TagTreeBuilder.GetTree(string.Empty, new PmlTr88Controller(moqController.Object, eventStore, new PmlOemTransitionCheck()));
            logger.WriteLine(temp.BuildTable().GetTagTablePrint());
        }

        [Fact]
        public void GetTreeTestTr88Filtered()
        {
            var moqController = new Mock<IPmlController>();
            var eventStore = new PmlEventStore();
            var temp = TagTreeBuilder.GetTree(string.Empty, new PmlTr88Controller(moqController.Object, eventStore, new PmlOemTransitionCheck()));
            logger.WriteLine(temp.BuildTable().GetTagTablePrint(true));
        }

        [Fact]
        public void GetTreeTestEumFiltered()
        {
            var moqController = new Mock<IPmlController>();
            var eventStore = new PmlEventStore();
            var temp = TagTreeBuilder.GetTree(string.Empty, new PmlEumController(moqController.Object, eventStore, new PmlOemTransitionCheck()));
            logger.WriteLine(temp.BuildTable().GetTagTablePrint(true));
        }


    }
}