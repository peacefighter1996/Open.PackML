using System;
using System.Linq;

namespace Open.PackML.Tags
{
    /// <summary>
    /// A tag call is a request to do something with a tag.
    /// </summary>
    public class TagCall
    {
        /// <summary>
        /// Creates a new tag call
        /// </summary>
        public TagCall()
        {
            Data = null;
            TagCallType = TagCallType.None;
            TagName = string.Empty;
        }
        /// <summary>
        /// Creates a new tag call
        /// </summary>
        /// <param name="tagCallType">The type of tag call.</param>
        /// <param name="tagName">Name of the tag being called</param>
        /// <param name="data">Data as input or set value data</param>
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
}