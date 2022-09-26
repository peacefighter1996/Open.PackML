using System;
using System.Linq;
using System.Reflection;

namespace Open.PackML.Tags
{
    public class ArrayTagDetail : TagDetail
    {
        public ArrayTagDetail(
            TagConfig config,
            object baseObject,
            TagDetail[] childTags,
            int length,
            MemberInfo[] propertyInfos,
            bool[] arrayType) : base(config, baseObject, childTags, propertyInfos, arrayType)
        { Length = length; }

        public int Length { get; }
    }
}