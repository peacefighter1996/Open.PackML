using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Open.PackMLTests")]
namespace Open.PackML.Tags.Builders
{
    internal class TagTreeBuilderProcessData
    {
        internal readonly object baseObject;
        internal readonly bool iec;

        internal List<Type> TypesChain { get; } = new List<Type>();
        internal List<MemberInfo> PropertyChain { get; } = new List<MemberInfo>();
        internal List<bool> ArrayChain { get; } = new List<bool>();

        internal TagTreeBuilderProcessData(bool Iec, object BaseObject)
        {
            baseObject = BaseObject;
            iec = Iec;
        }
    }
}
