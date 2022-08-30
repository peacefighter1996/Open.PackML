using System;
using System.Collections;
using System.Linq;
using System.Reflection;

namespace Autabee.Automation.Utility.IEC61131TypeConversion
{
    public static class IecTypeConvertor
    {
        // source https://www.plcnext.help/te/Programming/Csharp/Csharp_programming/Csharp_Supported_data_types.htm
        public static string GetIecType<T>(this T value)
        {
            Type type = value.GetType();
            if (type.IsArray)
            {
                var result = GetIecType(type.GetElementType()) + "[]";
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
                if (temp.Count == 1)
                {
                    return IEC.IECType.BOOL;
                }
                else if (temp.Count == 8)
                {
                    return IEC.IECType.BYTE;
                }
                else if ( temp.Count == 16)
                {
                    return IEC.IECType.WORD;
                }
                else if (temp.Count == 32)
                {
                    return IEC.IECType.DWORD;
                }
                else if (temp.Count == 64)
                {
                    return IEC.IECType.LWORD;
                }
                else 
                {
                    return "BOOL[]";
                }
            }
            else
            {
                return GetIecType(type);
            }
        }

        private static string GetIecType(Type type)
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

        public static Type GetCsharpType<T>(string type, object value)
        {
            switch (type)
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
                case "":
                default:
                    if (type.StartsWith("UDT"))
                    {
                        return Assembly.GetEntryAssembly().GetType(type.Substring(4).Replace('_', '.'));
                    }
                    else
                    {
                        throw new ArgumentException("Unable to convert to c# type");
                    }
            }
        }
    }




}
