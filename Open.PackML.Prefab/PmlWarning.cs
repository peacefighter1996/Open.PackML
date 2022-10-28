using Open.PackML.Tags.Attributes;
using System;

namespace Open.PackML.Prefab
{
    public class PmlWarning
    {
        [TagEndUserTerm("Trigger")]
        public bool Trigger { get; set; }
        [TagEndUserTerm("ID")]
        public int ID { get; set; }
        [TagEndUserTerm("Value")]
        public int Value { get; set; }
    }
}
