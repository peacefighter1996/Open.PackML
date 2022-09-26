using Open.PackML.EventArguments;
using System;

namespace Open.PackML.Interfaces
{
    public interface IPmlMachineController<T> where T : Enum
    {
        //Events 
        event EventHandler<PmlStateChangeEventArg> UpdateCurrentState;
        event EventHandler<MachineEventArgs<T>> MachineEvent;
    }
}
