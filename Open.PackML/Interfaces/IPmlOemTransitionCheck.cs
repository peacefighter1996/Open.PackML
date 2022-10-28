using Autabee.Utility;

namespace Open.PackML
{

    public interface IPmlOemTransitionCheck
    {
        ValidationResult CheckTransition(PmlCommand PmlCommand, PmlState currentState, int currentMode);
        ValidationResult CheckModeUpdate(int currentMode, int requestedMode, PmlState currentState);
    }
}