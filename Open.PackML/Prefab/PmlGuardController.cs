using Autabee.Utility;
using Open.PackML.EventArguments;
using Open.PackML;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Threading;

namespace Open.PackML.Prefab
{
    public class PmlGuardController : IPmlController 
    {

        protected PmlState currentState = PmlState.Undefined;
        protected PmlMode currentMode = PmlMode.Undefined;
        protected DateTime lastStateUpdate = DateTime.MinValue.ToUniversalTime();
        protected DateTime lastTransition = DateTime.MinValue.ToUniversalTime();
        protected IPmlController controller;
        protected IPmlEventStore eventStore;

        public event EventHandler<PmlStateChangeEventArg> UpdateCurrentState;
        public event EventHandler<PmlMachineEventArgs> MachineEvent;

        public PmlGuardController(IPmlController controller, IPmlEventStore eventStore)
        {
            //Guard Check null
            List<(string, string)> argumentsNull = new List<(string, string)>();
            if (controller == null)
            {
                argumentsNull.Add((nameof(controller), $"{nameof(controller)} is null."));
            }
            if (eventStore == null)
            {
                argumentsNull.Add((nameof(eventStore), $"{nameof(eventStore)} is null."));
            }
            if (argumentsNull.Count > 0)
            {
                throw new ArgumentNullException(string.Join(", ", argumentsNull.Select(x => x.Item1)));
            }


            this.eventStore = eventStore;
            this.controller = controller;

            controller.MachineEvent += Controller_MachineEvent;
            controller.UpdateCurrentState += Controller_UpdateCurrentState;

            currentState = controller.CurrentPmlState();
            currentMode = controller.CurrentPmlMode();
        }
        protected void Controller_UpdateCurrentState(object sender, PmlStateChangeEventArg currentPackMLState)
        {
            if (lastStateUpdate <= currentPackMLState.DateTime.ToUniversalTime())
            {
                lastStateUpdate = currentPackMLState.DateTime.ToUniversalTime();
                currentState = currentPackMLState.CurrentState;
                currentMode = currentPackMLState.CurrentMode;

                UpdateCurrentState?.Invoke(this, currentPackMLState);
            }
        }

        public ValidationResult UpdatePmlMode(PmlMode packMLMode)
        {
            var temp = PmlTransitionCheck.CheckModeUpdate(currentMode, packMLMode, currentState);
            if (temp.Success) temp.AddResult(controller.UpdatePmlMode(packMLMode));
            return temp;
        }
        public async Task<ValidationResult> UpdatePmlModeAsync(PmlMode packMLMode, CancellationToken cancellationToken)
        {
            var temp = PmlTransitionCheck.CheckModeUpdate(currentMode, packMLMode, currentState);
            return await TrySendPmlMode(packMLMode, temp, cancellationToken);
        }

        private async Task<ValidationResult> TrySendPmlMode(PmlMode packMLMode, ValidationResult temp, CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested) return new ValidationResult(false, "Operation cancelled");
            if (temp.Success) temp.AddResult(await controller.UpdatePmlModeAsync(packMLMode, cancellationToken).ConfigureAwait(true));
            return temp;
        }

        public virtual ValidationResult SendPmlCommand(PmlCommand command)
        {
            var temp = PmlTransitionCheck.CheckTransition(command, currentState, currentMode);
            if (temp.Success) temp.AddResult(controller.SendPmlCommand(command));
            return temp;
        }

        public virtual async Task<ValidationResult> SendPmlCommandAsync(PmlCommand command, CancellationToken cancellationToken)
        {
            var temp = PmlTransitionCheck.CheckTransition(command, currentState, currentMode);
            return await TrySendCommandAsync(command, temp, cancellationToken);
        }

        internal async Task<ValidationResult> TrySendCommandAsync(PmlCommand command, ValidationResult temp, CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested) return new ValidationResult(false, "Operation cancelled");
            if (temp.Success) temp.AddResult(await controller.SendPmlCommandAsync(command, cancellationToken).ConfigureAwait(true));
            return temp;
        }

        private void Controller_MachineEvent(object sender, PmlMachineEventArgs e)
        {
            //Prcesses the event 
            var result = eventStore.ProcessEvent(e.@enum);

            if (result.Success && (lastTransition < e.DateTime))
            {
                currentState = result.Object;
                lastTransition = e.DateTime;
                UpdateCurrentState?.Invoke(
                    this,
                    new PmlStateChangeEventArg
                    {
                        CurrentMode = currentMode,
                        CurrentState = currentState
                    });
            }

            //pass through event for subsequent event handelings.
            MachineEvent?.Invoke(this, e);
        }

        public PmlState CurrentPmlState() => currentState;
        public PmlMode CurrentPmlMode() => currentMode;
        public PmlState RetrieveCurrentPmlState()
        {
            currentState = controller.RetrieveCurrentPmlState();
            return currentState;
        }
        public PmlMode RetrieveCurrentPmlMode()
        {
            currentMode = controller.RetrieveCurrentPmlMode();
            return currentMode;
        }
        public async Task<PmlState> RetrieveCurrentPmlStateAsync(CancellationToken cancellationToken)
        {
            currentState = await controller.RetrieveCurrentPmlStateAsync(cancellationToken).ConfigureAwait(true);
            return currentState;
        }

        public async Task<PmlMode> RetrieveCurrentPmlModeAsync(CancellationToken cancellationToken)
        {
            currentMode = await controller.RetrieveCurrentPmlModeAsync(cancellationToken).ConfigureAwait(true);
            return currentMode;
        }

        public virtual ValidationResult UpdatePmlMode(int packMLMode)
        {
            if (Enum.IsDefined(typeof(PmlMode), packMLMode))
            {
                return UpdatePmlMode((PmlMode)packMLMode);
            }
            else
            {
                return new ValidationResult(false, $"Invalid {nameof(PmlMode)} value: {packMLMode}");
            }
        }

        public virtual async Task<ValidationResult> UpdatePmlModeAsync(int packMLMode, CancellationToken cancellationToken)
        {
            if (Enum.IsDefined(typeof(PmlMode), packMLMode))
            {
                if (cancellationToken.IsCancellationRequested) return new ValidationResult(false, "Operation cancelled");
                return await UpdatePmlModeAsync((PmlMode)packMLMode, cancellationToken).ConfigureAwait(true);
            }
            else
            {
                return new ValidationResult(false, $"Invalid {nameof(PmlMode)} value: {packMLMode}");
            }
        }
    }
}
