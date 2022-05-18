namespace Open.PackML
{
    public static class TransitionCheck
    {
        public static bool CheckTransition(Command command, State currentState, Mode currentMode)
        {
            switch (command)
            {
                case Command.Abort:
                    return TransitionCheck.Abort(currentState, currentMode);
                case Command.Clear:
                    return TransitionCheck.Clear(currentState, currentMode);
                case Command.Stop:
                    return TransitionCheck.Stop(currentState, currentMode);
                case Command.Reset:
                    return TransitionCheck.Reset(currentState, currentMode);
                case Command.Start:
                    return TransitionCheck.Start(currentState, currentMode);
                case Command.Hold:
                    return TransitionCheck.Hold(currentState, currentMode);
                case Command.Unhold:
                    return TransitionCheck.UnHold(currentState, currentMode);
                case Command.Suspend:
                    return TransitionCheck.Suspend(currentState, currentMode);
                case Command.UnSuspend:
                    return TransitionCheck.UnSuspend(currentState, currentMode);
                default:
                    return false;
            }
        }
        public static bool Stop(State currentPackMLState, PackML.Mode packMLMode = Mode.Production)
        {
            return currentPackMLState > State.Stopping;
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