using System;

namespace Open.PackML
{
    public class Warning
    {
        public Enum Id { get; }
        public Warning(Enum Id)
        {
            this.Id = Id;
        }
    }
}
