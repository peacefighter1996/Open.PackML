using Xunit;

namespace Open.PackML.Tests
{
    public class TransitionCheckTests
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
            Assert.True(expectation == TransitionCheck.Abort(PmlState), $"Check {PmlState} if it accepts {PmlCommand.Abort} PmlCommand");
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
            Assert.True(expectation == TransitionCheck.Stop(PmlState), $"Check {PmlState} if it accepts {PmlCommand.Stop} PmlCommand");
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
            Assert.True(expectation == TransitionCheck.Reset(PmlState), $"Check {PmlState} if it accepts {PmlCommand.Reset} PmlCommand");
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
            Assert.True(expectation == TransitionCheck.Start(PmlState), $"Check {PmlState} if it accepts {PmlCommand.Start} PmlCommand");
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
            Assert.True(expectation == TransitionCheck.Suspend(PmlState), $"Check {PmlState} if it accepts {PmlCommand.Suspend} PmlCommand");
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
            Assert.True(expectation == TransitionCheck.Hold(PmlState), $"Check {PmlState} if it accepts {PmlCommand.Hold} PmlCommand");
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
            Assert.True(expectation == TransitionCheck.UnHold(PmlState), $"Check {PmlState} if it accepts {PmlCommand.Unhold} PmlCommand");
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
            Assert.True(expectation == TransitionCheck.UnSuspend(PmlState), $"Check {PmlState} if it accepts {PmlCommand.UnSuspend} PmlCommand");
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
            Assert.True(expectation == TransitionCheck.Clear(PmlState), $"Check {PmlState} if it accepts {PmlCommand.Clear} PmlCommand");
        }
    }
}