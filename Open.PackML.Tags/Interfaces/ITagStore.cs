using Autabee.Utility;
using Open.PackML.Tags;

namespace Open.PackML.Interfaces
{
    public interface ITagStore
    {
        ValidationResult<TagDetail> Browse(string Orgin);
        ValidationResult<TagDetail> Browse(string Orgin, int Depth = 1);
    }
}