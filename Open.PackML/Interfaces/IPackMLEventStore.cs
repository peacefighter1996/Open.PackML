using System;
using System.Collections.Generic;

namespace Open.PackML
{
    public interface IPackMLEventStore<T> : IDictionary<T, PmlEventReaction<T>> where T : Enum
    {
        ValidationResult<PmlState> ProcessEvent(T MachineEventId);
        PmlEventReaction<T> GetMachineEvent(T @event);
    }
}