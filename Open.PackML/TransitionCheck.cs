namespace Open.PackML
{
    public static class TransitionCheck
    {
        public static bool Stop(State currentPackMLState, PackML.Mode packMLMode = Mode.Production)
        {
            if (currentPackMLState < State.Stopping)
            {
                return false;
            }
            else
            {
                return true;
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