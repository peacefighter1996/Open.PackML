using System;
using System.Collections.Generic;

namespace Open.PackML.Controllers
{
    public class DefaultEventStore : Dictionary<Enum, EventReaction<Enum>>, IPackMLEventStore<Enum>
    {
        public DefaultEventStore() { }
        public DefaultEventStore(List<EventReaction<Enum>> eventReactions)
        {
            foreach (var reaction in eventReactions)
            {
                this.Add(reaction.MachineEventId, reaction);
            }
        }


        public ValidationResult<State> ProcessEvent(Enum @event)
        {
            if (ContainsKey(@event))
            {
                return new ValidationResult<State>(this[@event].StateChangeId, true);
            }
            else
            {
                return new ValidationResult<State>(State.Undefined, false, $"Event {@event.GetType().Name} {@event} not found in event store");
            }
        }

        public EventReaction<Enum> GetMachineEvent(Enum @event)
        {
            if (this.ContainsKey(@event))
            {
                return this[@event];
            }
            else
            {
                return null;
            }
        }

    }
}
