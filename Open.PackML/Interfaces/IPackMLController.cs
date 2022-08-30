using System;
using System.Threading.Tasks;

namespace Open.PackML
{
    public interface IPackMLController
    {
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
    }
}
