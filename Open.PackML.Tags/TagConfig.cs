
using Open.PackML.Tags.IEC;
using System;
using System.Linq;
using System.Reflection;
using System.Xml.Serialization;

namespace Open.PackML.Tags
{
    public class TagConfig
    {
        string tagName;
        public event EventHandler TagNameUpdate;
        public string TagName
        {
            get => tagName; set
            {
                if (value != string.Empty && string.IsNullOrWhiteSpace(value)) TagName = string.Empty;
                if (tagName == value) return;
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
        }
        [XmlIgnore]
        public string TagTail { get; private set; }
        [XmlIgnore]
        public string SearchString { get; private set; }
        public string EndUserTerm { get; set; }
        public string Description { get; set; }
        public virtual TagType TagType { get; set; }
        public virtual Type DataType { get; set; }
        
        [XmlIgnore]
        public string[] TagAddress { get; private set; }

        public TagConfig() { }

        public TagConfig(string name, string endUserTerm, string description, TagType tagType, Type dataType)
        {
            TagName = name;
            EndUserTerm = endUserTerm;
            Description = description;
            TagType = tagType;
            DataType = dataType;
        }

        public TagConfig(TagConfig tagConfig) : this(tagConfig.TagName, tagConfig.EndUserTerm, tagConfig.Description, tagConfig.TagType, tagConfig.DataType)
        {
        }

        public override string ToString()
        {
            return string.Format("{0},{1},{2},{3},{4}", TagType.ToString(), TagName, EndUserTerm, Description, DataType.GetIecTypeString());
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