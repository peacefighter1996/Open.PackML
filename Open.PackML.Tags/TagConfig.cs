using Autabee.Utility.IEC61131StringTypeConversion;
using System;
using System.Linq;
#if (NET6_0_OR_GREATER)
using System.Text.Json.Serialization;
#endif
using System.Xml.Serialization;

namespace Open.PackML.Tags
{
    [Serializable]
    public class TagConfig
    {
        string tagName;

        public TagConfig() { }

        public TagConfig(TagConfig tagConfig) : this(tagConfig.Name, tagConfig.EndUserTerm, tagConfig.Description, tagConfig.TagType, tagConfig.DataType)
        {
        }

        public TagConfig(string name, string endUserTerm, string description, TagType tagType, Type dataType)
        {
            Name = name;
            EndUserTerm = endUserTerm;
            Description = description;
            TagType = tagType;
            DataType = dataType;
        }

        public event EventHandler TagNameUpdate;

        private void UpdateTagName(string value)
        {
            if (value == string.Empty)
            {
                tagName = value;
                SearchString = value;
                TagTail = value;
                TagAddress = new string[1] { value };
                //SearchHash = 0;
            }
            else
            {

                tagName = value;
                SearchString = TagStringToSearch(value);
                //SearchHash = SearchString.GetHashCode();
                TagAddress = SearchString.Split('.');
                TagTail = TagAddress.Last();
            }
            TagNameUpdate?.Invoke(this, EventArgs.Empty);
        }

        public static string TagStringToSearch(string name)
        {
            if (name == string.Empty) return string.Empty;
            var searchname = name;
            var i = searchname.IndexOf('[', 0);
            while (i != -1)
            {

                searchname = searchname.Substring(0, i) + searchname.Substring(searchname.IndexOf(']', i) + 1);

                i = searchname.IndexOf('[', 0);

            }
            return searchname;
        }
        public string ToIecString()
        {
            return string.Format("{0},{1},{2},{3},{4}", TagType.ToString(), Name, EndUserTerm, Description, DataType.GetIecTypeString());
        }
        public override string ToString()
        {
            return string.Format("{0},{1},{2},{3},{4}", TagType.ToString(), Name, EndUserTerm, Description, DataType.ToString());
        }

        public virtual Type DataType { get; set; }
        public string Description { get; set; }
        public string EndUserTerm { get; set; }
        public string Name
        {
            get => tagName; set
            {

                if (value == null)
                {
                    UpdateTagName(string.Empty);
                    return;
                }
                value = value.Trim();
                if (value.Equals(Name)) return;
                UpdateTagName(value);
            }
        }
        [XmlIgnore]
        public string SearchString { get; private set; }

        [XmlIgnore]
        public string TagTail { get; private set; }
        public virtual TagType TagType { get; set; }

#if (NET6_0_OR_GREATER)
        [JsonIgnore]
#endif
        [XmlIgnore]
        public string[] TagAddress { get; private set; }
    }
}