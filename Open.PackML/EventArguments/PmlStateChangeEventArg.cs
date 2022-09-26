using System;

namespace Open.PackML.EventArguments
{
    public class PmlStateChangeEventArg : EventArgs
    {
        public PmlState CurrentState { get; set; }
        public PmlMode CurrentMode { get; set; }
        public DateTime DateTime { get; set; } = DateTime.UtcNow;
    }
}