using Autabee.Utility.Messaging.Validation;
using Open.PackML.Tags;

namespace Open.PackML.Interfaces
{
    public interface ITagStore
    {
        ValidationResult<TagConfig[]> BrowseRoot();
        ValidationResult<TagConfig[]> BrowseAll();
        ValidationResult<TagConfig[]> Browse(string baseTagName);
        ValidationResult<TagConfig[]> Browse(string baseTagName, uint Depth);
    }
}