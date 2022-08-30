using Open.PackML;

namespace Open.PackML
{
    public static class PmlTransitionCheck

    {
        public static bool CheckTransition(PmlCommand PmlCommand, PmlState currentState, PmlMode currentMode)
        {
            switch (PmlCommand)
            {
                case PmlCommand.Abort:
                    return Abort(currentState, currentMode);
                case PmlCommand.Clear:
                    return Clear(currentState, currentMode);
                case PmlCommand.Stop:
                    return Stop(currentState, currentMode);
                case PmlCommand.Reset:
                    return Reset(currentState, currentMode);
                case PmlCommand.Start:
                    return Start(currentState, currentMode);
                case PmlCommand.Hold:
                    return Hold(currentState, currentMode);
                case PmlCommand.Unhold:
                    return UnHold(currentState, currentMode);
                case PmlCommand.Suspend:
                    return Suspend(currentState, currentMode);
                case PmlCommand.UnSuspend:
                    return UnSuspend(currentState, currentMode);
                default:
                    return false;
            }
        }
        public static bool Stop(PmlState currentPackMLState, PackML.PmlMode packMLMode = PmlMode.Production)
        {
            switch (currentPackMLState)
            {
                case PmlState.Undefined:
                    return false;
                case PmlState.Clearing:
                    return false;
                case PmlState.Stopped:
                    return true;
                case PmlState.Starting:
                    return true;
                case PmlState.Idle:
                    return true;
                case PmlState.Suspended:
                    return true;
                case PmlState.Execute:
                    return true;
                case PmlState.Stopping:
                    return false;
                case PmlState.Aborting:
                    return false;
                case PmlState.Aborted:
                    return false;
                case PmlState.Holding:
                    return true;
                case PmlState.Held:
                    return true;
                case PmlState.UnHolding:
                    return true;
                case PmlState.Suspending:
                    return true;
                case PmlState.UnSuspending:
                    return true;
                case PmlState.Resetting:
                    return true;
                case PmlState.Completing:
                    return true;
                case PmlState.Completed:
                    return true;
                default:
                    return false;
            }
        }

        public static bool Abort(PmlState currentPackMLState, PackML.PmlMode packMLMode = PmlMode.Production)
        {
            return true;
        }

        public static bool Reset(PmlState currentPackMLState, PackML.PmlMode packMLMode = PmlMode.Production)
        {
            if (currentPackMLState == PmlState.Stopped
                || currentPackMLState == PmlState.Completed)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool Start(PmlState currentPackMLState, PackML.PmlMode packMLMode = PmlMode.Production)
        {
            return currentPackMLState == PmlState.Idle;
        }

        public static bool Suspend(PmlState currentPackMLState, PackML.PmlMode packMLMode = PmlMode.Production)
        {
            return currentPackMLState == PmlState.Execute && packMLMode == PmlMode.Production;
        }

        public static bool Hold(PmlState currentPackMLState, PackML.PmlMode packMLMode = PmlMode.Production)
        {
            return currentPackMLState == PmlState.Execute && packMLMode == PmlMode.Production;
        }

        public static bool UnHold(PmlState currentPackMLState, PackML.PmlMode packMLMode = PmlMode.Production)
        {
            return currentPackMLState == PmlState.Held;
        }

        public static bool UnSuspend(PmlState currentPackMLState, PackML.PmlMode packMLMode = PmlMode.Production)
        {
            return currentPackMLState == PmlState.Suspended;
        }

        public static bool Clear(PmlState currentPackMLState, PackML.PmlMode packMLMode = PmlMode.Production)
        {
            return currentPackMLState == PmlState.Aborted;
        }

        public static ValidationResult CheckModeUpdate(PmlMode currentMode, PmlMode packMLMode, PmlState currentState)
        {
            if (currentMode == packMLMode)
            {
                return new ValidationResult(false, string.Format("PmlState already {0}", packMLMode));
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
                    "Current PmlState of machine not applicable for a PmlMode transition.\nState needs to be Idle, Stopped or Aborted");
            }
        }

    }
}