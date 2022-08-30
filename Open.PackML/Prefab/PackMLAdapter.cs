﻿using System;
using System.Threading.Tasks;

namespace Open.PackML.Prefab
{
    public abstract class PackMLAdapter<T> : IPackMLController<T> where T : Enum
    {
        protected PmlState currentState = PmlState.Undefined;
        protected PmlMode currentMode = PmlMode.Undefined;
        protected DateTime lastStateUpdate = DateTime.MinValue.ToUniversalTime();
        protected DateTime lastTransition = DateTime.MinValue.ToUniversalTime();
        protected IPackMLController<T> controller;
        protected IPackMLEventStore<T> packMLEventStore;
        public bool PreferAsync { get; set; } = true;
        protected bool controllerPreferAsync;

        public event EventHandler<PmlStateChangeEventArg> UpdateCurrentState;
        public event EventHandler<MachineEventArguments<T>> MachineEvent;

        public PackMLAdapter(IPackMLController<T> controller, IPackMLEventStore<T> packMLEventStore)
        {
            this.packMLEventStore = packMLEventStore;
            UpdateController(controller);
        }

        public virtual ValidationResult UpdateController(IPackMLController<T> controller)
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
            if (result.Success)
            {
                if (lastTransition < e.DateTime)
                {
                    currentState = result.Object;
                    lastTransition = e.DateTime;
                }
            }
            else if (MachineEvent == null)
            {
                Console.WriteLine(string.Format("Event Id: {0} Does not have any state resolves", e.@enum));
            }
            else
            {
                MachineEvent.BeginInvoke(this, e, null, new { });
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

        public virtual PmlState CurrentPackMLState()
        {
            return currentState;
        }

        public virtual PmlMode CurrentPackMLMode()
        {
            return currentMode;
        }

        public virtual async Task<PmlState> RetrieveCurrentPackMLStateAsync()
        {
            currentState = controllerPreferAsync ? await controller.RetrieveCurrentPackMLStateAsync() : controller.RetrieveCurrentPackMLState();
            return currentState;
        }

        public virtual async Task<PmlMode> RetrieveCurrentPackMLModeAsync()
        {
            currentMode = await controller.RetrieveCurrentPackMLModeAsync();
            return currentMode;
        }

        public virtual async Task<ValidationResult> SendPackMLCommandAsync(PmlCommand packMLCommand)
        {
            return await controller.SendPackMLCommandAsync(packMLCommand);
        }

        public virtual async Task<ValidationResult> SendPackMLModeAsync(PmlMode packMLMode)
        {
            return await controller.SendPackMLModeAsync(packMLMode);
        }

        public virtual PmlState RetrieveCurrentPackMLState()
        {
            currentState = SyncDesissions.SyncDesider(
                 controllerPreferAsync,
                 delegate { return controller.RetrieveCurrentPackMLState(); },
                 delegate { return controller.RetrieveCurrentPackMLStateAsync(); }
                 );

            return currentState;
        }

        public virtual PmlMode RetrieveCurrentPackMLMode()
        {
            currentMode = SyncDesissions.SyncDesider(
                controllerPreferAsync,
                delegate { return controller.RetrieveCurrentPackMLMode(); },
                delegate { return controller.RetrieveCurrentPackMLModeAsync(); }
                );

            return currentMode;
        }

        public virtual ValidationResult SendPackMLCommand(PmlCommand packMLCommand)
        {
            return SyncDesissions.SyncDesider(
                controllerPreferAsync,
                delegate { return controller.SendPackMLCommand(packMLCommand); },
                delegate { return controller.SendPackMLCommandAsync(packMLCommand); }
                );
        }

        public virtual ValidationResult SendPackMLMode(PmlMode packMLMode)
        {
            return SyncDesissions.SyncDesider(
                controllerPreferAsync,
                delegate { return controller.SendPackMLMode(packMLMode); },
                delegate { return controller.SendPackMLModeAsync(packMLMode); }
                );
        }

        public void ExecutePackTagCommand<G>(string name, G data)
        {
            throw new NotImplementedException();
        }

        public Task AsyncExecutePackTagCommand<G>(string name, G data)
        {
            throw new NotImplementedException();
        }

        public void ExecutePackTagCommand<G>(int id, G data)
        {
            throw new NotImplementedException();
        }

        public Task AsyncExecutePackTagCommand<G>(int id, G data)
        {
            throw new NotImplementedException();
        }

        public K ExecutePackTagCommand<G, K>(string name, G data)
        {
            throw new NotImplementedException();
        }

        public Task<K> AsyncExecutePackTagCommand<G, K>(string name, G data)
        {
            throw new NotImplementedException();
        }

        public K ExecutePackTagCommand<G, K>(int id, G data)
        {
            throw new NotImplementedException();
        }

        public Task<K> AsyncExecutePackTagCommand<G, K>(int id, G data)
        {
            throw new NotImplementedException();
        }
    }
}
