using System;
using System.Linq;

namespace Open.PackML.Tags
{
    /// <summary>
    /// A tag call is a request to do something with a tag.
    /// </summary>
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

        /// <summary>
        /// Data as input or set value data
        /// </summary>
        public object Data { get; set; }

        /// <summary>
        /// The type of tag call.
        /// </summary>
        public TagCallType TagCallType { get; set; }
        /// <summary>
        /// Name of the tag being called
        /// </summary>
        public string TagName { get; set; }
    }



    //public class BatchTagCall: Dictionary<uint,List<TagCall>>
    //{
    //    public bool allowAsyncCalls;
    //}
    //
    //public static class TagCallBuilder
    //{
    //    public static TagCall MethodCall(string tagName, object[] inputArguments)
    //    {
    //        return new TagCall(TagCallType.Execute, tagName, inputArguments);
    //    }
    //    public static TagCall GetCall(string tagName)
    //    {
    //        return new TagCall(TagCallType.Get, tagName);
    //    }
    //    public static TagCall SetCall(string tagName)
    //    {
    //        return new TagCall(TagCallType.Set, tagName);
    //    }
    //}
}