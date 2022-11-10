using Autabee.Utility;
using Open.PackML.Interfaces;
using Open.PackML.Tags.Builders;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Open.PackML.Tags.Prefab
{

    public class TagController : ITagController, ITagStore
    {
        TagTable tagTable;

        //public TagController(IEnumerable<TagDetail> tagDetails)
        //{
        //    tagTable = TagTableBuilder.BuildTable(tagDetails);
        //}

        //public TagController(TagTable tagTable)
        //{
        //    this.tagTable = tagTable;
        //}

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

        //public async Task<ValidationResult<object>> AsyncExecutePackTagCommand(string name, params object[] args)
        //{
        //    if (tagTable.TryGetValue(TagConfig.TagStringToSearch(name), out TagDetail tagDetail))
        //    {
        //        var queue = GetTagArrayIndexes(name);
        //        if (!queue.Success) return new ValidationResult<object>(false, unSuccesfullText: "Array number parsing failure: {0}", formatObjects: queue.FailString());
        //        return await tagDetail.ExecuteAsync(queue.Object, args).ConfigureAwait(true);
        //    }
        //    else return new ValidationResult<object>(false, unSuccesfullText: "Tag {0} not found", formatObjects: name);
        //}

        public ValidationResult<TagConfig[]> BrowseAll()
        {
            return new ValidationResult<TagConfig[]>(true, tagTable.GetTags);
        }
        public ValidationResult<TagConfig[]> BrowseRoot()
        {
            return new ValidationResult<TagConfig[]>(true, tagTable.GetRoots);
        }
        public ValidationResult<TagConfig[]> Browse(string name)
        {
            if (!tagTable.TryGetValue(TagConfig.TagStringToSearch(name), out TagConfig tagDetail))
                return new ValidationResult<TagConfig[]>(false, unSuccesfullText: "Tag {0} not found", formatObjects: name);
            return new ValidationResult<TagConfig[]>(true, ((TagDetail)tagDetail).ChildTags);
        }

        public ValidationResult<TagConfig[]> Browse(string name, uint Depth = 1)
        {
            if (!tagTable.TryGetValue(TagConfig.TagStringToSearch(name), out TagConfig tagDetail))
                return new ValidationResult<TagConfig[]>(false, unSuccesfullText: "Tag {0} not found", formatObjects: name);
            var result = GetChildren(((TagDetail)tagDetail), Depth).ToArray();


            return new ValidationResult<TagConfig[]>(true, result);
        }
        internal static IEnumerable<TagConfig> GetChildren(TagDetail tagDetail, uint Depth)
        {
            if (Depth <= 1)
                return tagDetail.ChildTags;

            var childeren = new List<TagConfig>();
            foreach (var child in tagDetail.ChildTags)
            {
                childeren.AddRange(GetChildren(child, Depth - 1));

            }
            childeren.AddRange(tagDetail.ChildTags);
            return childeren;
        }

        public ValidationResult<object> ExecutePackTagCommand(string name, params object[] args)
        {
            if (tagTable.TryGetValue(TagConfig.TagStringToSearch(name), out TagConfig tagDetail))
            {
                var queue = GetTagArrayIndexes(name);
                if (!queue.Success) return new ValidationResult<object>(false, unSuccesfullText: "Array number parsing failure: {0}", formatObjects: queue.FailString());
                return ((TagDetail)tagDetail).Execute(queue.Object, args);
            }
            else
            {
                return new ValidationResult<object>(false, unSuccesfullText: "Tag {0} not found", formatObjects: name);
            }
        }

        public ValidationResult<T> GetTagData<T>(string name)
        {
            var result = GetTagData(name);
            if (result.Success) return new ValidationResult<T>(true, (T)result.Object);
            else return new ValidationResult<T>(false, default, result.FailInfo);
        }

        public ValidationResult<object> GetTagData(string name)
        {
            if (tagTable.TryGetValue(
                TagConfig.TagStringToSearch(name),
                out TagConfig tagDetails))
            {
                var queue = GetTagArrayIndexes(name);
                if (!queue.Success) return new ValidationResult<object>(false, unSuccesfullText: "Array number parsing failure: {0}", formatObjects: queue.FailString());
                return ((TagDetail)tagDetails).GetValue(queue.Object);
            }

            return new ValidationResult<object>(false, unSuccesfullText: "Tag {0} not found", formatObjects: name);
        }

        public ValidationResult SetTagData<T>(string name, T data)
        {
            return SetTagData(name, (object)data);
        }

        public ValidationResult SetTagData(string name, object data)
        {
            if (tagTable.TryGetValue(
                TagConfig.TagStringToSearch(name),
                out TagConfig tagDetails))
            {
                var queue = GetTagArrayIndexes(name);
                if (!queue.Success) return new ValidationResult<object>(false, unSuccesfullText: "Array number parsing failure: {0}", formatObjects: queue.FailString());
                return ((TagDetail)tagDetails).SetValue(queue.Object, data);
            }
            return new ValidationResult<object>(false, unSuccesfullText: "Tag {0} not found", formatObjects: name);
        }

        public ValidationResult[] BatchSetTagData(List<(string, object)> data)
        {
            return data.AsParallel().Select(d => SetTagData(d.Item1, d.Item2)).ToArray();
        }

        public ValidationResult<object>[] BatchGetTagData(List<string> data)
        {
            return data.AsParallel().Select(d => GetTagData(d)).ToArray();
        }

        public ValidationResult<object>[] ExecutePackTagCommand(List<(string, object[])> data)
        {
            return data.AsParallel().Select(d => ExecutePackTagCommand(d.Item1, d.Item2)).ToArray();
        }

        public ValidationResult<object> TagCall(TagCall tagCall)
        {
            switch (tagCall.TagCallType)
            {
                case TagCallType.Get:
                    return GetTagData(tagCall.TagName);
                case TagCallType.Set:
                    return new ValidationResult<object>(SetTagData(tagCall.TagName, tagCall.Data));
                case TagCallType.Execute:
                    return ExecutePackTagCommand(tagCall.TagName, tagCall.Data);
                default:
                    return new ValidationResult<object>(false, unSuccesfullText: "TagCallType {0} not found", formatObjects: tagCall.TagCallType);
            }
        }


        public ValidationResult<object>[] BatchCall(List<TagCall> tagCalls)
        {
            return tagCalls.AsParallel().Select(o => TagCall(o)).ToArray();
        }
    }


}
