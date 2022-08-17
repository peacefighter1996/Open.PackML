using Xunit;

namespace Open.PackML.Tests
{
    public class PmlTransitionCheckTests
    {
        [Theory]
        [InlineData(State.Undefined, true)]
        [InlineData(State.Aborting, true)]
        [InlineData(State.Aborted, true)]
        [InlineData(State.Clearing, true)]
        [InlineData(State.Stopping, true)]
        [InlineData(State.Stopped, true)]
        [InlineData(State.Resetting, true)]
        [InlineData(State.Idle, true)]
        [InlineData(State.Starting, true)]
        [InlineData(State.Execute, true)]
        [InlineData(State.Held, true)]
        [InlineData(State.Holding, true)]
        [InlineData(State.UnHolding, true)]
        [InlineData(State.Suspending, true)]
        [InlineData(State.Suspended, true)]
        [InlineData(State.UnSuspending, true)]
        [InlineData(State.Completing, true)]
        [InlineData(State.Completed, true)]
        public void AbortTest(State state, bool expectation)
        {
            Assert.True(expectation == PmlTransitionCheck.Abort(state), $"Check {state} if it accepts {Command.Abort} Command");
        }

        [Theory]
        [InlineData(State.Undefined, false)]
        [InlineData(State.Aborting, false)]
        [InlineData(State.Aborted, false)]
        [InlineData(State.Clearing, false)]
        [InlineData(State.Stopping, false)]
        [InlineData(State.Stopped, true)]
        [InlineData(State.Resetting, true)]
        [InlineData(State.Idle, true)]
        [InlineData(State.Starting, true)]
        [InlineData(State.Execute, true)]
        [InlineData(State.Held, true)]
        [InlineData(State.Holding, true)]
        [InlineData(State.UnHolding, true)]
        [InlineData(State.Suspending, true)]
        [InlineData(State.Suspended, true)]
        [InlineData(State.UnSuspending, true)]
        [InlineData(State.Completing, true)]
        [InlineData(State.Completed, true)]
        public void StopTest(State state, bool expectation)
        {
            Assert.True(expectation == PmlTransitionCheck.Stop(state), $"Check {state} if it accepts {Command.Stop} Command");
        }

        [Theory]
        [InlineData(State.Undefined, false)]
        [InlineData(State.Aborting, false)]
        [InlineData(State.Aborted, false)]
        [InlineData(State.Clearing, false)]
        [InlineData(State.Stopping, false)]
        [InlineData(State.Stopped, true)]
        [InlineData(State.Resetting, false)]
        [InlineData(State.Idle, false)]
        [InlineData(State.Starting, false)]
        [InlineData(State.Execute, false)]
        [InlineData(State.Held, false)]
        [InlineData(State.Holding, false)]
        [InlineData(State.UnHolding, false)]
        [InlineData(State.Suspending, false)]
        [InlineData(State.Suspended, false)]
        [InlineData(State.UnSuspending, false)]
        [InlineData(State.Completing, false)]
        [InlineData(State.Completed, true)]
        public void ResetTest(State state, bool expectation)
        {
            Assert.True(expectation == PmlTransitionCheck.Reset(state), $"Check {state} if it accepts {Command.Reset} Command");
        }

        [Theory]
        [InlineData(State.Undefined, false)]
        [InlineData(State.Aborting, false)]
        [InlineData(State.Aborted, false)]
        [InlineData(State.Clearing, false)]
        [InlineData(State.Stopping, false)]
        [InlineData(State.Stopped, false)]
        [InlineData(State.Resetting, false)]
        [InlineData(State.Idle, true)]
        [InlineData(State.Starting, false)]
        [InlineData(State.Execute, false)]
        [InlineData(State.Held, false)]
        [InlineData(State.Holding, false)]
        [InlineData(State.UnHolding, false)]
        [InlineData(State.Suspending, false)]
        [InlineData(State.Suspended, false)]
        [InlineData(State.UnSuspending, false)]
        [InlineData(State.Completing, false)]
        [InlineData(State.Completed, false)]
        public void StartTest(State state, bool expectation)
        {
            Assert.True(expectation == PmlTransitionCheck.Start(state), $"Check {state} if it accepts {Command.Start} Command");
        }

        [Theory]
        [InlineData(State.Undefined, false)]
        [InlineData(State.Aborting, false)]
        [InlineData(State.Aborted, false)]
        [InlineData(State.Clearing, false)]
        [InlineData(State.Stopping, false)]
        [InlineData(State.Stopped, false)]
        [InlineData(State.Resetting, false)]
        [InlineData(State.Idle, false)]
        [InlineData(State.Starting, false)]
        [InlineData(State.Execute, true)]
        [InlineData(State.Held, false)]
        [InlineData(State.Holding, false)]
        [InlineData(State.UnHolding, false)]
        [InlineData(State.Suspending, false)]
        [InlineData(State.Suspended, false)]
        [InlineData(State.UnSuspending, false)]
        [InlineData(State.Completing, false)]
        [InlineData(State.Completed, false)]
        public void SuspendTest(State state, bool expectation)
        {
            Assert.True(expectation == PmlTransitionCheck.Suspend(state), $"Check {state} if it accepts {Command.Suspend} Command");
        }

        [Theory]
        [InlineData(State.Undefined, false)]
        [InlineData(State.Aborting, false)]
        [InlineData(State.Aborted, false)]
        [InlineData(State.Clearing, false)]
        [InlineData(State.Stopping, false)]
        [InlineData(State.Stopped, false)]
        [InlineData(State.Resetting, false)]
        [InlineData(State.Idle, false)]
        [InlineData(State.Starting, false)]
        [InlineData(State.Execute, true)]
        [InlineData(State.Held, false)]
        [InlineData(State.Holding, false)]
        [InlineData(State.UnHolding, false)]
        [InlineData(State.Suspending, false)]
        [InlineData(State.Suspended, false)]
        [InlineData(State.UnSuspending, false)]
        [InlineData(State.Completing, false)]
        [InlineData(State.Completed, false)]
        public void HoldTest(State state, bool expectation)
        {
            Assert.True(expectation == PmlTransitionCheck.Hold(state), $"Check {state} if it accepts {Command.Hold} Command");
        }

        [Theory]
        [InlineData(State.Undefined, false)]
        [InlineData(State.Aborting, false)]
        [InlineData(State.Aborted, false)]
        [InlineData(State.Clearing, false)]
        [InlineData(State.Stopping, false)]
        [InlineData(State.Stopped, false)]
        [InlineData(State.Resetting, false)]
        [InlineData(State.Idle, false)]
        [InlineData(State.Starting, false)]
        [InlineData(State.Execute, false)]
        [InlineData(State.Held, true)]
        [InlineData(State.Holding, false)]
        [InlineData(State.UnHolding, false)]
        [InlineData(State.Suspending, false)]
        [InlineData(State.Suspended, false)]
        [InlineData(State.UnSuspending, false)]
        [InlineData(State.Completing, false)]
        [InlineData(State.Completed, false)]
        public void UnHoldTest(State state, bool expectation)
        {
            Assert.True(expectation == PmlTransitionCheck.UnHold(state), $"Check {state} if it accepts {Command.Unhold} Command");
        }

        [Theory]
        [InlineData(State.Undefined, false)]
        [InlineData(State.Aborting, false)]
        [InlineData(State.Aborted, false)]
        [InlineData(State.Clearing, false)]
        [InlineData(State.Stopping, false)]
        [InlineData(State.Stopped, false)]
        [InlineData(State.Resetting, false)]
        [InlineData(State.Idle, false)]
        [InlineData(State.Starting, false)]
        [InlineData(State.Execute, false)]
        [InlineData(State.Held, false)]
        [InlineData(State.Holding, false)]
        [InlineData(State.UnHolding, false)]
        [InlineData(State.Suspending, false)]
        [InlineData(State.Suspended, true)]
        [InlineData(State.UnSuspending, false)]
        [InlineData(State.Completing, false)]
        [InlineData(State.Completed, false)]
        public void UnSuspendTest(State state, bool expectation)
        {
            Assert.True(expectation == PmlTransitionCheck.UnSuspend(state), $"Check {state} if it accepts {Command.UnSuspend} Command");
        }

        [Theory]
        [InlineData(State.Undefined, false)]
        [InlineData(State.Aborting, false)]
        [InlineData(State.Aborted, true)]
        [InlineData(State.Clearing, false)]
        [InlineData(State.Stopping, false)]
        [InlineData(State.Stopped, false)]
        [InlineData(State.Resetting, false)]
        [InlineData(State.Idle, false)]
        [InlineData(State.Starting, false)]
        [InlineData(State.Execute, false)]
        [InlineData(State.Held, false)]
        [InlineData(State.Holding, false)]
        [InlineData(State.UnHolding, false)]
        [InlineData(State.Suspending, false)]
        [InlineData(State.Suspended, false)]
        [InlineData(State.UnSuspending, false)]
        [InlineData(State.Completing, false)]
        [InlineData(State.Completed, false)]
        public void ClearTest(State state, bool expectation)
        {
            Assert.True(expectation == PmlTransitionCheck.Clear(state), $"Check {state} if it accepts {Command.Clear} Command");
        }
    }
}