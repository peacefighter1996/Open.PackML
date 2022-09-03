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
        public static TagDetails GetTree(string root, object obj, string description = "", string endUserTerm= "")
        {
            TagConfig conf = new TagConfig()
            {
                TagName = root,
                DataType = obj.GetType(),
                TagType = TagType.Undefined,
                Description = description,
                EndUserTerm = endUserTerm
            };

            return GetTree(conf, obj, false, true, new List<Type>());
        }

        private static TagDetails GetTree(TagConfig root, object obj, bool writable, bool readable, List<Type> types)
        {
            var properties = obj.GetType().GetProperties();
            List<TagDetails> Children = new List<TagDetails>();
            foreach (var property in properties)
            {
                if (property.GetCustomAttribute(typeof(TagIgnoreAttribute)) != null)
                {
                    continue;
                }
                var config = GetPropertyTagConfig(property, root.TagName);

                if (config.DataType.IsArray
                    && config.DataType != typeof(string))
                {
                    types.Add(config.DataType);



                    var list = property.GetValue(obj) as Array;
                    var tempvalue = Activator.CreateInstance(config.DataType.GetElementType());
                    int lenght = -1;
                    string arrayRoot = string.IsNullOrWhiteSpace(root.TagName) ? root.TagName : root.TagName + '.';

                    if (!property.CanWrite)
                    {
                        config.TagName = arrayRoot + property.Name + $"[0..{list.Length}]";
                        lenght = list.Length;
                    }
                    else
                    {
                        //ArrayChildren.Add(new TagDetails(config.DataType.BaseType, new TagDetails[0], root + '.' + property.Name + ".Lenght", false, true));
                        config.TagName = arrayRoot + property.Name + $"[#]";
                    }
                    var tree = GetTree(config,
                       tempvalue,
                    property.CanWrite,
                    property.CanRead,
                    types);

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
                        property.GetValue(obj),
                        property.CanWrite,
                        property.CanRead,
                        types));
                    types.Remove(config.DataType);
                }
                else
                {
                    Children.Add(new TagDetails(config, new TagDetails[0],  property.CanWrite, property.CanRead));
                }
            }
            
            return new TagDetails(root, Children.ToArray(), writable, readable);
        }

        private static TagConfig GetPropertyTagConfig(PropertyInfo property, string root)
        {
            TagConfig config = new TagConfig()
            {

                DataType = property.PropertyType,
                TagName = string.IsNullOrWhiteSpace(root) ? property.Name : root + '.' + property.Name,
                EndUserTerm = property.GetCustomAttribute<TagEndUserTermAttribute>() is TagEndUserTermAttribute e ? e.EndUserTerm : string.Empty,

                TagType = property.GetCustomAttribute<TagTypeAttribute>() is TagTypeAttribute p ? p.Type : TagType.Status,
                Description = property.GetCustomAttribute<DescriptionAttribute>() is DescriptionAttribute description
                ? description.Description
                : string.Empty
            };
       

            return config;
        }
    }
    public class TagTable : Collection<TagDetails>
    {
        public string GetTagTablePrint()
        {
            string print = "";
            foreach (TagConfig item in this) print += item.ToString() + Environment.NewLine;
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
                BuildTable(tagtree,table);
        }
    }
}
