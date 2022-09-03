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
    public class DefaultTagStore<T> : ITagStore where T : Enum
    {
        Dictionary<string, TagDetails> valuePairs;
        public DefaultTagStore(Dictionary<string, IPackMLEventStore<T>> StoredObjects)
        {
            valuePairs = new Dictionary<string, TagDetails>();
            foreach (var StoredObject in StoredObjects)
            {
                valuePairs.Add(StoredObject.Key, BuildTagTree.GetTree(StoredObject.Key, StoredObject.Value));
            }
        }

        public ValidationResult<TagDetails> Browse(string Orgin)
        {
            throw new NotImplementedException();
        }

        public ValidationResult<TagDetails> Browse(string Orgin, int Depth = 1)
        {
            throw new NotImplementedException();
        }
    }
}
