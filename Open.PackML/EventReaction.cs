using System;

namespace Open.PackML
{
    public class EventReaction<T> where T : Enum
    {
        public EventReaction(T machineEventId, PmlState stateChangeId)
        {
            MachineEventId = machineEventId;
            StateChangeId = stateChangeId;
        }

        public T MachineEventId { get; }
        public PmlState StateChangeId { get; }

    }
}
