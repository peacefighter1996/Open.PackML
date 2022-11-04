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
        PmlState RetrieveCurrentPmlState();
        PmlMode RetrieveCurrentPmlMode();

        //Deep Async
        Task<PmlState> RetrieveCurrentPmlStateAsync(CancellationToken cancellationToken);
        Task<PmlMode> RetrieveCurrentPmlModeAsync(CancellationToken cancellationToken);

        //Send PackML Function
        ValidationResult SendPmlCommand(PmlCommand packMLCommand);
        ValidationResult UpdatePmlMode(PmlMode packMLMode);
        ValidationResult UpdatePmlMode(int packMLMode);
        Task<ValidationResult> SendPmlCommandAsync(PmlCommand packMLCommand, CancellationToken cancellationToken);
        Task<ValidationResult> UpdatePmlModeAsync(PmlMode packMLMode, CancellationToken cancellationToken);
        Task<ValidationResult> UpdatePmlModeAsync(int packMLMode, CancellationToken cancellationToken);
    }
}
