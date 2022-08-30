namespace Open.PackML
{
    public static class TransitionCheck
    {
        public static bool CheckTransition(PmlCommand command, PmlState currentState, PmlMode currentMode)
        {
            switch (command)
            {
                case PmlCommand.Abort:
                    return TransitionCheck.Abort(currentState, currentMode);
                case PmlCommand.Clear:
                    return TransitionCheck.Clear(currentState, currentMode);
                case PmlCommand.Stop:
                    return TransitionCheck.Stop(currentState, currentMode);
                case PmlCommand.Reset:
                    return TransitionCheck.Reset(currentState, currentMode);
                case PmlCommand.Start:
                    return TransitionCheck.Start(currentState, currentMode);
                case PmlCommand.Hold:
                    return TransitionCheck.Hold(currentState, currentMode);
                case PmlCommand.Unhold:
                    return TransitionCheck.UnHold(currentState, currentMode);
                case PmlCommand.Suspend:
                    return TransitionCheck.Suspend(currentState, currentMode);
                case PmlCommand.UnSuspend:
                    return TransitionCheck.UnSuspend(currentState, currentMode);
                default:
                    return false;
            }
        }
        public static bool Stop(PmlState currentPackMLState, PackML.PmlMode packMLMode = PmlMode.Production)
        {
            return currentPackMLState > PmlState.Stopping;
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
    }
}