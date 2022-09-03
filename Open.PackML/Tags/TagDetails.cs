using Autabee.Automation.Utility.IEC61131TypeConversion;
using System;

namespace Open.PackML.Tags
{
    public class TagDetails:TagConfig
    {
        
        public TagDetails(TagConfig config, TagDetails[] childTags, bool writable, bool readable): base(config)
        {
            DataType=config.DataType;

            if (childTags != null) ChildTags = childTags;
            else ChildTags = new TagDetails[0];
            TagNodeAddress = config.TagName;
            Writable = writable;
            Readable = readable;
            TagNodeAddressComponents = config.TagName.Split('.');
        }
        public TagDetails[] ChildTags { get; }
        public string TagNodeAddress { get; }
        public string[] TagNodeAddressComponents { get; }
        public bool Writable { get; }
        public bool Readable { get; }

    }

    public class ArrayTagDetail : TagDetails
    {
        public ArrayTagDetail(TagConfig config, TagDetails[] childTags,  bool writable, bool readable, int length) : base(config, childTags,  writable, readable)
        {
            Length = length;
        }
        public bool FixedSize { get => !Writable; }
        public int Length { get; }
    }

    public class TagConfig
    {
        public string TagName { get; set; }
        public string EndUserTerm { get; set; }
        public string Description { get; set; }
        public virtual TagType TagType { get; set; }
        public Type DataType { get; set; }

        public TagConfig() { }

        public TagConfig(string name, string endUserTerm, string description, TagType tagType, Type dataType)
        {
            TagName = name;
            EndUserTerm = endUserTerm;
            Description = description;
            TagType = tagType;
            DataType = dataType;
        }

        public TagConfig(TagConfig tagConfig)
        {
            TagName = tagConfig.TagName;
            EndUserTerm = tagConfig.EndUserTerm;
            Description = tagConfig.Description;
            TagType = tagConfig.TagType;
            DataType = tagConfig.DataType;
        }

        public override string ToString()
        {
            return String.Format("{0},{1},{2},{3},{4}", TagType.ToString(), TagName, EndUserTerm, Description, DataType.GetIecTypeString());
        }

    }
}