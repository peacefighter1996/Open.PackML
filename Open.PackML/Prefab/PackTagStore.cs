using Open.PackML.Interfaces;
using Open.PackML.Tags;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Open.PackML.Prefab
{
    public class PackTagStore : Dictionary<int, TagDetails>, ITagStore
    {
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
