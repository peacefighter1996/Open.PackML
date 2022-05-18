namespace Open.PackML
{
    public class EventReaction<T>
    {
        public EventReaction(T machineEventId, State stateChangeId)
        {
            MachineEventId = machineEventId;
            StateChangeId = stateChangeId;
        }

        public T MachineEventId { get; }
        public State StateChangeId { get; }

    }
}
