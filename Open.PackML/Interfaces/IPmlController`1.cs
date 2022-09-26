using Autabee.Utility;
using Open.PackML.EventArguments;
using System;
using System.Threading.Tasks;

namespace Open.PackML.Interfaces
{
    public interface IPmlController<T>: IPmlMachineController<T> where T : Enum
    {
        //local
        PmlState CurrentPmlState();
        PmlMode CurrentPmlMode();

        //Deep Synced
        PmlState RetrieveCurrentPackMLState();
        PmlMode RetrieveCurrentPackMLMode();

        //Deep Async
        Task<PmlState> RetrieveCurrentPackMLStateAsync();
        Task<PmlMode> RetrieveCurrentPackMLModeAsync();

        //Send PackML Function
        ValidationResult SendPmlCommand(PmlCommand packMLCommand);
        ValidationResult UpdatePmlMode(PmlMode packMLMode);
        Task<ValidationResult> SendPackMLCommandAsync(PmlCommand packMLCommand);
        Task<ValidationResult> UpdatePackMLModeAsync(PmlMode packMLMode);
    }
}
