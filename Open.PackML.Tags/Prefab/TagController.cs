using Autabee.Utility;
using Open.PackML.Interfaces;
using Open.PackML.Tags.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace Open.PackML.Tags.Prefab
{

    public class TagController : ITagController, ITagStore
    {
        // Dictionary<string, TagDetails> valuePairs;
        TagTable tagTable;
        public TagController(Dictionary<string, object> StoredObjects
            //, System.Timers.Timer timer = null
            )
        {
            // valuePairs = new Dictionary<string, TagDetails>();
            var tagdetails = StoredObjects.Select(StoredObject => TagTreeBuilder.GetTree(StoredObject.Key, StoredObject.Value));
            tagTable = TagTableBuilder.BuildTable(tagdetails);
            //if(timer != null)
            //{
            //    timer.Elapsed += Timer_Elapsed;
            //}
        }
        
        //private bool updating;
        //private void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        //{
        //    if (updating) return;
        //    updating = true;
        //    var array = tagTable.GetArray;
        //    for (int i = 0; i < array.Length; i++)
        //    {
        //        var result = array[i];
        //    }
        //    updating = false;
        //}

        //public event EventHandler<TagUpdate> NotifySubscriptions;

        public Task<object[]> AsyncExecutePackTagCommand(string name, params object[] args)
        {
            throw new NotImplementedException();
        }

        public ValidationResult<TagDetail> Browse(string Orgin)
        {
            return tagTable.TryGetValue(Orgin, out TagDetail tagDetail)
                ? new ValidationResult<TagDetail>(true, tagDetail)
                : new ValidationResult<TagDetail>(false, unSuccesfullText: "Browse item not found");
        }

        public ValidationResult<TagDetail> Browse(string Orgin, int Depth = 1)
        {
            return tagTable.TryGetValue(Orgin, out TagDetail tagDetail)
                ? new ValidationResult<TagDetail>(true, tagDetail)
                : new ValidationResult<TagDetail>(false, unSuccesfullText: "Browse item not found");
        }

        public object[] ExecutePackTagCommand(string name, params object[] args)
        {
            throw new NotImplementedException();
        }

        public ValidationResult<object> GetTagData(string name)
        {
            if (tagTable.TryGetValue(
                TagConfig.TagStringToSearch(name), 
                out TagDetail tagDetails))
                return tagDetails.GetValue();
            
            return new ValidationResult<object>(false, null, "Tag not found");
        }

        public ValidationResult SetTagData(string name, object data)
        {
            if (tagTable.TryGetValue(
                TagConfig.TagStringToSearch(name),
                out TagDetail tagDetails))
                return tagDetails.SetValue(data);

            return new ValidationResult<object>(false, null, "Tag not found");
        }
    }
}
