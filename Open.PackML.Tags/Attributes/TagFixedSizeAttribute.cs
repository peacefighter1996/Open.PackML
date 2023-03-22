using System;

namespace Open.PackML.Tags.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class TagFixedSizeAttribute : Attribute
    {
        public int Size { get; }
        public TagFixedSizeAttribute(int size)
        {
            this.Size = size;
        }
    }

    //public class TagFixedInstanceAttribute : Attribute
    //{
    //    public bool Value { get; }
    //    public TagFixedInstanceAttribute(bool value)
    //    {
    //        this.Value = value;
    //    }
    //}
}