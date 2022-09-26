using System;
using System.Collections.Generic;
using System.Linq;

namespace Open.PackML.Tags.Builders
{
    public static class TagTableBuilder
    {
        public static TagTable BuildTagTable(Dictionary<string,object> collection)
        {
            var table = new TagTable();
            foreach (var item in collection)
            {
                BuildTable(TagTreeBuilder.GetTree(item.Key, item.Value), table);
            }
            table.GenerateArray();
            table.GenerateOnUpdate = true;
            return table;
        }

        public static TagTable BuildTable(IEnumerable<TagDetail> tagDetails)
        {
            var table = new TagTable();
            foreach (var tagDetail in tagDetails)
            {
                tagDetail.BuildTable(table);
            }
            table.GenerateArray();
            table.GenerateOnUpdate = true;
            return table;
        }
        public static TagTable BuildTable(this TagDetail tagDetails)
        {
            var table = new TagTable();
            tagDetails.BuildTable(table);
            table.GenerateArray();
            table.GenerateOnUpdate = true;
            return table;
        }

        public static void BuildTable(this TagDetail tagDetails, TagTable table)
        {
            table.Add(tagDetails.SearchString, tagDetails);
            //tagDetails.TagNameUpdate += table.TagUpdated;
            foreach (TagDetail tagtree in tagDetails.ChildTags)
                tagtree.BuildTable(table);
        }
    }
}
