using Autabee.Utility;
using Open.PackML.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using Open.PackML.Tags;
using Open.PackML.Tags.Builders;

namespace Open.PackML.Prefab
{
    //public class TagStore<T> : ITagStore where T : Enum
    //{
    //    Dictionary<string, TagDetail> valuePairs;
    //    public TagStore(Dictionary<string, object> StoredObjects)
    //    {
    //        valuePairs = new Dictionary<string, TagDetail>();
    //        foreach (var StoredObject in StoredObjects)
    //        {
    //            valuePairs.Add(StoredObject.Key, TagTreeBuilder.GetTree(StoredObject.Key, StoredObject.Value));
    //        }
    //    }

    //    public ValidationResult<TagDetail> Browse(string Orgin)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public ValidationResult<TagDetail> Browse(string Orgin, int Depth = 1)
    //    {
    //        throw new NotImplementedException();
    //    }
    //}
}
