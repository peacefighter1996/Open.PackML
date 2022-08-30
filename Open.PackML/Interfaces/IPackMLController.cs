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
        PmlState CurrentPackMLState();
        PmlMode CurrentPackMLMode();
        bool PreferAsync { get; }
        

        //Deep Synced
        PmlState RetrieveCurrentPackMLState();
        PmlMode RetrieveCurrentPackMLMode();
        ValidationResult SendPackMLCommand(PmlCommand packMLCommand);
        ValidationResult SendPackMLMode(PmlMode packMLMode);

        //Deep Async
        Task<PmlState> RetrieveCurrentPackMLStateAsync();
        Task<PmlMode> RetrieveCurrentPackMLModeAsync();
        Task<ValidationResult> SendPackMLCommandAsync(PmlCommand packMLCommand);
        Task<ValidationResult> SendPackMLModeAsync(PmlMode packMLMode);

        // events 
        event EventHandler<PmlStateChangeEventArg> UpdateCurrentState;
        event EventHandler<MachineEventArguments<T>> MachineEvent;
    }
}
