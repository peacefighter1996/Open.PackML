using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Open.PackML
{
    public abstract class DefaultPackMLAdapter : IPackMLView, IPackMLController
    {
        protected State currentState;
        protected Mode currentMode;
        protected IPackMLView[] views = new IPackMLView[0];
        protected IPackMLController controller;
        

        public bool PreferAsync => true;
        protected bool controllerPreferAsync;

        public DefaultPackMLAdapter(IPackMLController controller, IPackMLView[] packMLViews)
        {
            UpdateController(controller);
            views = packMLViews;
            UpdateViews();
        }

        public virtual void UpdateViews()
        {
            UpdateCurrentState(currentState, DateTime.UtcNow);
            UpdateCurrentMode(currentMode);
        }

        public virtual ValidationResult UpdateController(IPackMLController controller)
        {
            this.controller = controller;

            currentState = controller.CurrentPackMLState();
            currentMode = controller.CurrentPackMLMode();
            controllerPreferAsync = controller.PreferAsync;

            return new ValidationResult(true);
        }



        public virtual async void UpdateCurrentState(State currentPackMLState, DateTime dateTime)
        {
            await Task.Run(delegate { Parallel.ForEach(views, view => { 
                view.UpdateCurrentState(currentPackMLState, dateTime); 
            }); });
        }

        public virtual async void UpdateCurrentMode(Mode packMLMode)
        {
            await Task.Run(delegate { Parallel.ForEach(views, view => { 
                view.UpdateCurrentMode(packMLMode); 
            }); });
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

        public void AddView(IPackMLView packMLView)
        {
            if (!views.Contains(packMLView))
            {
                var temp = views.ToList();
                temp.Add( packMLView );
                views = temp.ToArray();
            }
        }
    }
}
