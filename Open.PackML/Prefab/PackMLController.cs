using System;
using System.Threading.Tasks;

namespace Open.PackML.Prefab
{
    public class PackMLController<T> : PackMLAdapter<T> where T : Enum
    {
        public PackMLController(IPackMLController<T> controller, IPackMLEventStore<T> packMLEventStore) : base(controller, packMLEventStore)
        {

        }

        public override ValidationResult SendPackMLCommand(PmlCommand packMLCommand)
        {
            return UpdateControlState(packMLCommand);
        }

        public override async Task<ValidationResult> SendPackMLCommandAsync(PmlCommand packMLCommand)
        {
            return await Task.Run(delegate { return UpdateControlState(packMLCommand); });
        }

        public override ValidationResult SendPackMLMode(PmlMode packMLMode)
        {
            var temp = CheckModeTransition(packMLMode);
            if (temp.success)
            {
                return base.SendPackMLMode(packMLMode);
            }
            return temp;
        }

        private ValidationResult CheckModeTransition(PmlMode packMLMode)
        {
            if (currentMode == packMLMode)
            {
                return new ValidationResult(false, string.Format("State already {0}", packMLMode));
            }
            if (currentState == PmlState.Aborted
                || currentState == PmlState.Idle
                || currentState == PmlState.Stopped)
            {
                return new ValidationResult(true);
            }
            else
            {
                return new ValidationResult(false,
                    "Current State of machine not applicable for a mode transition.\nState needs to be Idle, Stopped or Aborted");
            }
        }

        public override async Task<ValidationResult> SendPackMLModeAsync(PmlMode packMLMode)
        {

            return await base.SendPackMLModeAsync(packMLMode);
        }


        private ValidationResult DesidedSendPackMLCommand(PmlCommand packMLCommand)
        {
            return SyncDesissions.SyncDesider(controllerPreferAsync,
                delegate { return base.SendPackMLCommand(packMLCommand); },
                delegate
                {
                    return base.SendPackMLCommandAsync(packMLCommand);
                });
        }


        private ValidationResult UpdateControlState(PmlCommand packMLCommand)
        {
            if (TransitionCheck.CheckTransition(packMLCommand, currentState, currentMode))
            {
                return DesidedSendPackMLCommand(packMLCommand);
            }
            return new ValidationResult(false,
                    string.Format("Current State of machine ({0}) has not transition with command {1}.", currentState, packMLCommand));
        }
    }
}
