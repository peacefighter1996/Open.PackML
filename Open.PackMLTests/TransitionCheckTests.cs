using Xunit;

namespace Open.PackML.Tests
{
    public class PmlTransitionCheckTests
    {
        [Theory]
        [InlineData(PmlState.Undefined, true)]
        [InlineData(PmlState.Aborting, true)]
        [InlineData(PmlState.Aborted, true)]
        [InlineData(PmlState.Clearing, true)]
        [InlineData(PmlState.Stopping, true)]
        [InlineData(PmlState.Stopped, true)]
        [InlineData(PmlState.Resetting, true)]
        [InlineData(PmlState.Idle, true)]
        [InlineData(PmlState.Starting, true)]
        [InlineData(PmlState.Execute, true)]
        [InlineData(PmlState.Held, true)]
        [InlineData(PmlState.Holding, true)]
        [InlineData(PmlState.UnHolding, true)]
        [InlineData(PmlState.Suspending, true)]
        [InlineData(PmlState.Suspended, true)]
        [InlineData(PmlState.UnSuspending, true)]
        [InlineData(PmlState.Completing, true)]
        [InlineData(PmlState.Completed, true)]
        public void AbortTest(PmlState PmlState, bool expectation)
        {
            Assert.True(expectation == PmlTransitionCheck.Abort(state), $"Check {state} if it accepts {Command.Abort} Command");
        }

        [Theory]
        [InlineData(PmlState.Undefined, false)]
        [InlineData(PmlState.Aborting, false)]
        [InlineData(PmlState.Aborted, false)]
        [InlineData(PmlState.Clearing, false)]
        [InlineData(PmlState.Stopping, false)]
        [InlineData(PmlState.Stopped, true)]
        [InlineData(PmlState.Resetting, true)]
        [InlineData(PmlState.Idle, true)]
        [InlineData(PmlState.Starting, true)]
        [InlineData(PmlState.Execute, true)]
        [InlineData(PmlState.Held, true)]
        [InlineData(PmlState.Holding, true)]
        [InlineData(PmlState.UnHolding, true)]
        [InlineData(PmlState.Suspending, true)]
        [InlineData(PmlState.Suspended, true)]
        [InlineData(PmlState.UnSuspending, true)]
        [InlineData(PmlState.Completing, true)]
        [InlineData(PmlState.Completed, true)]
        public void StopTest(PmlState PmlState, bool expectation)
        {
            Assert.True(expectation == PmlTransitionCheck.Stop(state), $"Check {state} if it accepts {Command.Stop} Command");
        }

        [Theory]
        [InlineData(PmlState.Undefined, false)]
        [InlineData(PmlState.Aborting, false)]
        [InlineData(PmlState.Aborted, false)]
        [InlineData(PmlState.Clearing, false)]
        [InlineData(PmlState.Stopping, false)]
        [InlineData(PmlState.Stopped, true)]
        [InlineData(PmlState.Resetting, false)]
        [InlineData(PmlState.Idle, false)]
        [InlineData(PmlState.Starting, false)]
        [InlineData(PmlState.Execute, false)]
        [InlineData(PmlState.Held, false)]
        [InlineData(PmlState.Holding, false)]
        [InlineData(PmlState.UnHolding, false)]
        [InlineData(PmlState.Suspending, false)]
        [InlineData(PmlState.Suspended, false)]
        [InlineData(PmlState.UnSuspending, false)]
        [InlineData(PmlState.Completing, false)]
        [InlineData(PmlState.Completed, true)]
        public void ResetTest(PmlState PmlState, bool expectation)
        {
            Assert.True(expectation == PmlTransitionCheck.Reset(state), $"Check {state} if it accepts {Command.Reset} Command");
        }

        [Theory]
        [InlineData(PmlState.Undefined, false)]
        [InlineData(PmlState.Aborting, false)]
        [InlineData(PmlState.Aborted, false)]
        [InlineData(PmlState.Clearing, false)]
        [InlineData(PmlState.Stopping, false)]
        [InlineData(PmlState.Stopped, false)]
        [InlineData(PmlState.Resetting, false)]
        [InlineData(PmlState.Idle, true)]
        [InlineData(PmlState.Starting, false)]
        [InlineData(PmlState.Execute, false)]
        [InlineData(PmlState.Held, false)]
        [InlineData(PmlState.Holding, false)]
        [InlineData(PmlState.UnHolding, false)]
        [InlineData(PmlState.Suspending, false)]
        [InlineData(PmlState.Suspended, false)]
        [InlineData(PmlState.UnSuspending, false)]
        [InlineData(PmlState.Completing, false)]
        [InlineData(PmlState.Completed, false)]
        public void StartTest(PmlState PmlState, bool expectation)
        {
            Assert.True(expectation == PmlTransitionCheck.Start(state), $"Check {state} if it accepts {Command.Start} Command");
        }

        [Theory]
        [InlineData(PmlState.Undefined, false)]
        [InlineData(PmlState.Aborting, false)]
        [InlineData(PmlState.Aborted, false)]
        [InlineData(PmlState.Clearing, false)]
        [InlineData(PmlState.Stopping, false)]
        [InlineData(PmlState.Stopped, false)]
        [InlineData(PmlState.Resetting, false)]
        [InlineData(PmlState.Idle, false)]
        [InlineData(PmlState.Starting, false)]
        [InlineData(PmlState.Execute, true)]
        [InlineData(PmlState.Held, false)]
        [InlineData(PmlState.Holding, false)]
        [InlineData(PmlState.UnHolding, false)]
        [InlineData(PmlState.Suspending, false)]
        [InlineData(PmlState.Suspended, false)]
        [InlineData(PmlState.UnSuspending, false)]
        [InlineData(PmlState.Completing, false)]
        [InlineData(PmlState.Completed, false)]
        public void SuspendTest(PmlState PmlState, bool expectation)
        {
            Assert.True(expectation == PmlTransitionCheck.Suspend(state), $"Check {state} if it accepts {Command.Suspend} Command");
        }

        [Theory]
        [InlineData(PmlState.Undefined, false)]
        [InlineData(PmlState.Aborting, false)]
        [InlineData(PmlState.Aborted, false)]
        [InlineData(PmlState.Clearing, false)]
        [InlineData(PmlState.Stopping, false)]
        [InlineData(PmlState.Stopped, false)]
        [InlineData(PmlState.Resetting, false)]
        [InlineData(PmlState.Idle, false)]
        [InlineData(PmlState.Starting, false)]
        [InlineData(PmlState.Execute, true)]
        [InlineData(PmlState.Held, false)]
        [InlineData(PmlState.Holding, false)]
        [InlineData(PmlState.UnHolding, false)]
        [InlineData(PmlState.Suspending, false)]
        [InlineData(PmlState.Suspended, false)]
        [InlineData(PmlState.UnSuspending, false)]
        [InlineData(PmlState.Completing, false)]
        [InlineData(PmlState.Completed, false)]
        public void HoldTest(PmlState PmlState, bool expectation)
        {
            Assert.True(expectation == PmlTransitionCheck.Hold(state), $"Check {state} if it accepts {Command.Hold} Command");
        }

        [Theory]
        [InlineData(PmlState.Undefined, false)]
        [InlineData(PmlState.Aborting, false)]
        [InlineData(PmlState.Aborted, false)]
        [InlineData(PmlState.Clearing, false)]
        [InlineData(PmlState.Stopping, false)]
        [InlineData(PmlState.Stopped, false)]
        [InlineData(PmlState.Resetting, false)]
        [InlineData(PmlState.Idle, false)]
        [InlineData(PmlState.Starting, false)]
        [InlineData(PmlState.Execute, false)]
        [InlineData(PmlState.Held, true)]
        [InlineData(PmlState.Holding, false)]
        [InlineData(PmlState.UnHolding, false)]
        [InlineData(PmlState.Suspending, false)]
        [InlineData(PmlState.Suspended, false)]
        [InlineData(PmlState.UnSuspending, false)]
        [InlineData(PmlState.Completing, false)]
        [InlineData(PmlState.Completed, false)]
        public void UnHoldTest(PmlState PmlState, bool expectation)
        {
            Assert.True(expectation == PmlTransitionCheck.UnHold(state), $"Check {state} if it accepts {Command.Unhold} Command");
        }

        [Theory]
        [InlineData(PmlState.Undefined, false)]
        [InlineData(PmlState.Aborting, false)]
        [InlineData(PmlState.Aborted, false)]
        [InlineData(PmlState.Clearing, false)]
        [InlineData(PmlState.Stopping, false)]
        [InlineData(PmlState.Stopped, false)]
        [InlineData(PmlState.Resetting, false)]
        [InlineData(PmlState.Idle, false)]
        [InlineData(PmlState.Starting, false)]
        [InlineData(PmlState.Execute, false)]
        [InlineData(PmlState.Held, false)]
        [InlineData(PmlState.Holding, false)]
        [InlineData(PmlState.UnHolding, false)]
        [InlineData(PmlState.Suspending, false)]
        [InlineData(PmlState.Suspended, true)]
        [InlineData(PmlState.UnSuspending, false)]
        [InlineData(PmlState.Completing, false)]
        [InlineData(PmlState.Completed, false)]
        public void UnSuspendTest(PmlState PmlState, bool expectation)
        {
            Assert.True(expectation == PmlTransitionCheck.UnSuspend(state), $"Check {state} if it accepts {Command.UnSuspend} Command");
        }

        [Theory]
        [InlineData(PmlState.Undefined, false)]
        [InlineData(PmlState.Aborting, false)]
        [InlineData(PmlState.Aborted, true)]
        [InlineData(PmlState.Clearing, false)]
        [InlineData(PmlState.Stopping, false)]
        [InlineData(PmlState.Stopped, false)]
        [InlineData(PmlState.Resetting, false)]
        [InlineData(PmlState.Idle, false)]
        [InlineData(PmlState.Starting, false)]
        [InlineData(PmlState.Execute, false)]
        [InlineData(PmlState.Held, false)]
        [InlineData(PmlState.Holding, false)]
        [InlineData(PmlState.UnHolding, false)]
        [InlineData(PmlState.Suspending, false)]
        [InlineData(PmlState.Suspended, false)]
        [InlineData(PmlState.UnSuspending, false)]
        [InlineData(PmlState.Completing, false)]
        [InlineData(PmlState.Completed, false)]
        public void ClearTest(PmlState PmlState, bool expectation)
        {
            Assert.True(expectation == PmlTransitionCheck.Clear(state), $"Check {state} if it accepts {Command.Clear} Command");
        }
    }
}