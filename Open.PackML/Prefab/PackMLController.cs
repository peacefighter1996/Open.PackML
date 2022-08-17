using System;
using System.Threading.Tasks;

namespace Open.PackML.Prefab
{
    public class PackMLController<T> : PackMLAdapter<T> where T : Enum
    {
        public PackMLController(IPackMLController<T> controller, IPackMLEventStore<T> packMLEventStore) : base(controller, packMLEventStore)
        {

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
            var temp = PmlTransitionCheck.CheckModeUpdate(currentMode, packMLMode, currentState);
            if (temp.Success)
            {
                currentMode = packMLMode;
            }
            return temp;
        }

        public override async Task<ValidationResult> SendPackMLModeAsync(Mode packMLMode)
        {

            return await base.SendPackMLModeAsync(packMLMode);
        }


        private ValidationResult DesidedSendPackMLCommand(Command packMLCommand)
        {
            return SyncDesissions.SyncDesider(controllerPreferAsync,
                delegate { return base.SendPackMLCommand(packMLCommand); },
                async delegate
                {
                    return await base.SendPackMLCommandAsync(packMLCommand);
                });
        }


        private ValidationResult UpdateControlState(Command packMLCommand)
        {
            if (PmlTransitionCheck.CheckTransition(packMLCommand, currentState, currentMode))
            {
                return DesidedSendPackMLCommand(packMLCommand);
            }
            return new ValidationResult(false,
                    string.Format("Current State of machine ({0}) has not transition with command {1}.", currentState, packMLCommand));
        }
    }
}
