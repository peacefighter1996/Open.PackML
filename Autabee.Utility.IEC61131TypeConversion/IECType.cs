using System.Linq;

namespace Autabee.Utility.IEC61131TypeConversion
{
    public static class IECType
    {
        private static string[] DefaultTypes { get; } =
        {
            "BOOL",
            "BYTE",
            "WORD",
            "DWORD",
            "LWORD",
            "STRING",
            "WSTRING",
            "CHAR",
            "SINT",
            "INT",
            "DINT",
            "USINT",
            "UINT",
            "UDINT",
            "ULINT",
            "REAL",
            "LREAL",
            "DATE",
            "LTIME",
            "OBJECT"
        };

        public static string BOOL { get => "BOOL"; }
        public static string BYTE { get => "BYTE"; }
        public static string WORD { get => "WORD"; }
        public static string DWORD { get => "DWORD"; }
        public static string LWORD { get => "LWORD"; }

        public static string STRING { get => "STRING"; }
        public static string WSTRING { get => "WSTRING"; }
        public static string CHAR { get => "CHAR"; }

        public static string SINT { get => "SINT"; }
        public static string INT { get => "INT"; }
        public static string DINT { get => "DINT"; }
        public static string LINT { get => "LINT"; }

        public static string USINT { get => "USINT"; }
        public static string UINT { get => "UINT"; }
        public static string UDINT { get => "UDINT"; }
        public static string ULINT { get => "ULINT"; }

        public static string REAL { get => "REAL"; }
        public static string LREAL { get => "LREAL"; }

        public static string DATE { get => "DATE"; }
        public static string LTIME { get => "LTIME"; }
        public static string OBJECT { get => "OBJECT"; }

        public static string ArrayOf(string type) => $"{type}[]";

        public static bool ContainsType(string type) {
            type = type.ToUpper();
            
            if (DefaultTypes.Contains(type) || type.StartsWith("UDT_")) return true;
            if (type.EndsWith("]")) return ContainsType(type.Substring(0, type.IndexOf('[')));
            return false;
        }
    }
}
