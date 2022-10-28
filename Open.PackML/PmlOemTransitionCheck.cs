using Autabee.Utility;
using Open.PackML;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Cryptography;
using System.Xml;

namespace Open.PackML
{
    public class PmlOemTransitionCheck : IPmlOemTransitionCheck
    {
        private Dictionary<int, PmlMachineDefinedMode> MachineDefinedModes { get; } = new Dictionary<int, PmlMachineDefinedMode>();

        public PmlOemTransitionCheck() : base()
        {

        }
        public PmlOemTransitionCheck(IEnumerable<PmlMachineDefinedMode> machineDefinedModes)
        {
            MachineDefinedModes = machineDefinedModes.ToDictionary(o => o.ModeCode);
        }

        public ValidationResult CheckTransition(PmlCommand pmlCommand, PmlState currentState, int currentMode)
        {
            Func<PmlState, PmlMode, ValidationResult> func;
            switch (pmlCommand)
            {
                case PmlCommand.Abort: func = PmlTransitionCheck.Abort; break;
                case PmlCommand.Clear: func = PmlTransitionCheck.Clear; break;
                case PmlCommand.Stop: func = PmlTransitionCheck.Stop; break;
                case PmlCommand.Reset: func = PmlTransitionCheck.Reset; break;
                case PmlCommand.Start: func = PmlTransitionCheck.Start; break;
                case PmlCommand.Hold: func = PmlTransitionCheck.Hold; break;
                case PmlCommand.UnHold: func = PmlTransitionCheck.UnHold; break;
                case PmlCommand.Suspend: func = PmlTransitionCheck.Suspend; break;
                case PmlCommand.UnSuspend: func = PmlTransitionCheck.UnSuspend; break;
                default: return new ValidationResult(false, "Unsupported command");
            };
            return CheckTransion(currentState, currentMode, func, pmlCommand);
        }


        private ValidationResult CheckTransion(PmlState currentPackMLState, int pmlMode, Func<PmlState, PmlMode, ValidationResult> transitionCheck, PmlCommand command)
        {
            if (Enum.IsDefined(typeof(PmlMode), pmlMode))
            {
                return transitionCheck(currentPackMLState, (PmlMode)pmlMode);
            }
            else if (CheckOemMode(currentPackMLState, pmlMode))
            {
                return new ValidationResult(true);
            }
            else
            {
                return new ValidationResult(false, "Currently not in a mode that accepts {0}.", command);
            }
        }

        private bool CheckOemMode(PmlState currentPackMLState, int pmlMode)
        {
            return MachineDefinedModes.TryGetValue(pmlMode, out PmlMachineDefinedMode machineDefinedMode)
                            && machineDefinedMode.AcceptableCommandsAtState.TryGetValue(currentPackMLState, out List<PmlCommand> acceptableCommands)
                            && acceptableCommands.Contains(PmlCommand.Abort);
        }

        public ValidationResult CheckModeUpdate(int currentMode, int requestedMode, PmlState currentState)
        {
            if (Enum.IsDefined(typeof(PmlMode), currentMode))
            {
                return PmlTransitionCheck.CheckModeUpdate((PmlMode)currentMode, (PmlMode)requestedMode, currentState);
            }
            // check if currentmode is a oem mode that is defined.
            else if (MachineDefinedModes.ContainsKey(requestedMode))
            {
                //try getting the current machine defined mode.
                if (MachineDefinedModes.TryGetValue(currentMode, out PmlMachineDefinedMode currentMachineDefinedMode))
                {
                    if (currentMachineDefinedMode.AcceptModeChangeAtState.Contains(currentState))
                    {
                        return new ValidationResult(true);
                    }
                    else
                    {
                        return new ValidationResult(false,
                            "In current Mode [{0}] its not allowed to change while in State [{1}]",
                            currentMachineDefinedMode.Name,
                            currentState.ToString());
                    }
                }
                else
                {
                    throw new ArgumentOutOfRangeException(nameof(currentMode), $"This Oem mode [{currentMode}] is not defined, so current state should never have been reached or current is undefined.");
                }
            }
            else
            {
                return new ValidationResult(false, "Unkown mode {0}.", requestedMode);
            }
        }
    }
}
