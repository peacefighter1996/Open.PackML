using System;

namespace Open.PackML
{
    public interface IPackMLView
    {
        void UpdateCurrentState(State currentPackMLState, DateTime dateTime);
        void UpdateCurrentMode(Mode packMLMode);
    }
}