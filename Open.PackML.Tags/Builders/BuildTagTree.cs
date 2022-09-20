using Open.PackML.Tags.Attributes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace Open.PackML.Tags.Builders
{
    public static class BuildTagTree
    {
        public static TagDetails GetTree(string root, object obj, string description = "", string endUserTerm = "")
        {
            TagConfig conf = new TagConfig()
            {
                TagName = root,
                DataType = obj.GetType(),
                TagType = TagType.Undefined,
                Description = description,
                EndUserTerm = endUserTerm
            };

            return GetTree(conf, obj.GetType(), false, true, new List<Type>(), TagType.Undefined);
        }

        private static TagDetails GetTree(TagConfig root, Type obj, bool writable, bool readable, List<Type> types, TagType TagTypeCarry)
        {
            var properties = obj.GetProperties();
            List<TagDetails> Children = new List<TagDetails>();
            foreach (var property in properties)
            {
                if (property.GetCustomAttribute(typeof(TagIgnoreAttribute)) != null)
                {
                    continue;
                }
                var config = GetPropertyTagConfig(property, root.TagName, TagTypeCarry);

                if ((config.DataType.IsArray
                    || config.DataType.GetInterface("System.Collections.IEnumerable") != null)
                    && config.DataType != typeof(string))
                {
                    Type elementType;
                    if (config.DataType.IsArray)
                    {
                        elementType = config.DataType.GetElementType();
                    }
                    else
                    {
                        if (config.DataType.GenericTypeArguments.Count() > 0)
                            elementType = config.DataType.GenericTypeArguments[0];
                        else continue;
                    }

                    int lenght = -1;
                    string arrayRoot = string.IsNullOrWhiteSpace(root.TagName) ? root.TagName : root.TagName + '.';
                    var fixedSizeAttribute = property.GetCustomAttribute(typeof(TagFixedSizeAttribute)) as TagFixedSizeAttribute;
                    if (fixedSizeAttribute != null && fixedSizeAttribute.Size >= 0)
                    {
                        config.TagName = arrayRoot + property.Name + $"[0..{fixedSizeAttribute.Size}]";
                        lenght = fixedSizeAttribute.Size;
                    }
                    else
                    {                      
                        config.TagName = arrayRoot + property.Name + $"[#]";
                    }
                    var tree = GetTree(config,
                        elementType,
                        property.CanWrite,
                        property.CanRead,
                        types,
                        config.TagType);

                    Children.Add(new ArrayTagDetail(
                        config,
                        tree.ChildTags.ToArray(),
                        property.CanWrite,
                        property.CanRead,
                        lenght
                        ));

                    types.Remove(config.DataType);
                }
                else if (config.DataType.GetInterfaces().Contains(typeof(IEnumerable))
                    && config.DataType != typeof(string))
                {

                }
                else if ((config.DataType.IsInterface
                || config.DataType.IsClass
                || config.DataType.IsValueType)
                && !config.DataType.IsPrimitive
                && config.DataType != typeof(string)
                && config.DataType != typeof(DateTime)
                && config.DataType != typeof(TimeSpan)
                && !config.DataType.IsEnum
                && (
                    types.Count == 0
                    || !types.Contains(config.DataType)
                    )
                 )
                {
                    types.Add(config.DataType);
                    Children.Add(GetTree(config,
                        property.PropertyType,
                        property.CanWrite,
                        property.CanRead,
                        types,
                        config.TagType));
                    types.Remove(config.DataType);
                }
                else
                {
                    Children.Add(new TagDetails(config, new TagDetails[0], property.CanWrite, property.CanRead));
                }
            }

            return new TagDetails(root, Children.ToArray(), writable, readable);
        }

        private static TagConfig GetPropertyTagConfig(PropertyInfo property, string root, TagType TagCarry)
        {
            TagConfig config = new TagConfig()
            {

                DataType = property.PropertyType,
                TagName = string.IsNullOrWhiteSpace(root) ? property.Name : root + '.' + property.Name,
                EndUserTerm = property.GetCustomAttribute<TagEndUserTermAttribute>() is TagEndUserTermAttribute e ? e.EndUserTerm : string.Empty,

                TagType = property.GetCustomAttribute<TagTypeAttribute>() is TagTypeAttribute p ? p.Type : TagCarry,
                Description = property.GetCustomAttribute<DescriptionAttribute>() is DescriptionAttribute description
                ? description.Description
                : string.Empty
            };


            return config;
        }
    }
    public class TagTable : Collection<TagDetails>
    {
        public string GetTagTablePrint(bool filterUndifined = false)
        {
            string print = "";
            IEnumerable<TagDetails> collection = this;
            if (filterUndifined)
                collection = collection.Where(o => o.TagType != TagType.Undefined);
            foreach (TagConfig item in collection) print += item.ToString() + Environment.NewLine;
            return print;
        }
    }

    public static class TagTableBuilder
    {
        public static TagTable BuildTable(this TagDetails tagDetails)
        {
            var table = new TagTable();

            BuildTable(tagDetails, table);

            return table;
        }

        public static void BuildTable(this TagDetails tagDetails, TagTable table)
        {
            table.Add(tagDetails);

            foreach (TagDetails tagtree in tagDetails.ChildTags)
                BuildTable(tagtree, table);
        }
    }
}
