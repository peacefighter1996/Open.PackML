using System;
using System.Collections.Generic;
using System.Linq;

namespace Open.PackML.Tags
{
    /// <summary>
    /// This class is used to store all the tags that are used in a given controller.
    /// </summary>
    public class TagTable : Dictionary<string, TagConfig>
    {
        private TagConfig[] roots;
        private TagConfig[] array;
        private bool generateOnUpdate;

        public TagTable() : base()
        {

        }
        public TagTable(TagConfig[] roots) : base()
        {
            this.roots = roots;
        }
        public TagTable(TagConfig[] roots, TagConfig[] tags) : base()
        {
            this.roots = roots;
            foreach (var tag in tags)
            {
                Add(tag.Name, tag);
            }
        }
        public string GetTagTablePrint(bool filterUndifined = false, bool Iec = false)
        {
            IEnumerable<TagConfig> temp = array;
            if (filterUndifined) temp = temp.Where(o => o.TagType != TagType.Undefined);
            return array
                .Aggregate(string.Empty, (accumulator, item) => accumulator += (Iec ? item.ToIecString() : item.ToString()) + Environment.NewLine);
        }
        public void TagNameUpdated(object sender, EventArgs e)
        {
            if (sender is TagConfig data)
            {
                Remove(data.SearchString);
                Add(data.Name, data);
            }
        }

        public bool GenerateOnUpdate
        {
            get => generateOnUpdate; set
            {
                if (value == generateOnUpdate) return;
                generateOnUpdate = true;
            }
        }

        public new void Add(string key, TagConfig value)
        {
            base.Add(key, value);
            value.TagNameUpdate += TagNameUpdated;
            if (generateOnUpdate) UpdateArray(value);
        }



        public void AddRoot(string key, TagConfig value)
        {
            base.Add(key, value);
            UpdateRootArray(value);
            value.TagNameUpdate += TagNameUpdated;
            if (generateOnUpdate) UpdateArray(value);
        }

        private void UpdateArray(TagConfig config)
        {
            array = array.Append(config).OrderBy(o => o.TagAddress).ToArray();
        }
        private void UpdateRootArray(TagConfig config)
        {
            roots = roots.Append(config).OrderBy(o => o.TagAddress).ToArray();
        }

        public void GenerateArray()
        {
            array = this.Select(o => o.Value).ToArray();
        }

        /// <summary>
        /// Pre generated array of all the tags in the table
        /// </summary>
        public TagConfig[] GetTags { get => array == null ? Array.Empty<TagConfig>() : array; }
        /// <summary>
        /// tags that are on the top level of managed objects
        /// </summary>
        public TagConfig[] GetRoots { get => roots == null ? Array.Empty<TagConfig>() : roots; }
    }
}
