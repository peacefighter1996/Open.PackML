using Autabee.Utility;
using System;
using System.Threading.Tasks;

namespace Open.PackML
{
    public interface IPmlController: IPmlMachineController
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
        ValidationResult UpdatePmlMode(int packMLMode);
        Task<ValidationResult> SendPackMLCommandAsync(PmlCommand packMLCommand);
        Task<ValidationResult> UpdatePackMLModeAsync(PmlMode packMLMode);
        Task<ValidationResult> UpdatePackMLModeAsync(int packMLMode);
    }
}
