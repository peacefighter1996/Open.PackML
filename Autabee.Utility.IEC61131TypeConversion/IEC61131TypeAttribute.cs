using System;
using System.Reflection;

namespace IEC
{
    public class IEC61131TypeAttribute : CustomAttributeData
    {
        public string Type { get; private set; }
        public IEC61131TypeAttribute(string type) : base()
        {
            type = type.ToUpper();
            if (IECType.ContainsType(type))
            {
                Type = type;
            }
            else
            {
                throw new ArgumentException("Unkown IEC Type");
            }
        }
    }
}
