﻿using Autabee.Utility;
using Open.PackML.Interfaces;
using Open.PackML.Tags.Builders;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Xml.Linq;

namespace Open.PackML.Tags.Prefab
{

    public class TagController : ITagController, ITagStore
    {
        TagTable tagTable;

        public TagController(IEnumerable<TagDetail> tagDetails)
        {
            tagTable = TagTableBuilder.BuildTable(tagDetails);
        }

        public TagController(TagTable tagTable)
        {
            this.tagTable = tagTable;
        }

        public TagController(Dictionary<string, object> StoredObjects
            //, System.Timers.Timer timer = null
            , bool Iec = false
            )
        {
            var tagdetails = StoredObjects.Select(StoredObject => TagTreeBuilder.GetTree(StoredObject.Key, StoredObject.Value, Iec: Iec));
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

        ValidationResult<Queue<int>> GetTagArrayIndexes(string name)
        {
            try
            {
                Queue<int> queue = new Queue<int>();
                var index = name.IndexOf('[');
                while (index > -1)
                {
                    var index2 = name.IndexOf(']', index);
                    var value = int.Parse(name.Substring(index + 1, index2 - index - 1));
                    queue.Enqueue(value);
                    index = name.IndexOf('[', index2 + 1);
                }
                return new ValidationResult<Queue<int>>(true, queue);
            }
            catch (FormatException e)
            {
                return new ValidationResult<Queue<int>>(false, null, e.Message);
            }
        }

        public async Task<ValidationResult<object>> AsyncExecutePackTagCommand(string name, params object[] args)
        {
            if (tagTable.TryGetValue(TagConfig.TagStringToSearch(name), out TagDetail tagDetail))
            {
                var queue = GetTagArrayIndexes(name);
                if (!queue.Success) return new ValidationResult<object>(false, unSuccesfullText: "Array number parsing failure: {0}", formatObjects: queue.FailString());
                return await tagDetail.ExecuteAsync(queue.Object, args).ConfigureAwait(true);
            }
            else return new ValidationResult<object>(false, unSuccesfullText: "Tag {0} not found", formatObjects: name);
        }



        public ValidationResult<TagDetail> Browse(string name)
        {
            return tagTable.TryGetValue(TagConfig.TagStringToSearch(name), out TagDetail tagDetail)
                ? new ValidationResult<TagDetail>(true, tagDetail)
                : new ValidationResult<TagDetail>(false, unSuccesfullText: "Tag {0} not found", formatObjects: name);
        }

        public ValidationResult<TagDetail> Browse(string name, int Depth = 1)
        {
            return tagTable.TryGetValue(TagConfig.TagStringToSearch(name), out TagDetail tagDetail)
                ? new ValidationResult<TagDetail>(true, tagDetail)
                : new ValidationResult<TagDetail>(false, unSuccesfullText: "Tag {0} not found", formatObjects: name);
        }

        public ValidationResult<object> ExecutePackTagCommand(string name, params object[] args)
        {
            if (tagTable.TryGetValue(TagConfig.TagStringToSearch(name), out TagDetail tagDetail))
            {
                var queue = GetTagArrayIndexes(name);
                if (!queue.Success) return new ValidationResult<object>(false, unSuccesfullText: "Array number parsing failure: {0}", formatObjects: queue.FailString());
                return tagDetail.Execute(queue.Object, args);
            }
            else
            {
                return new ValidationResult<object>(false, unSuccesfullText: "Tag {0} not found", formatObjects: name);
            }
        }

        public ValidationResult<T> GetTagData<T>(string name)
        {
            if (tagTable.TryGetValue(
                TagConfig.TagStringToSearch(name),
                out TagDetail tagDetails))
            {
                var queue = GetTagArrayIndexes(name);
                if (!queue.Success) return new ValidationResult<T>(false, unSuccesfullText: "Array number parsing failure: {0}", formatObjects: queue.FailString());
                return tagDetails.GetValue<T>(queue.Object);
            }

            return new ValidationResult<T>(false, unSuccesfullText: "Tag {0} not found", formatObjects: name);
        }

        public ValidationResult SetTagData<T>(string name, T data)
        {
            if (tagTable.TryGetValue(
                TagConfig.TagStringToSearch(name),
                out TagDetail tagDetails))
            {
                var queue = GetTagArrayIndexes(name);
                if (!queue.Success) return new ValidationResult<object>(false, unSuccesfullText: "Array number parsing failure: {0}", formatObjects: queue.FailString());
                return tagDetails.SetValue(queue.Object, data);
            }
            return new ValidationResult<object>(false, unSuccesfullText: "Tag {0} not found", formatObjects: name);
        }
    }
}