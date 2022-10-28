using System;

namespace Open.PackML
{
    public class PmlEventReaction
    {
        public PmlEventReaction(Enum machineEventId, PmlState stateChangeId)
        {
            MachineEventId = machineEventId;
            StateChangeId = stateChangeId;
        }

        public Enum MachineEventId { get; }
        public PmlState StateChangeId { get; }

    }
}
