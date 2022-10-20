using Autabee.Utility;
using System.Collections.ObjectModel;
using System.Security.Cryptography;

namespace Open.PackML
{
    public static class PmlTransitionCheck
    {
       
        public static ValidationResult CheckTransition(PmlCommand PmlCommand, PmlState currentState, PmlMode currentMode)
        {
            return PmlCommand switch
            {
                PmlCommand.Abort => Abort(currentState, currentMode),
                PmlCommand.Clear => Clear(currentState, currentMode),
                PmlCommand.Stop => Stop(currentState, currentMode),
                PmlCommand.Reset => Reset(currentState, currentMode),
                PmlCommand.Start => Start(currentState, currentMode),
                PmlCommand.Hold => Hold(currentState, currentMode),
                PmlCommand.UnHold => UnHold(currentState, currentMode),
                PmlCommand.Suspend => Suspend(currentState, currentMode),
                PmlCommand.UnSuspend => UnSuspend(currentState, currentMode),
                _ => new ValidationResult(false, "Unsupported command"),
            };
        }
        public static ValidationResult Stop(PmlState currentPackMLState, PmlMode packMLMode = PmlMode.Production)
        {
            if (packMLMode == PmlMode.Undefined) return new ValidationResult(false, "Mode is undefined");
            switch (currentPackMLState)
            {
                case PmlState.Undefined:
                case PmlState.Clearing:
                case PmlState.Stopped:
                case PmlState.Stopping:
                case PmlState.Aborting:
                case PmlState.Aborted:
                    return new ValidationResult(false, "Currently not in a state that accepts Stop.");
                case PmlState.Completed:
                case PmlState.Starting:
                case PmlState.Idle:
                case PmlState.Suspended:
                case PmlState.Execute:
                case PmlState.Holding:
                case PmlState.Held:
                case PmlState.UnHolding:
                case PmlState.Suspending:
                case PmlState.UnSuspending:
                case PmlState.Resetting:
                case PmlState.Completing:
                    return new ValidationResult(true);
                default:
                     return new ValidationResult(false, "Unkown current state");
            }
        }

        public static ValidationResult Abort(PmlState currentPackMLState, PmlMode packMLMode = PmlMode.Production)
        {
            if (currentPackMLState == PmlState.Aborted 
                || currentPackMLState == PmlState.Aborting)
            {
                return new ValidationResult(false, "Already aborted or aborting");
            }
            else
            {
                return new ValidationResult();
            }
        }

        public static ValidationResult Reset(PmlState currentPackMLState, PmlMode packMLMode = PmlMode.Production)
        {
            if (packMLMode == PmlMode.Undefined) return new ValidationResult(false, "Current mode is undefined");
            if (currentPackMLState == PmlState.Stopped
                || currentPackMLState == PmlState.Completed)
            {
                return new ValidationResult();
            }
            else
            {
                return new ValidationResult(false, "Current PackML state not in stopped or completed.");
            }
        }

        public static ValidationResult Start(PmlState currentPackMLState, PmlMode packMLMode = PmlMode.Production)
        {
            if (packMLMode == PmlMode.Undefined) return new ValidationResult(false, "Current mode is undefined");
            return currentPackMLState == PmlState.Idle ? new ValidationResult() : new ValidationResult(false, "Current PackML state not in idle.");
        }

        public static ValidationResult Suspend(PmlState currentPackMLState, PmlMode packMLMode = PmlMode.Production)
        {
            return HeldSuspend(currentPackMLState, packMLMode);
        }

        private static ValidationResult HeldSuspend(PmlState currentPackMLState, PmlMode packMLMode)
        {
            if (packMLMode == PmlMode.Undefined) return new ValidationResult(false, "Current mode is undefined");
            return currentPackMLState == PmlState.Execute
                ? packMLMode == PmlMode.Production
                    ? new ValidationResult()
                    : new ValidationResult(false, "Current PackML mode not in production.")
                : packMLMode == PmlMode.Production
                    ? new ValidationResult(false, "Current PackML state not in execute.")
                    : new ValidationResult(false, "Currently PackML state not in execute and mode not in production.");
        }

        public static ValidationResult Hold(PmlState currentPackMLState, PmlMode packMLMode = PmlMode.Production)
        {
            return HeldSuspend(currentPackMLState, packMLMode);
        }

        public static ValidationResult UnHold(PmlState currentPackMLState, PmlMode packMLMode = PmlMode.Production)
        {
            if (packMLMode == PmlMode.Undefined) return new ValidationResult(false, "Current mode is undefined");
            return currentPackMLState == PmlState.Held
                ? new ValidationResult() 
                : new ValidationResult(false, "Current PackML state not in held.");
        }

        public static ValidationResult UnSuspend(PmlState currentPackMLState, PmlMode packMLMode = PmlMode.Production)
        {
            if (packMLMode == PmlMode.Undefined) return new ValidationResult(false, "Current mode is undefined");
            return currentPackMLState == PmlState.Suspended
                ? new ValidationResult()
                : new ValidationResult(false, "Current PackML state not in suspended.");
        }

        public static ValidationResult Clear(PmlState currentPackMLState, PmlMode packMLMode = PmlMode.Production)
        {
            if (packMLMode == PmlMode.Undefined) return new ValidationResult(false, "Current mode is undefined");
            return currentPackMLState == PmlState.Aborted
                ? new ValidationResult()
                : new ValidationResult(false, "Current PackML state not in aborted.");
        }

        public static ValidationResult CheckModeUpdate(PmlMode currentMode, PmlMode packMLMode, PmlState currentState)
        {
            if (packMLMode == PmlMode.Undefined)
            {
                return new ValidationResult(false, "Not allowed to change mode to undefined mode");
            }
            if (currentMode == packMLMode)
            {
                return new ValidationResult(false, string.Format("PmlState already {0}", packMLMode));
            }
            if (currentState == PmlState.Aborted
                || currentState == PmlState.Idle
                || currentState == PmlState.Stopped)
            {
                return new ValidationResult();
            }
            else
            {
                return new ValidationResult(false,
                    "Current PmlState of machine not applicable for a PmlMode transition.\nState needs to be Idle, Stopped or Aborted");
            }
        }

    }
}