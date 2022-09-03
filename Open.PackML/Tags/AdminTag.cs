using System;

namespace Open.PackML.Tags
{
    public class AdminTag<T> : DataTag<T> where T : IComparable
    {
        public AdminTag(DataTagConfig dataTagConfig) : base(dataTagConfig)
        {
        }

        public override TagType TagType => TagType.Admin;
    }
}
