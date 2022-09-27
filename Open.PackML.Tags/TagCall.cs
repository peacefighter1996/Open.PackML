using System;
using System.Linq;

namespace Open.PackML.Tags
{
    public class TagCall
    {
        public TagCall()
        {
        }
        public TagCall(TagCallType tagCallType, string tagName, object data = null)
        {
            TagCallType = tagCallType;
            TagName = tagName;
            Data = data;
        }

        public TagCallType TagCallType { get; set; }
        public object Data { get; set; }
        public string TagName { get; set; }
    }
}