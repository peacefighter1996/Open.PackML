using System;
using System.Collections.Generic;
using System.Text;

namespace Open.PackML
{
    public class EventReaction
    {
        public EventReaction(int machineEventId, State stateChangeId)
        {
            MachineEventId = machineEventId;
            StateChangeId = stateChangeId;
        }

        public int MachineEventId { get; }
        public State StateChangeId { get; }

    }
}
