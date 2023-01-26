
using Autabee.Utility.IEC61131StringTypeConversion;
using Open.PackML.Tags.Attributes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Open.PackMLTests")]
namespace Open.PackML.Tags.Builders
{
    internal static class TagTreeBuilder
    {
        /// <summary>
        /// This function builds a tree of tags for a given object.
        /// </summary>
        /// <param name="root">Root object name</param>
        /// <param name="obj">Object for which the tree needs to be build from</param>
        /// <param name="description">Description of the obj</param>
        /// <param name="endUserTerm">Simplyfied term for enduser for the root tag</param>
        /// <param name="StartTagType">Type of tag that is used to set the typing of the propeties that are being scanned</param>
        /// <returns>TagDetail</returns>
        /// <exception cref="ArgumentException">Does not allow a collection to be used as base object.</exception>
        internal static TagDetail GetTree(string root, object obj, string description = "", string endUserTerm = "", TagType StartTagType = TagType.Undefined, bool Iec = false)
        {
            Type type = obj.GetType();
            if (type.IsArray
                || type.GetInterfaces().Contains(typeof(IEnumerable)))
                throw new ArgumentException("Cannot create a tag tree from an array or IEnumerable");
            TagConfig conf = new TagConfig()
            {
                Name = root,
                DataType = type,
                TagType = TagType.Undefined,
                Description = description,
                EndUserTerm = endUserTerm
            };
            TagTreeBuilderProcessData tagbuilder = new TagTreeBuilderProcessData(Iec, obj);
            tagbuilder.TypesChain.Add(type);
            return GetTree(conf, type, tagbuilder, TagType.Undefined);
        }

        private static TagDetail GetTree(TagConfig root,
            Type objType,
            TagTreeBuilderProcessData tagBuilder,
            TagType TagTypeCarry)
        {
            List<TagDetail> Children = new List<TagDetail>();
            Children.AddRange(objType.GetProperties()
                .Select(o => ProcessProperty(root, tagBuilder, TagTypeCarry, o))
                .Where(o => o != null));
            foreach (var method in objType.GetMethods())
            {
                if (method.GetCustomAttribute(typeof(TagTypeAttribute)) is TagTypeAttribute attribute)
                {
                    Children.Add(ProcessMethode(root, tagBuilder, method, attribute));
                }
            }

            return new TagDetail(root, tagBuilder, Children.ToArray());
        }        

        private static TagDetail ProcessMethode(TagConfig root, TagTreeBuilderProcessData tagBuilder, MethodInfo method, TagTypeAttribute attribute)
        {
            var config = GetMethodeTagConfig(method, root.Name, attribute.TagType, tagBuilder.iec);
            tagBuilder.ArrayChain.Add(false);
            tagBuilder.PropertyChain.Add(method);
            var result = new TagDetail(config, tagBuilder, new TagDetail[0]);
            tagBuilder.PropertyChain.RemoveLast();
            tagBuilder.ArrayChain.RemoveLast();
            return result;
        }

        private static TagDetail ProcessProperty(TagConfig root, TagTreeBuilderProcessData tagBuilder, TagType TagTypeCarry, PropertyInfo property)
        {
            if (property.GetCustomAttribute(typeof(TagIgnoreAttribute)) != null) return null;

            if (!(property.CanRead || property.CanWrite)) return null;

            var config = GetPropertyTagConfig(property, root.Name, TagTypeCarry);
            tagBuilder.PropertyChain.Add(property);
            TagDetail result;
            if (config.IsCollection(tagBuilder.TypesChain))
            {
                result = ProcessCollection(root, tagBuilder, property, config);
            }
            else if (config.IsObject(tagBuilder.TypesChain))
            {
                result = ProcessObject(tagBuilder, property, config);
            }
            else
            {
                result = ProcessValue(tagBuilder, config);
            }
            tagBuilder.PropertyChain.RemoveLast();
            return result;
        }

        private static TagDetail ProcessValue(TagTreeBuilderProcessData tagBuilder, TagConfig config)
        {
            tagBuilder.ArrayChain.Add(false);
            var result = new TagDetail(config, tagBuilder, new TagDetail[0]);
            tagBuilder.ArrayChain.RemoveLast();
            return result;
        }

        private static TagDetail ProcessObject(TagTreeBuilderProcessData tagBuilder, PropertyInfo property, TagConfig config)
        {
            tagBuilder.PrepareDive(config, false);
            var result = GetTree(config,
                property.PropertyType,
                tagBuilder,
                config.TagType
                );
            tagBuilder.Surface();
            return result;
        }

        private static TagDetail ProcessCollection(TagConfig root, TagTreeBuilderProcessData tagBuilder, PropertyInfo property, TagConfig config)
        {
            Type elementType = config.GetElementType();
            (int lenght, config.Name) = GetNameAndLenght(root, property);
            tagBuilder.PrepareDive(config, true);
            var tree = GetTree(config,
                elementType,
                tagBuilder,
                config.TagType
                );

            var temp = (new TagDetail(
                config,
                tagBuilder,
                tree.ChildTags.ToArray(),
                lenght));
            tagBuilder.Surface();
            return temp;
        }

        private static void Surface(this TagTreeBuilderProcessData tagBuilder)
        {
            tagBuilder.ArrayChain.RemoveLast();
            tagBuilder.TypesChain.RemoveLast();
        }

        private static void RemoveLast<T>(this List<T> ts) => ts.RemoveAt(ts.Count - 1);


        private static void PrepareDive(this TagTreeBuilderProcessData tagBuilder, TagConfig config, bool array)
        {
            tagBuilder.TypesChain.Add(config.DataType);
            tagBuilder.ArrayChain.Add(array);
        }

        private static Type GetElementType(this TagConfig config)
        {
            Type elementType;
            if (config.DataType.IsArray)
                elementType = config.DataType.GetElementType();
            else if (config.DataType.GenericTypeArguments.Count() > 0)
                elementType = config.DataType.GenericTypeArguments[0];
            else throw new Exception($"Unhandled Elementtype of collection type {config.DataType}");
            return elementType;
        }

        private static TagConfig GetPropertyTagConfig(PropertyInfo property, string root, TagType TagCarry)
        {
            TagConfig config = new TagConfig()
            {
                DataType = property.PropertyType,
                Name = string.IsNullOrWhiteSpace(root) ? property.Name : root + '.' + property.Name,
                EndUserTerm = property.GetCustomAttribute<TagEndUserTermAttribute>() is TagEndUserTermAttribute e ? e.EndUserTerm : string.Empty,

                TagType = property.GetCustomAttribute<TagTypeAttribute>() is TagTypeAttribute p ? p.TagType : TagCarry,
                Description = property.GetCustomAttribute<DescriptionAttribute>() is DescriptionAttribute description
                ? description.Description
                : string.Empty
            };


            return config;
        }

        private static TagConfig GetMethodeTagConfig(MethodInfo property, string root, TagType TagCarry, bool Iec)
        {
            var paramters = property.GetParameters().Aggregate(string.Empty, (accumulator, item) =>
                accumulator += (Iec ? item.ParameterType.GetIecTypeString() : item.ParameterType.ToString())
                    + " " + item.Name + ",").TrimEnd(',');
            TagConfig config = new TagConfig()
            {
                DataType = property.ReturnType,
                Name = (string.IsNullOrWhiteSpace(root) ? property.Name : root + '.' + property.Name) + "(" + paramters + ")",
                EndUserTerm = property.GetCustomAttribute<TagEndUserTermAttribute>() is TagEndUserTermAttribute e ? e.EndUserTerm : string.Empty,

                TagType = property.GetCustomAttribute<TagTypeAttribute>() is TagTypeAttribute p ? p.TagType : TagCarry,
                Description = property.GetCustomAttribute<DescriptionAttribute>() is DescriptionAttribute description
                ? description.Description
                : string.Empty
            };



            return config;
        }
        private static (int, string) GetNameAndLenght(TagConfig root, PropertyInfo property)
        {
            int length;
            string name = string.Empty;
            string arrayRoot = string.IsNullOrWhiteSpace(root.Name) ? root.Name : root.Name + '.';
            var fixedSizeAttribute = property.GetCustomAttribute(typeof(TagFixedSizeAttribute)) as TagFixedSizeAttribute;

            if (fixedSizeAttribute != null && fixedSizeAttribute.Size >= 0)
            {
                if (property.PropertyType == typeof(char[]))
                {
                    name = arrayRoot + property.Name; // + $"[{fixedSizeAttribute.Size}]";
                    length = fixedSizeAttribute.Size;
                }
                else
                {
                    name = arrayRoot + property.Name + $"[0..{fixedSizeAttribute.Size}]";
                    length = fixedSizeAttribute.Size;
                }
            }
            else
            {
                name = arrayRoot + property.Name + $"[#]";
                length = -1;
            }

            return (length, name);
        }
        private static bool IsObject(this TagConfig config, IEnumerable<Type> types)
        {
            return config.DataType.IsObject(types);
        }
        private static bool IsObject(this Type type, IEnumerable<Type> types)
        {
            return (type.IsInterface
                    || type.IsClass
                    || type.IsValueType)
                    && !type.IsPrimitive
                    && type != typeof(string)
                    && type != typeof(DateTime)
                    && type != typeof(TimeSpan)
                    && !type.IsEnum
                    && !types.Contains(type);
        }

        private static bool IsCollection(this TagConfig config, IEnumerable<Type> types)
        {
            return config.DataType.IsCollection(types);
        }

        private static bool IsCollection(this Type type, IEnumerable<Type> types)
        {
            return (type.IsArray
                    || type.GetInterfaces().Contains(typeof(IEnumerable)))
                    && type != typeof(string)
                    && !types.Contains(type);
        }

    }
}
