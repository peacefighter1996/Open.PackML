using Autabee.Utility;
using Open.PackML.Tags;

namespace Open.PackML.Interfaces
{
    public interface ITagStore
    {
        ValidationResult<TagDetails> Browse(string Orgin);
        ValidationResult<TagDetails> Browse(string Orgin, int Depth = 1);
    }
}