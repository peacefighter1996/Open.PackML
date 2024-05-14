using System;

namespace Open.PackML.Tags
{
    [Flags]
    public enum TagType
    {
        Undefined = 0,
        Status = 1,
        Command = 2,
        // performance information
        Admin = 4
    }
}
