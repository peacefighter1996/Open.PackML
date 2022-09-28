using Autabee.Utility;
using Open.PackML;
using Open.PackML.EventArguments;
using Open.PackML.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Open.PackMLTests.TestObjects
{
    public class TestPmlController : IPmlController<Enum>
    {
        public PmlMode CurrentMode = PmlMode.Undefined;
        public PmlState CurrentState = PmlState.Undefined;
        public event EventHandler<PmlStateChangeEventArg> UpdateCurrentState;
        public event EventHandler<MachineEventArgs<Enum>> MachineEvent;

        public PmlMode CurrentPmlMode()
        {
            return CurrentMode;
        }

        public PmlState CurrentPmlState()
        {
            return CurrentState;
        }

        public PmlMode RetrieveCurrentPackMLMode()
        {
            throw new NotImplementedException();
        }

        public Task<PmlMode> RetrieveCurrentPackMLModeAsync()
        {
            throw new NotImplementedException();
        }

        public void InvokeEvent(Enum @enum)
        {

            MachineEvent?.Invoke(this, new MachineEventArgs<Enum>() { @enum = @enum });

        }

        public PmlState RetrieveCurrentPackMLState()
        {
            throw new NotImplementedException();
        }

        public Task<PmlState> RetrieveCurrentPackMLStateAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<ValidationResult> SendPackMLCommandAsync(PmlCommand packMLCommand) => SendPmlCommand(packMLCommand);

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

        public async Task<ValidationResult> UpdatePackMLModeAsync(PmlMode packMLMode) => UpdatePmlMode(packMLMode);

        public ValidationResult UpdatePmlMode(PmlMode packMLMode)
        {
            throw new NotImplementedException();
        }
    }
}