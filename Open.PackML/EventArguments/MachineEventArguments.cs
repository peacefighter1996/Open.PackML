using System;

namespace Open.PackML
{
    public class MachineEventArguments<T> where T : Enum
    {
        public T @enum { get; set; }
        public DateTime DateTime { get; set; } = DateTime.UtcNow;
    }
}