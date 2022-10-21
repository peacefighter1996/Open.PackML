﻿using System;
using System.Collections;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Xml;
using System.Xml.Linq;

namespace Autabee.Utility.IEC61131TypeConversion
{
    public static class IecTypeConvertor
    {
        // source https://www.plcnext.help/te/Programming/Csharp/Csharp_programming/Csharp_Supported_data_types.htm
        public static string GetIecTypeString<T>(this T value)
        {
            Type type = value.GetType();
            return GetIecTypeString(type);

        }
        public static string GetIecTypeString(this Type type)
        {
            if (type.IsArray)
            {
                var result = GetIecTypeString(type.GetElementType()) + "[]";
                if (string.Equals( result, "USINT[]"))
                {
                    return IECType.STRING;
                }
                else
                {
                    return result;
                }
            }
            else
            {
                switch (type.FullName)
                {
                    case "System.Boolean":
                        return IECType.BOOL;

                    //unsigned Integers 
                    case "System.Byte":
                        return IECType.USINT;
                    case "System.UInt16":
                        return IECType.UINT;
                    case "System.UInt32":
                        return IECType.UDINT;
                    case "System.UInt64":
                        return IECType.ULINT;

                    //signed Integers 
                    case "System.SByte":
                        return IECType.SINT;
                    case "System.Int16":
                        return IECType.INT;
                    case "System.Int32":
                        return IECType.DINT;
                    case "System.Int64":
                        return IECType.LINT;

                    //Floating Points
                    case "System.Float":
                        return IECType.REAL;
                    case "System.Double":
                        return IECType.LREAL;

                    //Duration
                    case "System.TimeSpan":
                        return IECType.LTIME;
                    case "System.Date":
                        return IECType.DATE;

                    case "System.Char":
                        return IECType.CHAR;
                    case "System.String":
                        return IECType.WSTRING;
                    case "System.Collections.BitArray":
                        return IECType.BOOL + "[]";
                    default:
                        return "UDT_" + type.FullName.Replace('.', '_');
                }
            }

        }

        public static Type GetCsharpType(string typeString)
        {
            switch (typeString)
            {
                case "BOOL":
                    return typeof(bool);
                case "USINT":
                    return typeof(byte);
                case "BYTE":
                case "WORD":
                case "DWORD":
                case "LWORD":
                case "BOOL[]":
                    return typeof(BitArray);
                case "UINT":
                    return typeof(ushort);
                case "UDINT":
                    return typeof(uint);
                case "ULINT":
                    return typeof(ulong);
                case "SINT":
                    return typeof(sbyte);
                case "INT":
                    return typeof(short);
                case "DINT":
                    return typeof(int);
                case "LINT":
                    return typeof(long);
                case "REAL":
                    return typeof(float);
                case "LREAL":
                    return typeof(double);
                case "LTIME":
                    return typeof(TimeSpan);
                case "DATE":
                    return typeof(DateTime);
                case "CHAR":
                    return typeof(char);
                default:
                    
                    if (typeString.StartsWith("UDT"))
                    {
                        return Assembly.GetEntryAssembly().GetType(typeString.Substring(4).Replace('_', '.'));
                    }
                    else
                    {
                        throw new ArgumentException($"Unable to convert {typeString} to c# type");
                    }
            }
        }


        public static XmlDocument GetIecTypeStructure<T>(this T value, int depth = 1)
        {
            var item = new XmlDocument();
            item.NameTable.Add(value.GetType().Namespace);
            var node = item.CreateElement(value.GetIecTypeString());
            //node.SetAttribute("type", value.GetIecTypeString());
            item.AppendChild(node);
            AddSubNodes(value.GetType(), item, node, depth - 1);
            return item;
        }

        private static void AddSubNodes(Type value, XmlDocument item, XmlNode baseNode, int depth)
        {
            foreach (var property in value.GetProperties())
            {
                var node = item.CreateElement(property.Name);
                node.SetAttribute("type", property.PropertyType.GetIecTypeString());
                baseNode.AppendChild(node);
                if (depth > 0 && node.Attributes[0].Value.Substring(0, 3).Equals("UDT"))
                {
                    AddSubNodes(property.PropertyType, item, node, depth - 1);
                }
            }
        }
    }




}
