using Xunit;
using Open.PackML;

namespace Open.PackMLTests
{
    public class PmlTransitionCheckTests
    {
        [Theory]
        [InlineData(PmlState.Undefined, true)]
        [InlineData(PmlState.Aborting, false)]
        [InlineData(PmlState.Aborted, false)]
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
            Assert.True(expectation == PmlTransitionCheck.Abort(PmlState).Success, $"Check {PmlState} if it accepts {PmlCommand.Abort} PmlCommand");
        }

        [Theory]
        [InlineData(PmlState.Undefined, false)]
        [InlineData(PmlState.Aborting, false)]
        [InlineData(PmlState.Aborted, false)]
        [InlineData(PmlState.Clearing, false)]
        [InlineData(PmlState.Stopping, false)]
        [InlineData(PmlState.Stopped, false)]
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
            Assert.True(expectation == PmlTransitionCheck.Stop(PmlState).Success, $"Check {PmlState} if it accepts {PmlCommand.Stop} PmlCommand");
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
            Assert.True(expectation == PmlTransitionCheck.Reset(PmlState).Success, $"Check {PmlState} if it accepts {PmlCommand.Reset} PmlCommand");
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
            Assert.True(expectation == PmlTransitionCheck.Start(PmlState).Success, $"Check {PmlState} if it accepts {PmlCommand.Start} PmlCommand");
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
            Assert.True(expectation == PmlTransitionCheck.Suspend(PmlState).Success, $"Check {PmlState} if it accepts {PmlCommand.Suspend} PmlCommand");
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
            Assert.True(expectation == PmlTransitionCheck.Hold(PmlState).Success, $"Check {PmlState} if it accepts {PmlCommand.Hold} PmlCommand");
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
            Assert.True(expectation == PmlTransitionCheck.UnHold(PmlState).Success, $"Check {PmlState} if it accepts {PmlCommand.UnHold} PmlCommand");
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
            Assert.True(expectation == PmlTransitionCheck.UnSuspend(PmlState).Success, $"Check {PmlState} if it accepts {PmlCommand.UnSuspend} PmlCommand");
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
            Assert.True(expectation == PmlTransitionCheck.Clear(PmlState).Success, $"Check {PmlState} if it accepts {PmlCommand.Clear} PmlCommand");
        }
    }
}