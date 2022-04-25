using System;
using System.Collections.Generic;
using System.Text;

namespace Open.PackML
{
    public static class StateCheck
    {
        public static bool ProccessingRequest(this State state)
        {
            if(State.Starting <= state && state <= State.Completing)
            {
                return true;
            }
            return false;
        }
    }
}
