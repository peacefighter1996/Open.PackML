using System;

namespace Open.PackML
{
    public class PmlStateChangeEventArg : EventArgs
    {
        public State CurrentState { get; set; }
        public Mode CurrentMode { get; set; }
        public DateTime DateTime { get; set; } = DateTime.UtcNow;
    }
}