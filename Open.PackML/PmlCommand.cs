﻿namespace Open.PackML
{
    // Based on [1] 
    public enum PmlCommand
    {
        NoCommand = 0,
        Reset = 1,
        Start = 2,
        Stop = 3,
        Hold = 4,
        UnHold = 5,
        Suspend = 6,
        UnSuspend = 7,
        Abort = 8,
        Clear = 9,
    }
}