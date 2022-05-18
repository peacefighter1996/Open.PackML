using Xunit;

namespace Open.PackML.Tests
{
    public class TransitionCheckTests
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
            Assert.True(expectation == TransitionCheck.Abort(state), $"Check {state} if it accepts {Command.Abort} Command");
        }

        [Fact()]
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
        public void StopTest()
        {
            Assert.True(false, "This test needs an implementation");
        }

        [Fact()]
        public void ResetTest()
        {
            Assert.True(false, "This test needs an implementation");
        }

        [Fact()]
        public void StartTest()
        {
            Assert.True(false, "This test needs an implementation");
        }

        [Fact()]
        public void SuspendTest()
        {
            Assert.True(false, "This test needs an implementation");
        }

        [Fact()]
        public void HoldTest()
        {
            Assert.True(false, "This test needs an implementation");
        }

        [Fact()]
        public void UnHoldTest()
        {
            Assert.True(false, "This test needs an implementation");
        }

        [Fact()]
        public void UnSuspendTest()
        {
            Assert.True(false, "This test needs an implementation");
        }

        [Fact()]
        public void ClearTest()
        {
            Assert.True(false, "This test needs an implementation");
        }
    }
}