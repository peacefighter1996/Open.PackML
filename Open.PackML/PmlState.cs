namespace Open.PackML
{
    // Based on [1] 
    public enum PmlState : int
    {
        Undefined = 0,
        Clearing = 1,
        Stopped = 2,
        Starting = 3,
        Idle = 4,
        Suspended = 5,
        Execute = 6,
        Stopping = 7,
        Aborting = 8,
        Aborted = 9,
        Holding = 10,
        Held = 11,
        UnHolding = 12,
        Suspending = 13,
        UnSuspending = 14,
        Resetting = 15,
        Completing = 16,
        Completed = 17,
    }
}