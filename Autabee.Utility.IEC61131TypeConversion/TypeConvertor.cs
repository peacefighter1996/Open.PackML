using System;
using System.Collections;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Xml;
using System.Xml.Linq;

namespace Autabee.Automation.Utility.IEC61131TypeConversion
{
    public static class IecTypeConvertor
    {
        // source https://www.plcnext.help/te/Programming/Csharp/Csharp_programming/Csharp_Supported_data_types.htm
        public static string GetIecTypeString<T>(this T value)
        {
            Type type = value.GetType();
            if (type.IsArray)
            {
                var result = GetIecTypeString(type.GetElementType()) + "[]";
                if (result == "USINT[]")
                {
                    return IEC.IECType.STRING;
                }
                else
                {
                    return result;
                }
            }
            else if (type.FullName == "System.Collections.BitArray")
            {
                var temp = value as BitArray;
                switch (temp.Count)
                {
                    case 1:
                        return IEC.IECType.BOOL;
                    case 8:
                        return IEC.IECType.BYTE;
                    case 16:
                        return IEC.IECType.WORD;
                    case 32:
                        return IEC.IECType.DWORD;
                    case 64:
                        return IEC.IECType.LWORD;
                    default:
                        return String.Format("BOOL[{0}]", temp.Count);
                }
            }
            else
            {
                return GetIecTypeString(type);
            }
        }
        public static string GetIecTypeString(this Type type)
        {
            switch (type.FullName)
            {
                case "System.Boolean":
                    return IEC.IECType.BOOL;

                //unsigned Integers 
                case "System.Byte":
                    return IEC.IECType.USINT;
                case "System.UInt16":
                    return IEC.IECType.UINT;
                case "System.UInt32":
                    return IEC.IECType.UDINT;
                case "System.UInt64":
                    return IEC.IECType.ULINT;

                //signed Integers 
                case "System.SByte":
                    return IEC.IECType.SINT;
                case "System.Int16":
                    return IEC.IECType.INT;
                case "System.Int32":
                    return IEC.IECType.DINT;
                case "System.Int64":
                    return IEC.IECType.LINT;

                //Floating Points
                case "System.Float":
                    return IEC.IECType.REAL;
                case "System.Double":
                    return IEC.IECType.LREAL;

                //Duration
                case "System.TimeSpan":
                    return IEC.IECType.LTIME;
                case "System.Date":
                    return IEC.IECType.DATE;

                case "System.Char":
                    return IEC.IECType.CHAR;
                case "System.String":
                    return IEC.IECType.WSTRING;
                case "System.Array":
                    return IEC.IECType.STRING;
                default:
                    return "UDT_" + type.FullName.Replace('.', '_');
            }
        }

        public static Type GetCsharpType<T>(string typeString)
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

                default:
                    if (typeString.StartsWith("UDT"))
                    {
                        return Assembly.GetEntryAssembly().GetType(typeString.Substring(4).Replace('_', '.'));
                    }
                    else
                    {
                        throw new ArgumentException("Unable to convert to c# type");
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
            AddSubNodes(value.GetType(), item, node,depth-1);
            return item;
        }

        private static void AddSubNodes(Type value, XmlDocument item, XmlNode baseNode, int depth)
        {
            foreach (var property in value.GetProperties())
            {
                var node = item.CreateElement(property.Name);
                node.SetAttribute("type", property.PropertyType.GetIecTypeString());
                baseNode.AppendChild(node);
                if(depth > 0 && node.Attributes[0].Value.Substring(0,3) == "UDT")
                {
                    AddSubNodes(property.PropertyType, item, node, depth-1);
                }
            }
        }
    }




}
