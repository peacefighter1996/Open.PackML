using System;
using System.Linq;
using System.Reflection;

namespace Open.PackML.Tags
{
    public class ArrayTagDetail : TagDetail
    {
        public ArrayTagDetail(TagConfig config, object baseObject, TagDetail[] childTags, int length, PropertyInfo[] propertyInfos) : base(config, baseObject, childTags, propertyInfos)
        {
            Length = length;
        }
        public bool FixedSize { get => !Writable; }
        public int Length { get; }

    }
}