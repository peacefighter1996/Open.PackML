using Open.PackML;

namespace Open.PackML
{
    public static class PmlTransitionCheck

    {
        public static bool CheckTransition(Command command, State currentState, Mode currentMode)
        {
            switch (command)
            {
                case Command.Abort:
                    return Abort(currentState, currentMode);
                case Command.Clear:
                    return Clear(currentState, currentMode);
                case Command.Stop:
                    return Stop(currentState, currentMode);
                case Command.Reset:
                    return Reset(currentState, currentMode);
                case Command.Start:
                    return Start(currentState, currentMode);
                case Command.Hold:
                    return Hold(currentState, currentMode);
                case Command.Unhold:
                    return UnHold(currentState, currentMode);
                case Command.Suspend:
                    return Suspend(currentState, currentMode);
                case Command.UnSuspend:
                    return UnSuspend(currentState, currentMode);
                default:
                    return false;
            }
        }
        public static bool Stop(State currentPackMLState, PackML.Mode packMLMode = Mode.Production)
        {
            switch (currentPackMLState)
            {
                case State.Undefined:
                    return false;
                case State.Clearing:
                    return false;
                case State.Stopped:
                    return true;
                case State.Starting:
                    return true;
                case State.Idle:
                    return true;
                case State.Suspended:
                    return true;
                case State.Execute:
                    return true;
                case State.Stopping:
                    return false;
                case State.Aborting:
                    return false;
                case State.Aborted:
                    return false;
                case State.Holding:
                    return true;
                case State.Held:
                    return true;
                case State.UnHolding:
                    return true;
                case State.Suspending:
                    return true;
                case State.UnSuspending:
                    return true;
                case State.Resetting:
                    return true;
                case State.Completing:
                    return true;
                case State.Completed:
                    return true;
                default:
                    return false;
            }
        }

        public static bool Abort(State currentPackMLState, PackML.Mode packMLMode = Mode.Production)
        {
            return true;
        }

        public static bool Reset(State currentPackMLState, PackML.Mode packMLMode = Mode.Production)
        {
            if (currentPackMLState == State.Stopped
                || currentPackMLState == State.Completed)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool Start(State currentPackMLState, PackML.Mode packMLMode = Mode.Production)
        {
            return currentPackMLState == State.Idle;
        }

        public static bool Suspend(State currentPackMLState, PackML.Mode packMLMode = Mode.Production)
        {
            return currentPackMLState == State.Execute && packMLMode == Mode.Production;
        }

        public static bool Hold(State currentPackMLState, PackML.Mode packMLMode = Mode.Production)
        {
            return currentPackMLState == State.Execute && packMLMode == Mode.Production;
        }

        public static bool UnHold(State currentPackMLState, PackML.Mode packMLMode = Mode.Production)
        {
            return currentPackMLState == State.Held;
        }

        public static bool UnSuspend(State currentPackMLState, PackML.Mode packMLMode = Mode.Production)
        {
            return currentPackMLState == State.Suspended;
        }

        public static bool Clear(State currentPackMLState, PackML.Mode packMLMode = Mode.Production)
        {
            return currentPackMLState == State.Aborted;
        }

        public static ValidationResult CheckModeUpdate(Mode currentMode, Mode packMLMode, State currentState)
        {
            if (currentMode == packMLMode)
            {
                return new ValidationResult(false, string.Format("State already {0}", packMLMode));
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

    }
}