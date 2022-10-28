using Open.PackML.EventArguments;
using System;

namespace Open.PackML
{
    public interface IPmlMachineController
    {
        //Events 
        event EventHandler<PmlStateChangeEventArg> UpdateCurrentState;
        event EventHandler<PmlMachineEventArgs> MachineEvent;
    }
}
