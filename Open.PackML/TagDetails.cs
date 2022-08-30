using System;

namespace Open.PackML
{
    public class TagDetails
    {
        public TagDetails(Type dataType, TagDetails[] childTags, string tagNodeAddress, bool writable, bool readable)
        {
            DataType = dataType;
            ChildTags = childTags;
            TagNodeAddress = tagNodeAddress;
            Writable = writable;
            Readable = readable;
        }
        public Type DataType { get; }
        public TagDetails[] ChildTags { get; }
        public string TagNodeAddress { get; }
        public string[] TagNodeAddressComponents { get => TagNodeAddress.Split('.'); }
        public bool Writable { get; }
        public bool Readable { get; }
    }

    public class ArrayTagDetail : TagDetails
    {
        public ArrayTagDetail(Type dataType, TagDetails[] childTags, string tagNodeAddress, bool writable, bool readable, int lenght) : base(dataType, childTags, tagNodeAddress, writable, readable)
        {
            Lenght = lenght;
        }
        public bool FixedSize { get => !Writable; }
        public int Lenght { get; }
    }

    public class TagDetailBuilder
    {
        private Type dataType;

        TagDetailBuilder DataType(Type type)
        {
            this.dataType = type;
            return this;
        }
    }
}