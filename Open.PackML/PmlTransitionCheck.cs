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
                    break;
                case State.Clearing:
                    return false;
                    break;
                case State.Stopped:
                    return true;
                    break;
                case State.Starting:
                    return true;
                    break;
                case State.Idle:
                    return true;
                    break;
                case State.Suspended:
                    return true;
                    break;
                case State.Execute:
                    return true;
                    break;
                case State.Stopping:
                    return false;
                    break;
                case State.Aborting:
                    return false;
                    break;
                case State.Aborted:
                    return false;
                    break;
                case State.Holding:
                    return true;
                    break;
                case State.Held:
                    return true;
                    break;
                case State.UnHolding:
                    return true;
                    break;
                case State.Suspending:
                    return true;
                    break;
                case State.UnSuspending:
                    return true;
                    break;
                case State.Resetting:
                    return true;
                    break;
                case State.Completing:
                    return true;
                    break;
                case State.Completed:
                    return true;
                    break;
                default:
                    return false;
                    break;
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
    }
}