using System;

namespace Open.PackML.EventArguments
{
    public class MachineEventArgs<T>: EventArgs where T : Enum
    {
        public T @enum { get; set; }
        public DateTime DateTime { get; set; } = DateTime.UtcNow;
    }
}