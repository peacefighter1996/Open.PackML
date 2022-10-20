using Autabee.Utility;
using Open.PackML.Tags;
using System.Collections.Generic;

namespace Open.PackML.Interfaces
{
    public interface ITagStore
    {
        ValidationResult<TagConfig[]> Browse(string Orgin);
        ValidationResult<TagConfig[]> Browse(string Orgin, int Depth = 1);
    }
}