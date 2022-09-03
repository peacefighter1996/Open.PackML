namespace Open.PackML
{
    public static class TransitionCheck
    {
        public static bool CheckTransition(PmlCommand command, PmlState currentState, PmlMode currentMode)
        {
            switch (command)
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
        public static bool Stop(PmlState currentPackMLState, PmlMode packMLMode = PmlMode.Production)
        {
            return currentPackMLState > PmlState.Stopping;
        }

        public static bool Abort(PmlState currentPackMLState, PmlMode packMLMode = PmlMode.Production)
        {
            return true;
        }

        public static bool Reset(PmlState currentPackMLState, PmlMode packMLMode = PmlMode.Production)
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

        public static bool Start(PmlState currentPackMLState, PmlMode packMLMode = PmlMode.Production)
        {
            return currentPackMLState == PmlState.Idle;
        }

        public static bool Suspend(PmlState currentPackMLState, PmlMode packMLMode = PmlMode.Production)
        {
            return currentPackMLState == PmlState.Execute && packMLMode == PmlMode.Production;
        }

        public static bool Hold(PmlState currentPackMLState, PmlMode packMLMode = PmlMode.Production)
        {
            return currentPackMLState == PmlState.Execute && packMLMode == PmlMode.Production;
        }

        public static bool UnHold(PmlState currentPackMLState, PmlMode packMLMode = PmlMode.Production)
        {
            return currentPackMLState == PmlState.Held;
        }

        public static bool UnSuspend(PmlState currentPackMLState, PmlMode packMLMode = PmlMode.Production)
        {
            return currentPackMLState == PmlState.Suspended;
        }

        public static bool Clear(PmlState currentPackMLState, PmlMode packMLMode = PmlMode.Production)
        {
            return currentPackMLState == PmlState.Aborted;
        }
    }
}