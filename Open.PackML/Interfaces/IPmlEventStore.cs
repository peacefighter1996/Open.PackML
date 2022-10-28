using Autabee.Utility;
using System;
using System.Collections.Generic;

namespace Open.PackML
{
    public interface IPmlEventStore
    {
        ValidationResult<PmlState> ProcessEvent(Enum MachineEventId);
        PmlEventReaction GetMachineEvent(Enum @event);
    }
}