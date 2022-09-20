using System;
using System.Diagnostics.Contracts;

namespace Open.PackML
{
    public class PmlWarning
    {
        public bool Trigger { get; set; }
        public int ID { get; set; }
        public int Value { get; set; }
    }
}
