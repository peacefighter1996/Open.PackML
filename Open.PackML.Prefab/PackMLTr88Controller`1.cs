using Autabee.Utility;
using Open.PackML.EventArguments;
using Open.PackML.Interfaces;
using Open.PackML.Tags;
using Open.PackML.Tags.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Open.PackML.Prefab
{
    public class PmlTr88Controller<T> : IPmlController<T> where T : Enum
    {

        protected PmlState currentState = PmlState.Undefined;
        protected PmlMode currentMode = PmlMode.Undefined;
        protected DateTime lastStateUpdate = DateTime.MinValue.ToUniversalTime();
        protected DateTime lastTransition = DateTime.MinValue.ToUniversalTime();
        protected IPmlController<T> controller;
        protected IPmlEventStore<T> eventStore;

        public event EventHandler<PmlStateChangeEventArg> UpdateCurrentState;
        public event EventHandler<MachineEventArgs<T>> MachineEvent;

        public PmlTr88Controller(IPmlController<T> controller, IPmlEventStore<T> eventStore)
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
        public async Task<ValidationResult> UpdatePackMLModeAsync(PmlMode packMLMode)
        {
            var temp = PmlTransitionCheck.CheckModeUpdate(currentMode, packMLMode, currentState);
            if (temp.Success) temp.AddResult(await controller.UpdatePackMLModeAsync(packMLMode).ConfigureAwait(true));

            return temp;
        }

        public ValidationResult SendPmlCommand(PmlCommand command)
        {
            var temp = PmlTransitionCheck.CheckTransition(command, currentState, currentMode);
            if (temp.Success) temp.AddResult(controller.SendPmlCommand(command));

            return temp;
        }

        public async Task<ValidationResult> SendPackMLCommandAsync(PmlCommand command)
        {
            var temp = PmlTransitionCheck.CheckTransition(command, currentState, currentMode);
            if (temp.Success) temp.AddResult(await controller.SendPackMLCommandAsync(command).ConfigureAwait(true));

            return temp;
        }

        private void Controller_MachineEvent(object sender, MachineEventArgs<T> e)
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
        public PmlState RetrieveCurrentPackMLState()
        {
            currentState = controller.RetrieveCurrentPackMLState();
            return currentState;
        }
        public PmlMode RetrieveCurrentPackMLMode()
        {
            currentMode = controller.RetrieveCurrentPackMLMode();
            return currentMode;
        }
        public async Task<PmlState> RetrieveCurrentPackMLStateAsync()
        {
            currentState = await controller.RetrieveCurrentPackMLStateAsync().ConfigureAwait(true);
            return currentState;
        }

        public async Task<PmlMode> RetrieveCurrentPackMLModeAsync()
        {
            currentMode = await controller.RetrieveCurrentPackMLModeAsync().ConfigureAwait(true);
            return currentMode;
        }

        [TagEndUserTerm("OEE.Bad count")]
        [TagType(TagType.Admin)]
        public int ProdDefectiveCount { get; protected set; }
        [TagEndUserTerm("OEE.Bad count")]
        [TagType(TagType.Admin)]
        public int ProdProcessedCount { get; protected set; }

        [TagEndUserTerm("Stop Reason")]
        public PmlStopReason StopReason { get; protected set; } = new PmlStopReason();

    }
}
