using Autabee.Utility;
using System;
using System.Collections.Generic;

namespace Open.PackML.Interfaces
{
    public interface IPmlEventStore<T> : IDictionary<T, PmlEventReaction<T>> where T : Enum
    {
        ValidationResult<PmlState> ProcessEvent(T MachineEventId);
        PmlEventReaction<T> GetMachineEvent(T @event);
    }
}