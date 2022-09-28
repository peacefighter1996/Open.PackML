using Open.PackML;
using Open.PackML.Tags;
using Open.PackML.Tags.Attributes;
using Open.PackMLTests.Prefab;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Open.PackMLTests.TestObjects
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
}