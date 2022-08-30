using System.Linq;

namespace IEC
{
    public static class IECType
    {
        private static string[] types =
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

        public static bool ContainsType(string type)
        {
            return types.Contains(type);
        }
    }
}
