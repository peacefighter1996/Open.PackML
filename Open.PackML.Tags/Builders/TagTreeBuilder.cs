
using Open.PackML.Tags.Attributes;
using Open.PackML.Tags.IEC;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Open.PackML.Tags.Builders
{
    public static class TagTreeBuilder
    {
        /// <summary>
        /// Builds a TagTree from a base object
        /// </summary>
        /// <param name="root">Root object name</param>
        /// <param name="obj">Object for which the tree needs to be build</param>
        /// <param name="description"></param>
        /// <param name="endUserTerm"></param>
        /// <param name="StartTagType"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static TagDetail GetTree(string root, object obj, string description = "", string endUserTerm = "", TagType StartTagType = TagType.Undefined)
        {
            Type type = obj.GetType();
            if ((type.IsArray
                    || type.GetInterfaces().Contains(typeof(IEnumerable)))) 
                throw new ArgumentException("Cannot create a tag tree from an array or IEnumerable");

            TagConfig conf = new TagConfig()
            {
                TagName = root,
                DataType = type,
                TagType = TagType.Undefined,
                Description = description,
                EndUserTerm = endUserTerm
            };

            return GetTree(conf, type, obj, new List<Type>() { type }, TagType.Undefined, new List<MemberInfo>(), new List<bool>() { });
        }

        private static TagDetail GetTree(TagConfig root,
            Type objType,
            object baseObject,
            List<Type> types,
            TagType TagTypeCarry,
            List<MemberInfo> propertyChain,
            List<bool> arraytree)
        {
            List<TagDetail> Children = new List<TagDetail>();
            foreach (var property in objType.GetProperties())
            {
                if (property.GetCustomAttribute(typeof(TagIgnoreAttribute)) != null) continue;

                if (!(property.CanRead || property.CanWrite)) continue;

                var config = GetPropertyTagConfig(property, root.TagName, TagTypeCarry);
                propertyChain.Add(property);
                if ((config.DataType.IsArray
                    || config.DataType.GetInterfaces().Contains(typeof(IEnumerable)))
                    && config.DataType != typeof(string)
                    && !types.Contains(config.DataType))
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
                    types.Add(config.DataType);
                    arraytree.Add(true);
                    var tree = GetTree(config,
                        elementType,
                        baseObject,
                        types,
                        config.TagType,
                        propertyChain,
                        arraytree
                        );

                    Children.Add(new ArrayTagDetail(
                        config,
                        baseObject,
                        tree.ChildTags.ToArray(),
                        lenght,
                        propertyChain.ToArray(),
                        arraytree.ToArray()));
                    arraytree.RemoveAt(arraytree.Count - 1);
                    types.Remove(config.DataType);
                }
                else if ((config.DataType.IsInterface
                || config.DataType.IsClass
                || config.DataType.IsValueType)
                && !config.DataType.IsPrimitive
                && config.DataType != typeof(string)
                && config.DataType != typeof(DateTime)
                && config.DataType != typeof(TimeSpan)
                && !config.DataType.IsEnum
                && !types.Contains(config.DataType) // prevent circular references
                 )
                {
                    types.Add(config.DataType);
                    arraytree.Add(false);
                    Children.Add(GetTree(config,
                        property.PropertyType,
                        baseObject,
                        types,
                        config.TagType,
                        propertyChain,
                        arraytree
                        ));
                    arraytree.RemoveAt(arraytree.Count - 1);
                    types.Remove(config.DataType);
                }
                else
                {
                    arraytree.Add(false);
                    Children.Add(new TagDetail(config, baseObject, new TagDetail[0], propertyChain.ToArray(),arraytree.ToArray()));
                    arraytree.RemoveAt(arraytree.Count - 1);
                }
                propertyChain.RemoveAt(propertyChain.Count - 1);
            }
            foreach (var method in objType.GetMethods())
            {
                arraytree.Add(false);
                if (method.GetCustomAttribute(typeof(TagTypeAttribute)) is TagTypeAttribute attribute)
                {
                    var config = GetPropertyTagConfig(method, root.TagName, attribute.TagType);
                    propertyChain.Add(method);
                    Children.Add(new TagDetail(config, baseObject, new TagDetail[0], propertyChain.ToArray(), arraytree.ToArray()));
                    propertyChain.Remove(method);
                }
                arraytree.RemoveAt(arraytree.Count - 1);
            }


            return new TagDetail(root, baseObject, Children.ToArray(), propertyChain.ToArray(), arraytree.ToArray());
        }

        private static TagConfig GetPropertyTagConfig(PropertyInfo property, string root, TagType TagCarry)
        {
            TagConfig config = new TagConfig()
            {
                DataType = property.PropertyType,
                TagName = string.IsNullOrWhiteSpace(root) ? property.Name : root + '.' + property.Name,
                EndUserTerm = property.GetCustomAttribute<TagEndUserTermAttribute>() is TagEndUserTermAttribute e ? e.EndUserTerm : string.Empty,

                TagType = property.GetCustomAttribute<TagTypeAttribute>() is TagTypeAttribute p ? p.TagType : TagCarry,
                Description = property.GetCustomAttribute<DescriptionAttribute>() is DescriptionAttribute description
                ? description.Description
                : string.Empty
            };


            return config;
        }

        private static TagConfig GetPropertyTagConfig(MethodInfo property, string root, TagType TagCarry)
        {
            var value = property.GetParameters().Aggregate("", (accumulator, item) => accumulator += item.ParameterType.GetIecTypeString() + " " + item.Name + ",");
            TagConfig config = new TagConfig()
            {
                DataType = property.ReturnType,
                TagName = (string.IsNullOrWhiteSpace(root) ? property.Name : root + '.' + property.Name) + "(" + value.Substring(0, value.Length - 1) + ")",
                EndUserTerm = property.GetCustomAttribute<TagEndUserTermAttribute>() is TagEndUserTermAttribute e ? e.EndUserTerm : string.Empty,

                TagType = property.GetCustomAttribute<TagTypeAttribute>() is TagTypeAttribute p ? p.TagType : TagCarry,
                Description = property.GetCustomAttribute<DescriptionAttribute>() is DescriptionAttribute description
                ? description.Description
                : string.Empty
            };


            return config;
        }
    }
}
