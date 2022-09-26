using Open.PackML.Tags;
using Open.PackML.Tags.Attributes;
using System;
using System.Linq;

namespace Open.PackML.Prefab
{
    public class PmlStopReason
    {
        public PmlStopReason() { }
        public PmlStopReason(int id, int value)
        {
            ID = id;
            Value = value;
        }

        [TagEndUserTerm("Event and stop reason")]
        [TagType(TagType.Admin)]
        public int ID { get; set; }

        [TagEndUserTerm("Detailed Error Information")]
        [TagType(TagType.Admin)]
        public int Value { get; set; }
    }
}
