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

namespace Open.PackMLTests.Prefab
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
        public int[] IntegerArray1 { get; } = new int[3];
        public int[] IntegerArray2 { get; set; } = new int[4];

        public List<int> IntegerList { get; set; } = new List<int>() { 1, 2, 3, 4 };
        [TagFixedSize(5)]
        public TestObject2[] ObjectArray { get; } = new TestObject2[5];


        public ExecuteObject[] ExecuteObjects { get; } = new ExecuteObject[]
        {
            new ExecuteObject(), new ExecuteObject(), new ExecuteObject(), new ExecuteObject()
        };
        [TagType(TagType.Command)]
        public void SetValue(int value1)
        {
            Integer1 = value1;
        }
    }
    internal class ExecuteObject
    {
        public int Integer1 { get; private set; }
        [TagType(TagType.Command)]
        public void SetValue(int value1)
        {
            Integer1 = value1;
        }
        [TagType(TagType.Command)]
        public int PlusOne(int value1)
        {
            return value1 + 1;
        }
    }
    internal class TestObject2
    {
        public int Integer1 { get; }
        public PmlState PmlState { get; }

        public List<ExecuteObject> ExecuteObjects { get; set; } = new List<ExecuteObject>()
        {
            new ExecuteObject(),
            new ExecuteObject(),
            new ExecuteObject()
        };
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
            var temp = TagTreeBuilder.GetTree("", new TestObject1());
            var table = temp.BuildTable();
            logger.WriteLine(table.GetTagTablePrint());
        }

        [Fact]
        public void GetTreeIecTest()
        {
            var temp = TagTreeBuilder.GetTree("", new TestObject1(), Iec:true);
            var table = temp.BuildTable();
            logger.WriteLine(table.GetTagTablePrint());
        }

        [Fact]
        public void GetTreeTest1()
        {
            var temp1 = TagTreeBuilder.GetTree("Machine1",new TestObject1(), Iec:true);
            var temp2 = TagTreeBuilder.GetTree("Machine2", new TestObject2(), Iec: true);
            var table = TagTableBuilder.BuildTable(new TagDetail[2] { temp1, temp2 });
            logger.WriteLine(table.GetTagTablePrint());
            
        }







        
        [Fact]
        public void GetTreeTestTr88()
        {
            var moqController = new Mock<IPmlController<Enum>>();
            var eventStore = new EventStore();
            var temp = TagTreeBuilder.GetTree("", new PmlTr88Controller<Enum>(moqController.Object, eventStore));
            logger.WriteLine(temp.BuildTable().GetTagTablePrint());
        }
        [Fact]
        public void GetTreeTestTr88Filtered()
        {
            var moqController = new Mock<IPmlController<Enum>>();
            var eventStore = new EventStore();
            var temp = TagTreeBuilder.GetTree("", new PmlTr88Controller<Enum>(moqController.Object,eventStore));
            logger.WriteLine(temp.BuildTable().GetTagTablePrint(true));
        }

        [Fact]
        public void GetTreeTestEumFiltered()
        {
            var moqController = new Mock<IPmlController<Enum>>();
            var eventStore = new EventStore();
            var temp = TagTreeBuilder.GetTree("", new PmlTr88Controller<Enum>(moqController.Object, eventStore));
            logger.WriteLine(temp.BuildTable().GetTagTablePrint(true));
        }

        
    }
}