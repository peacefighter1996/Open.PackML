using Open.PackML.EventArguments;
using System;
using System.Threading.Tasks;

namespace Open.PackML.Interfaces
{
    public interface IPackMLController<T> where T: Enum 
    {
        



        //local
        PmlState CurrentPackMLState();
        PmlMode CurrentPackMLMode();
        bool PreferAsync { get; }
        

        //Deep Synced
        PmlState RetrieveCurrentPackMLState();
        PmlMode RetrieveCurrentPackMLMode();
        ValidationResult SendPackMLCommand(PmlCommand packMLCommand);
        ValidationResult UpdatePackMLMode(PmlMode packMLMode);

        //Deep Async
        Task<PmlState> RetrieveCurrentPackMLStateAsync();
        Task<PmlMode> RetrieveCurrentPackMLModeAsync();
        Task<ValidationResult> SendPackMLCommandAsync(PmlCommand packMLCommand);
        Task<ValidationResult> UpdatePackMLModeAsync(PmlMode packMLMode);

        // events 
        event EventHandler<PmlStateChangeEventArg> UpdateCurrentState;
        event EventHandler<MachineEventArgs<T>> MachineEvent;
    }
}
