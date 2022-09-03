using System;

namespace Open.PackML.Tags
{
    public abstract class Tag<T> : TagConfig
    {
        public Tag(TagConfig tagConfig) : base(tagConfig)
        {
            DataType = typeof(T);
        }

    }
}
