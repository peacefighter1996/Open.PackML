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
using Moq;
using Open.PackML.Interfaces;
using Open.PackML;
using Open.PackML.Prefab;
using Open.PackMLTests.TestObjects;
using Open.PackML.Tags.Prefab;

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
            var temp = TagTreeBuilder.GetTree("", new TestObject1());
            var table = temp.BuildTable();
            logger.WriteLine(table.GetTagTablePrint());
        }

        [Fact]
        public void GetTreeIecTest()
        {
            var temp = TagTreeBuilder.GetTree("", new TestObject1(), Iec: true);
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
            var temp = TagTreeBuilder.GetTree("", new PmlTr88Controller(moqController.Object, eventStore, new PmlOemTransitionCheck()));
            logger.WriteLine(temp.BuildTable().GetTagTablePrint());
        }

        [Fact]
        public void GetTreeTestTr88Filtered()
        {
            var moqController = new Mock<IPmlController>();
            var eventStore = new PmlEventStore();
            var temp = TagTreeBuilder.GetTree("", new PmlTr88Controller(moqController.Object, eventStore, new PmlOemTransitionCheck()));
            logger.WriteLine(temp.BuildTable().GetTagTablePrint(true));
        }

        [Fact]
        public void GetTreeTestEumFiltered()
        {
            var moqController = new Mock<IPmlController>();
            var eventStore = new PmlEventStore();
            var temp = TagTreeBuilder.GetTree("", new PmlEumController(moqController.Object, eventStore, new PmlOemTransitionCheck()));
            logger.WriteLine(temp.BuildTable().GetTagTablePrint(true));
        }


    }
}