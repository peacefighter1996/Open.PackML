using Autabee.Utility;
using System;
using System.Threading.Tasks;
using System.Threading;

namespace Open.PackML.Prefab
{
    public class PmlOemGuardController : PmlGuardController
    {

        protected new int currentMode = 0;
        private IPmlOemTransitionCheck oemTransitionCheck;

        public PmlOemGuardController(IPmlController controller, IPmlEventStore eventStore, IPmlOemTransitionCheck pmlOemTransitionCheck) : base(controller, eventStore)
        {
            oemTransitionCheck = pmlOemTransitionCheck ?? throw new ArgumentNullException(nameof(pmlOemTransitionCheck));
        }

        public new ValidationResult UpdatePmlMode(int packMLMode)
        {
            var temp = oemTransitionCheck.CheckModeUpdate(currentMode, packMLMode, currentState);
            if (temp.Success) temp.AddResult(controller.UpdatePmlMode(packMLMode));

            return temp;
        }
        public new async Task<ValidationResult> UpdatePackMLModeAsync(int packMLMode, CancellationToken cancellationToken)
        {
            var temp = oemTransitionCheck.CheckModeUpdate(currentMode, packMLMode, currentState);
            if (cancellationToken.IsCancellationRequested) return new ValidationResult(false, "Operation cancelled");
            if (temp.Success) return await controller.UpdatePmlModeAsync(packMLMode, cancellationToken).ConfigureAwait(true);
            return temp;
        }

        public new ValidationResult SendPmlCommand(PmlCommand command)
        {
            var temp = oemTransitionCheck.CheckTransition(command, currentState, currentMode);
            if (temp.Success) temp.AddResult(controller.SendPmlCommand(command));
            return temp;
        }

        public new async Task<ValidationResult> SendPackMLCommandAsync(PmlCommand command, CancellationToken cancellationToken)
        {
            var temp = oemTransitionCheck.CheckTransition(command, currentState, currentMode);
            if (cancellationToken.IsCancellationRequested) return new ValidationResult(false, "Operation cancelled");
            if (temp.Success) return await controller.SendPmlCommandAsync(command, cancellationToken).ConfigureAwait(true);
            return temp;
        }

    }
}
