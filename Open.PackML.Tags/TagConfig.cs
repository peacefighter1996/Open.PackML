using Autabee.Utility.IEC61131TypeConversion;
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
        public event EventHandler TagNameUpdate;
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

        [XmlIgnore]
        public string TagTail { get; private set; }
        [XmlIgnore]
        public string SearchString { get; private set; }
        public string EndUserTerm { get; set; }
        public string Description { get; set; }
        public virtual TagType TagType { get; set; }
        public virtual Type DataType { get; set; }
#if (NET6_0_OR_GREATER)
        [JsonIgnore]
#endif
        [XmlIgnore]
        public string[] TagAddress { get; private set; }

        public TagConfig() { }

        public TagConfig(string name, string endUserTerm, string description, TagType tagType, Type dataType)
        {
            Name = name;
            EndUserTerm = endUserTerm;
            Description = description;
            TagType = tagType;
            DataType = dataType;
        }

        public TagConfig(TagConfig tagConfig) : this(tagConfig.Name, tagConfig.EndUserTerm, tagConfig.Description, tagConfig.TagType, tagConfig.DataType)
        {
        }
        public string ToIecString()
        {
            return string.Format("{0},{1},{2},{3},{4}", TagType.ToString(), Name, EndUserTerm, Description, DataType.GetIecTypeString());
        }
        public override string ToString()
        {
            return string.Format("{0},{1},{2},{3},{4}", TagType.ToString(), Name, EndUserTerm, Description, DataType.ToString());
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
    }
}