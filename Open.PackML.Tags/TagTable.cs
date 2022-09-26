using System;
using System.Collections.Generic;
using System.Linq;

namespace Open.PackML.Tags
{
    public class TagTable : Dictionary<string, TagDetail>
    {
        TagDetail[] array;
        private bool generateOnUpdate;

        public TagTable() : base()
        {

        }
        public TagTable(TagDetail[] tags) : base()
        {
            foreach (var tag in tags)
            {
                Add(tag.TagName, tag);
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
            if (sender is TagDetail data)
            {
                Remove(data.SearchString);
                Add(data.TagName, data);
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

        public new void Add(string key, TagDetail value)
        {
            base.Add(key, value);
            value.TagNameUpdate += TagNameUpdated;
            if (generateOnUpdate) GenerateArray();
        }

        public void GenerateArray()
        {
            array = this.Select(o => o.Value).ToArray();
        }

        public TagDetail[] GetArray { get => array == null ? Array.Empty<TagDetail>() : array; }
    }
}
