using Open.PackML;
using Open.PackMLTests.Prefab;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Open.PackMLTests.TestObjects
{
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
}