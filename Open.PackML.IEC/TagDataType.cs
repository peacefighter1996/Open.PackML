using Autabee.Automation.Utility.IEC61131TypeConversion;
using Open.PackML.Tags;
using System;

namespace Open.PackML.Iec
{
    public static class TagDataType
    {
        public static string GetIecType(this TagDetails tag)
        {
            return IecTypeConvertor.GetIecTypeString<Type>(tag.DataType);
        }
    }
}
