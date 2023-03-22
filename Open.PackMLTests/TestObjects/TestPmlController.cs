using Autabee.Utility;
using Open.PackML;
using Open.PackML.EventArguments;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Open.PackMLTests.TestObjects
{
    public class TestPmlController : IPmlController
    {
        public PmlMode CurrentMode = PmlMode.Undefined;
        public PmlState CurrentState = PmlState.Undefined;
        public event EventHandler<PmlStateChangeEventArg> UpdateCurrentState;
        public event EventHandler<PmlMachineEventArgs> MachineEvent;

        public PmlMode CurrentPmlMode()
        {
            return CurrentMode;
        }

        public PmlState CurrentPmlState()
        {
            return CurrentState;
        }

        public PmlMode RetrieveCurrentPmlMode()
        {
            throw new NotImplementedException();
        }

        public Task<PmlMode> RetrieveCurrentPmlModeAsync(CancellationToken token)
        {
            throw new NotImplementedException();
        }

        public void InvokeEvent(Enum @enum)
        {

            MachineEvent?.Invoke(this, new PmlMachineEventArgs() { @enum = @enum });

        }

        public PmlState RetrieveCurrentPmlState()
        {
            throw new NotImplementedException();
        }

        public Task<PmlState> RetrieveCurrentPmlStateAsync(CancellationToken token)
        {
            throw new NotImplementedException();
        }

        public async Task<ValidationResult> SendPmlCommandAsync(PmlCommand packMLCommand, CancellationToken token) => SendPmlCommand(packMLCommand);

        public ValidationResult SendPmlCommand(PmlCommand packMLCommand)
        {
            if (packMLCommand == PmlCommand.Clear)
            {
                CurrentState = PmlState.Clearing;
                UpdateCurrentState.Invoke(this, new PmlStateChangeEventArg()
                {
                    CurrentMode = CurrentMode,
                    CurrentState = CurrentState,
                    DateTime = DateTime.UtcNow
                });

                Task.Delay(20);
                CurrentState = PmlState.Stopped;
                UpdateCurrentState.Invoke(this, new PmlStateChangeEventArg()
                {
                    CurrentMode = CurrentMode,
                    CurrentState = CurrentState,
                    DateTime = DateTime.UtcNow
                });
                return new ValidationResult(true);
            }
            else
            {
                return new ValidationResult(false);
            }

        }

        public async Task<ValidationResult> UpdatePmlModeAsync(PmlMode packMLMode, CancellationToken token) => UpdatePmlMode(packMLMode);

        public ValidationResult UpdatePmlMode(PmlMode packMLMode)
        {
            throw new NotImplementedException();
        }

        public ValidationResult UpdatePmlMode(int packMLMode)
        {
            throw new NotImplementedException();
        }

        public Task<ValidationResult> UpdatePmlModeAsync(int packMLMode, CancellationToken token)
        {
            throw new NotImplementedException();
        }
    }
}