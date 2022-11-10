using Autabee.Utility;
using System;

namespace Open.PackML
{
    public interface IPmlEventStore
    {
        ValidationResult<PmlState> GetMachineEvent(Enum MachineEventId);
    }
}