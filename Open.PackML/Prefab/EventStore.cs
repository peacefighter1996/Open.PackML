using Autabee.Utility;
using Open.PackML.Interfaces;
using System;
using System.Collections.Generic;

namespace Open.PackML.Prefab
{
    public class EventStore: Dictionary<Enum, PmlEventReaction<Enum>>, IPmlEventStore<Enum>
    {
        public EventStore() { }
        public EventStore(List<PmlEventReaction<Enum>> eventReactions)
        {
            foreach (var reaction in eventReactions)
            {
                this.Add(reaction.MachineEventId, reaction);
            }
        }


        public ValidationResult<PmlState> ProcessEvent(Enum @event)
        {
            if (ContainsKey(@event))
            {
                return new ValidationResult<PmlState>(true, this[@event].StateChangeId);
            }
            else
            {
                return new ValidationResult<PmlState>(false, PmlState.Undefined, $"Event {@event.GetType().Name} {@event} not found in event store");
            }
        }

        public PmlEventReaction<Enum> GetMachineEvent(Enum @event)
        {
            if (this.TryGetValue(@event, out PmlEventReaction<Enum> value))
            {
                return value;
            }
            else
            {
                return null;
            }
        }

    }
}
