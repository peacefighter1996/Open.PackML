using Open.PackML.Tags;
using Open.PackML.Tags.Attributes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;

namespace Open.PackML.Tags.Builders
{
    public static class BuildTagTree
    {
        public static TagDetails GetTree(string root, object obj)
        {
            return GetTree(root, obj, false, true, new());
        }

        private static TagDetails GetTree(string root, object obj, bool writable, bool readable, List<Type> types)
        {
            var properties = obj.GetType().GetProperties();
            List<TagDetails> Children = new();
            foreach (var property in properties)
            {
                if (property.GetCustomAttribute(typeof(TagIgnoreAttribute)) != null)
                {
                    continue;
                }
                Type propertyType = property.PropertyType;
                TagTypeAttribute p = property.GetCustomAttribute<TagTypeAttribute>();
                TagType type = TagType.Undefined;

                if (p != null)
                {
                    type = p.type;
                }

                if (propertyType.IsArray
                    && propertyType != typeof(string))
                {
                    types.Add(propertyType);


                    List<TagDetails> ArrayChildren = new();
                    var list = property.GetValue(obj) as Array;
                    var tempvalue = Activator.CreateInstance(propertyType.GetElementType());
                    string temproot;
                    int lenght = -1;
                    if (!property.CanWrite)
                    {
                        temproot = root + '.' + property.Name + $"[0..{list.Length}]";
                        lenght = list.Length;
                    }
                    else
                    {
                        ArrayChildren.Add(new TagDetails(propertyType.BaseType, new TagDetails[0], root + '.' + property.Name + ".Lenght", false, true));
                        temproot = root + '.' + property.Name + $"[#]";
                    }
                    ArrayChildren.Add(GetTree(temproot,
                       tempvalue,
                    property.CanWrite,
                    property.CanRead,
                    types));

                    Children.Add(new ArrayTagDetail(
                        propertyType,
                        ArrayChildren.ToArray(),
                        temproot,
                        property.CanWrite,
                        property.CanRead,
                        lenght
                        ));

                    types.Remove(propertyType);
                }
                else if (propertyType.GetInterfaces().Contains(typeof(IEnumerable))
                    && propertyType != typeof(string))
                {

                }
                else if ((propertyType.IsInterface
                || propertyType.IsClass
                || propertyType.IsValueType)
                && !propertyType.IsPrimitive
                && propertyType != typeof(string)
                && propertyType != typeof(DateTime)
                && propertyType != typeof(TimeSpan)
                && !propertyType.IsEnum
                && (
                    types.Count == 0
                    || !types.Contains(propertyType)
                    )
                 )
                {
                    types.Add(propertyType);
                    Children.Add(GetTree(root + '.' + property.Name,
                        property.GetValue(obj),
                        property.CanWrite,
                        property.CanRead,
                        types));
                    types.Remove(propertyType);
                }
                else
                {
                    var address = root + '.' + property.Name;
                    Children.Add(new TagDetails(propertyType, new TagDetails[0], address, property.CanWrite, property.CanRead));
                }
            }

            return new TagDetails(obj.GetType(), Children.ToArray(), root, writable, readable);
        }
    }
    public class TagTable : Collection<TagConfig>
    {

    }

    public static class TagTableBuilder
    {
        public static TagTable BuildTable(this TagDetails tagDetails)
        {
            return new TagTable();
        }
    }
}
