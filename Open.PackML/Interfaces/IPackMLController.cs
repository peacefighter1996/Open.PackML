using System;
using System.Threading.Tasks;

namespace Open.PackML
{
    public interface IPackMLController<T> where T: Enum 
    {
        //PackTag Controls
        void ExecutePackTagCommand<G>(string name, G data);
        Task AsyncExecutePackTagCommand<G>(string name, G data);
        void ExecutePackTagCommand<G>(int id, G data);
        Task AsyncExecutePackTagCommand<G>(int id, G data);

        K ExecutePackTagCommand<G,K>(string name, G data);
        Task<K> AsyncExecutePackTagCommand<G,K>(string name, G data);
        K ExecutePackTagCommand<G,K>(int id, G data);
        Task<K> AsyncExecutePackTagCommand<G,K>(int id, G data);



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
