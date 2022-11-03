using Autabee.Utility;
using System;
using System.Threading;
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
        Task<PmlState> RetrieveCurrentPackMLStateAsync(CancellationToken cancellationToken);
        Task<PmlMode> RetrieveCurrentPackMLModeAsync(CancellationToken cancellationToken);

        //Send PackML Function
        ValidationResult SendPmlCommand(PmlCommand packMLCommand);
        ValidationResult UpdatePmlMode(PmlMode packMLMode);
        ValidationResult UpdatePmlMode(int packMLMode);
        Task<ValidationResult> SendPackMLCommandAsync(PmlCommand packMLCommand, CancellationToken cancellationToken);
        Task<ValidationResult> UpdatePackMLModeAsync(PmlMode packMLMode, CancellationToken cancellationToken);
        Task<ValidationResult> UpdatePackMLModeAsync(int packMLMode, CancellationToken cancellationToken);
    }
}
