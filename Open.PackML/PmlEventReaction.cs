using System;

namespace Open.PackML
{
    public class PmlEventReaction<T> where T : Enum
    {
        public PmlEventReaction(T machineEventId, PmlState stateChangeId)
        {
            MachineEventId = machineEventId;
            StateChangeId = stateChangeId;
        }

        public T MachineEventId { get; }
        public PmlState StateChangeId { get; }

    }
}
