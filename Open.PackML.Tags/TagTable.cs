using System;
using System.Collections.Generic;
using System.Linq;

namespace Open.PackML.Tags
{
    public class TagTable : Dictionary<string, TagConfig>
    {
        TagConfig[] array;
        private bool generateOnUpdate;

        public TagTable() : base()
        {

        }
        public TagTable(TagConfig[] tags) : base()
        {
            foreach (var tag in tags)
            {
                Add(tag.Name, tag);
            }
        }
        public string GetTagTablePrint(bool filterUndifined = false)
        {
            if(filterUndifined)
            return array
                .Where(o => o.TagType != TagType.Undefined)
                .Aggregate("", (accumulator, item) => accumulator += item.ToString() + Environment.NewLine);
            return array
                .Aggregate("", (accumulator, item) => accumulator += item.ToString() + Environment.NewLine);
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
                GenerateArray();
            }
        }

        public new void Add(string key, TagConfig value)
        {
            base.Add(key, value);
            value.TagNameUpdate += TagNameUpdated;
            if (generateOnUpdate) GenerateArray();
        }

        public void GenerateArray()
        {
            array = this.Select(o => o.Value).ToArray();
        }

        public TagConfig[] GetTags { get => array == null ? Array.Empty<TagConfig>() : array; }
    }
}
