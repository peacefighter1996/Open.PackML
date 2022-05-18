using System;

namespace Open.PackML
{
    public interface IMachineController<T> : IPackMLController where T : Enum
    {
        event EventHandler<MachineEventArguments<T>> MachineEvent;
    }
}
