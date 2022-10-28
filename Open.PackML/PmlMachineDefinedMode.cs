using System;
using System.Collections.Generic;

namespace Open.PackML
{
    public class PmlMachineDefinedMode
    {
        public PmlMachineDefinedMode(int modeCode, string name, string description,
            Dictionary<PmlState, List<PmlCommand>> AcceptableCommandsAtStates = null,
            List<PmlState> AcceptableModeChangesAtState = null)
        {
            ModeCode = modeCode;
            Name = name;
            Description = description;
            AcceptableCommandsAtState = AcceptableCommandsAtStates ?? new Dictionary<PmlState, List<PmlCommand>>();
            AcceptModeChangeAtState = AcceptableModeChangesAtState ?? new List<PmlState>() {
                PmlState.Idle,
                PmlState.Stopped,
                PmlState.Aborted
            };
        }

        public int ModeCode { get; }
        public string Name { get; }
        public string Description { get; }
        public Dictionary<PmlState, List<PmlCommand>> AcceptableCommandsAtState { get; }
        public List<PmlState> AcceptModeChangeAtState { get; internal set; }
    }
}
