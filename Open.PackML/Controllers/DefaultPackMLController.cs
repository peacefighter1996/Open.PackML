using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Open.PackML
{
    public class DefaultPackMLController : DefaultPackMLAdapter, IMachineView
    {
        protected IPackMLEventStore packMLEventStore;
        private DateTime lastTransition = DateTime.MinValue;
        public DefaultPackMLController(IPackMLController controller, IPackMLView[] packMLViews, IPackMLEventStore packMLEventStore) : base(controller, packMLViews)
        {
            this.packMLEventStore = packMLEventStore;
        }

        public void MachineEvent(int Id, DateTime dateTime)
        {
            var result = packMLEventStore.ProcessEvent(Id);
            if (result.success)
            {
                if (lastTransition < dateTime)
                {
                    currentState = result.Object;
                    lastTransition = dateTime;
                }
                base.UpdateCurrentState(result.Object, dateTime);
            }
            else
            {
                Console.WriteLine($"Event Id: {Id} Does not have any state resolves");
            }
        }
       
        public override ValidationResult SendPackMLCommand(Command packMLCommand)
        {
            return UpdateControlState(packMLCommand);
        }

        public override async Task<ValidationResult> SendPackMLCommandAsync(Command packMLCommand)
        {
            return await Task.Run(delegate { return UpdateControlState(packMLCommand); });
        }

        public override ValidationResult SendPackMLMode(Mode packMLMode)
        {
            var temp = CheckModeTransition(packMLMode);
            if (temp.success)
            {
                return base.SendPackMLMode(packMLMode);
            }
            return temp;
        }

        private ValidationResult CheckModeTransition(Mode packMLMode)
        {
            if (currentMode == packMLMode)
            {
                return new ValidationResult(false, $"State already {packMLMode}");
            }
            if (currentState == State.Aborted
                || currentState == State.Idle
                || currentState == State.Stopped)
            {
                return new ValidationResult(true);
            }
            else
            {
                return new ValidationResult(false,
                    "Current State of machine not applicable for a mode transition.\nState needs to be Idle, Stopped or Aborted");
            }
        }

        public override async Task<ValidationResult> SendPackMLModeAsync(Mode packMLMode)
        {

            return await base.SendPackMLModeAsync(packMLMode);
        }


        private ValidationResult DesidedSendPackMLCommand(Command packMLCommand)
        {
            return SyncDesissions.SyncDesider(controllerPreferAsync,
                delegate { return base.SendPackMLCommand(packMLCommand); },
                delegate { 
                    return base.SendPackMLCommandAsync(packMLCommand); 
                });
        }


        private ValidationResult UpdateControlState(Command packMLCommand)
        {
            switch (packMLCommand)
            {
                case Command.Abort:
                    if (TransitionCheck.Abort(currentState))
                    {
                        return DesidedSendPackMLCommand(packMLCommand);
                    }
                    break;
                case Command.Clear:
                    if (TransitionCheck.Clear(currentState))
                    {
                        return DesidedSendPackMLCommand(packMLCommand);
                    }
                    break;
                case Command.Stop:
                    if (TransitionCheck.Stop(currentState))
                    {
                        return DesidedSendPackMLCommand(packMLCommand);
                    }
                    break;
                case Command.Reset:
                    if (TransitionCheck.Reset(currentState))
                    {
                        return DesidedSendPackMLCommand(packMLCommand);
                    }
                    break;
                case Command.Start:
                    if (TransitionCheck.Start(currentState))
                    {
                        return DesidedSendPackMLCommand(packMLCommand);
                    }
                    break;
                case Command.Hold:
                    if (TransitionCheck.Hold(currentState))
                    {
                        return DesidedSendPackMLCommand(packMLCommand);
                    }
                    break;
                case Command.Unhold:
                    if (TransitionCheck.UnHold(currentState))
                    {
                        return DesidedSendPackMLCommand(packMLCommand);
                    }
                    break;
                case Command.Suspend:
                    if (TransitionCheck.Suspend(currentState))
                    {
                        return DesidedSendPackMLCommand(packMLCommand);
                    }
                    break;
                case Command.UnSuspend:
                    if (TransitionCheck.UnSuspend(currentState))
                    {
                        return DesidedSendPackMLCommand(packMLCommand);
                    }
                    break;
            }
            return new ValidationResult(false,
                    $"Current State of machine ({currentState}) has not transition with command {packMLCommand}.");
        }
    }
}
