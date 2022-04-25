using System.Collections;

namespace Open.PackML
{
    public interface IPackMLEventStore : ICollection
    {
        ValidationResult<State> ProcessEvent(int MachineEventId);
        EventReaction GetMachineEvent(int @event);
    }
}