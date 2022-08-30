using System;

namespace Open.PackML
{
    public class PmlWarning
    {
        public Enum Id { get; }
        public PmlWarning(Enum Id)
        {
            this.Id = Id;
        }
    }
}
