using System;

namespace Open.PackML.Tags
{
    public class StatusTag<T> : DataTag<T> where T : IComparable
    {
        public StatusTag(DataTagConfig dataTagConfig) : base(dataTagConfig)
        {
        }

        public override TagType TagType => TagType.Status;
    }
}
