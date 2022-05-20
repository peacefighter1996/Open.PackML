using System;
using System.Threading.Tasks;

namespace Open.PackML
{
    public interface IPackMLController<T> where T: Enum 
    {
        //local
        State CurrentPackMLState();
        Mode CurrentPackMLMode();
        bool PreferAsync { get; }
        

        //Deep Synced
        State RetrieveCurrentPackMLState();
        Mode RetrieveCurrentPackMLMode();
        ValidationResult SendPackMLCommand(Command packMLCommand);
        ValidationResult SendPackMLMode(Mode packMLMode);

        //Deep Async
        Task<State> RetrieveCurrentPackMLStateAsync();
        Task<Mode> RetrieveCurrentPackMLModeAsync();
        Task<ValidationResult> SendPackMLCommandAsync(Command packMLCommand);
        Task<ValidationResult> SendPackMLModeAsync(Mode packMLMode);

        // events 
        event EventHandler<PmlStateChangeEventArg> UpdateCurrentState;
        event EventHandler<MachineEventArguments<T>> MachineEvent;
    }
}
