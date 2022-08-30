namespace Open.PackML.Prefab
{
    public interface ITagStore
    {
        ValidationResult<TagDetails> Browse(string Orgin);
        ValidationResult<TagDetails> Browse(string Orgin, int Depth = 1);
    }
}