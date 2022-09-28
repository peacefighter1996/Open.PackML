using Open.PackML.Tags;
using Open.PackML.Tags.Attributes;
using System;
using System.Linq;

namespace Open.PackMLTests.Prefab
{
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
}