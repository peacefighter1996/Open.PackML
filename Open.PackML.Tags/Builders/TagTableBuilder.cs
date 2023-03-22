using System;
using System.Collections.Generic;
using System.Linq;

namespace Open.PackML.Tags.Builders
{
    /// <summary>
    /// This class builds a table of tags for a given object.
    /// </summary>
    public static class TagTableBuilder
    {
        // Generates a TagTable for a given object
        // TagName: base name of the tag
        // obj: Object to generate the TagTable for
        public static TagTable BuildTagTable(string TagName, object obj)
        {
            var table = new TagTable();

            table.ExtendTableAtRoot(TagTreeBuilder.GetTree(TagName, obj), false);

            table.GenerateArray();
            table.GenerateOnUpdate = true;
            return table;
        }
        /// <summary>
        /// This code creates a new tag table for a given collection. using the key as the
        /// tag table base name. This is used for the PackML state machine.
        /// </summary>
        /// <param name="collection">collection of (tag base name, object)</param>
        public static TagTable BuildTagTable(Dictionary<string, object> collection)
        {
            var table = new TagTable();
            foreach (var item in collection)
            {
                table.ExtendTableAtRoot(TagTreeBuilder.GetTree(item.Key, item.Value), false);
            }
            table.GenerateArray();
            table.GenerateOnUpdate = true;
            return table;
        }
        internal static TagTable BuildTable(IEnumerable<TagDetail> tagDetails)
        {
            var table = new TagTable(tagDetails.ToArray());
            foreach (var tagDetail in tagDetails)
            {
                table.ExtendTable(tagDetail, false);
            }
            table.GenerateArray();
            table.GenerateOnUpdate = true;
            return table;
        }
        [assembly: InternalVisibleTo("Open.PackMLTests")]
        internal static TagTable BuildTable(this TagDetail tagDetails)
        {
            var table = new TagTable(new TagDetail[] { tagDetails });
            table.ExtendTable(tagDetails, false);
            table.GenerateArray();
            table.GenerateOnUpdate = true;
            return table;
        }

        [assembly: InternalVisibleTo("Open.PackMLTests")]
        internal static void ExtendTable(this TagTable table, TagDetail tagDetails, bool update = true)
        {
            if (update) table.GenerateOnUpdate = false;
            table.Add(tagDetails.SearchString, tagDetails);
            for (int i = 0; i < tagDetails.ChildTags.Length; i++)
                table.ExtendTable(tagDetails.ChildTags[i], false);
            if (update)
            {
                table.GenerateArray();
                table.GenerateOnUpdate = true;
            }
        }
        [assembly: InternalVisibleTo("Open.PackMLTests")]
        internal static void ExtendTableAtRoot(this TagTable table, TagDetail tagDetails, bool update = true)
        {
            if (update) table.GenerateOnUpdate = false;
            table.AddRoot(tagDetails.SearchString, tagDetails);
            for (int i = 0; i < tagDetails.ChildTags.Length; i++)
                table.ExtendTable(tagDetails.ChildTags[i], false);
            if (update)
            {
                table.GenerateArray();
                table.GenerateOnUpdate = true;
            }
        }

    }
}
