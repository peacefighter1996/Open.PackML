using Autabee.Utility;
using System;
using System.Collections.Generic;

namespace Open.PackML.Prefab
{
    /// <summary>
    /// Collections of machine events to update the state machine
    /// </summary>
    public class PmlEventStore : Dictionary<Enum, PmlEventReaction>, IPmlEventStore
    {
        public PmlEventStore() { }
        public PmlEventStore(List<PmlEventReaction> eventReactions)
        {
            foreach (var reaction in eventReactions)
            {
                this.Add(reaction.MachineEventId, reaction);
            }
        }

        /// <summary>
        /// trys to get the event reaction for the given event id
        /// </summary>
        /// <param name="event"></param>
        /// <returns></returns>
        public ValidationResult<PmlState> GetMachineEvent(Enum @event)
        {
            if (this.TryGetValue(@event, out var value))
            {
                return new ValidationResult<PmlState>(true, value.StateChangeId);
            }
            else
            {
                return new ValidationResult<PmlState>(false, PmlState.Undefined, $"Event {@event.GetType().Name} {@event} not found in event store");
            }
        }

        public void Add(PmlEventReaction pmlEventReaction)
        {
            this.Add(pmlEventReaction.MachineEventId, pmlEventReaction);
        }

    }
}
