using System;
using System.Collections.ObjectModel;
using System.Text;

namespace Open.PackML
{
    public class EventStore : Collection<EventReaction>, IPackMLEventStore
    {
        public EventReaction GetMachineEvent(int @event)
        {
            for (int i = 0; i < Count; i++)
            {
                if (this[i].MachineEventId == @event)
                {
                    return this[i];
                }
            }

            return new EventReaction(0, State.Undefined);

        }

        public ValidationResult<State> ProcessEvent(int MachineEventId)
        {
            for (int i = 0; i < this.Count; i++)
            {
                if (this[i].MachineEventId == MachineEventId)
                {
                    return new ValidationResult<State>(this[i].StateChangeId, true);
                }
            }
            return new ValidationResult<State>(State.Undefined, false, "No known state transtion");
        }
    }
}
