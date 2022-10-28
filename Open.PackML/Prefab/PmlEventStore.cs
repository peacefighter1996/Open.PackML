using Autabee.Utility;
using Open.PackML;
using System;
using System.Collections.Generic;
using static System.Collections.Specialized.BitVector32;

namespace Open.PackML.Prefab
{
    public class PmlEventStore: Dictionary<Enum, PmlEventReaction>, IPmlEventStore
    {
        public PmlEventStore() { }
        public PmlEventStore(List<PmlEventReaction> eventReactions)
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

        public PmlEventReaction GetMachineEvent(Enum @event)
        {
            if (this.TryGetValue(@event, out PmlEventReaction value))
            {
                return value;
            }
            else
            {
                return null;
            }
        }
        public void Add(PmlEventReaction pmlEventReaction)
        {
            this.Add(pmlEventReaction.MachineEventId, pmlEventReaction);
        }

    }
}
