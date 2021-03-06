using System;
using System.Threading.Tasks;

namespace Open.PackML
{
    public abstract class DefaultPackMLAdapter<T> : IPackMLController where T : Enum
    {
        protected State currentState = State.Undefined;
        protected Mode currentMode = Mode.Undefined;
        protected DateTime lastStateUpdate = DateTime.MinValue.ToUniversalTime();
        protected DateTime lastTransition = DateTime.MinValue.ToUniversalTime();
        protected IMachineController<T> controller;
        protected IPackMLEventStore<T> packMLEventStore;
        public bool PreferAsync { get; set; } = true;
        protected bool controllerPreferAsync;

        public event EventHandler<PmlStateChangeEventArg> UpdateCurrentState;

        public DefaultPackMLAdapter(IMachineController<T> controller, IPackMLEventStore<T> packMLEventStore)
        {
            this.packMLEventStore = packMLEventStore;
            UpdateController(controller);
        }

        public virtual ValidationResult UpdateController(IMachineController<T> controller)
        {
            if (this.controller != null)
            {
                this.controller.UpdateCurrentState -= Controller_UpdateCurrentState;
                this.controller.MachineEvent -= Controller_MachineEvent;
            }

            this.controller = controller;
            currentState = controller.CurrentPackMLState();
            currentMode = controller.CurrentPackMLMode();
            controllerPreferAsync = controller.PreferAsync;
            this.controller.UpdateCurrentState += Controller_UpdateCurrentState;
            this.controller.MachineEvent += Controller_MachineEvent;

            return new ValidationResult(true);
        }

        private void Controller_MachineEvent(object sender, MachineEventArguments<T> e)
        {
            var result = packMLEventStore.ProcessEvent(e.@enum);
            if (result.success)
            {
                if (lastTransition < e.DateTime)
                {
                    currentState = result.Object;
                    lastTransition = e.DateTime;

                }

            }
            else
            {
                Console.WriteLine(string.Format("Event Id: {0} Does not have any state resolves", e.@enum));
            }
        }

        protected async void Controller_UpdateCurrentState(object sender, PmlStateChangeEventArg currentPackMLState)
        {
            if (lastStateUpdate <= currentPackMLState.DateTime.ToUniversalTime())
            {
                lastStateUpdate = currentPackMLState.DateTime.ToUniversalTime();
                currentState = currentPackMLState.CurrentState;
                currentMode = currentPackMLState.CurrentMode;


                UpdateCurrentState?.Invoke(this, currentPackMLState);
            }
        }

        public virtual State CurrentPackMLState()
        {
            return currentState;
        }

        public virtual Mode CurrentPackMLMode()
        {
            return currentMode;
        }

        public virtual async Task<State> RetrieveCurrentPackMLStateAsync()
        {
            currentState = controllerPreferAsync ? await controller.RetrieveCurrentPackMLStateAsync() : controller.RetrieveCurrentPackMLState();
            return currentState;
        }

        public virtual async Task<Mode> RetrieveCurrentPackMLModeAsync()
        {
            currentMode = await controller.RetrieveCurrentPackMLModeAsync();
            return currentMode;
        }

        public virtual async Task<ValidationResult> SendPackMLCommandAsync(Command packMLCommand)
        {
            return await controller.SendPackMLCommandAsync(packMLCommand);
        }

        public virtual async Task<ValidationResult> SendPackMLModeAsync(Mode packMLMode)
        {
            return await controller.SendPackMLModeAsync(packMLMode);
        }

        public virtual State RetrieveCurrentPackMLState()
        {
            currentState = SyncDesissions.SyncDesider(
                 controllerPreferAsync,
                 delegate { return controller.RetrieveCurrentPackMLState(); },
                 delegate { return controller.RetrieveCurrentPackMLStateAsync(); }
                 );

            return currentState;
        }

        public virtual Mode RetrieveCurrentPackMLMode()
        {
            currentMode = SyncDesissions.SyncDesider(
                controllerPreferAsync,
                delegate { return controller.RetrieveCurrentPackMLMode(); },
                delegate { return controller.RetrieveCurrentPackMLModeAsync(); }
                );

            return currentMode;
        }

        public virtual ValidationResult SendPackMLCommand(Command packMLCommand)
        {
            return SyncDesissions.SyncDesider(
                controllerPreferAsync,
                delegate { return controller.SendPackMLCommand(packMLCommand); },
                delegate { return controller.SendPackMLCommandAsync(packMLCommand); }
                );
        }

        public virtual ValidationResult SendPackMLMode(Mode packMLMode)
        {
            return SyncDesissions.SyncDesider(
                controllerPreferAsync,
                delegate { return controller.SendPackMLMode(packMLMode); },
                delegate { return controller.SendPackMLModeAsync(packMLMode); }
                );
        }
    }
}
