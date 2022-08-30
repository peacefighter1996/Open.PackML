using Autabee.Automation.Utility.IEC61131TypeConversion;
using System;

namespace Open.PackML.Iec
{
    public static class TagDataType
    {
        public static string GetIecType(this TagDetails tag)
        {
            return IecTypeConvertor.GetIecType(tag.DataType);
        }
    }
}
