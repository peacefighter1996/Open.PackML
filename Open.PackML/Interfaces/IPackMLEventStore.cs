using System;
using System.Collections.Generic;

namespace Open.PackML
{
    public interface IPackMLEventStore<T> : IDictionary<T, EventReaction<T>> where T : Enum
    {
        ValidationResult<State> ProcessEvent(T MachineEventId);
        EventReaction<T> GetMachineEvent(T @event);
    }
}