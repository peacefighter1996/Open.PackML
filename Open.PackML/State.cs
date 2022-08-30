namespace Open.PackML
{
    public enum PmlState
    {
        Undefined = 0,
        Aborting = 1,
        Aborted = 2,
        Clearing = 3,
        Stopping = 4,
        Stopped = 5,
        Resetting = 6,
        Idle = 7,
        Starting = 8,
        Execute = 9,
        Held = 10,
        Holding = 11,
        UnHolding = 12,
        Suspending = 13,
        Suspended = 14,
        UnSuspending = 15,
        Completing = 16,
        Completed = 17,
    }
}