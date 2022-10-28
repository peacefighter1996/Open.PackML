using System;

namespace Open.PackML.EventArguments
{
    public class PmlMachineEventArgs
    {
        public Enum @enum { get; set; }
        public DateTime DateTime { get; set; } = DateTime.UtcNow;
    }
}