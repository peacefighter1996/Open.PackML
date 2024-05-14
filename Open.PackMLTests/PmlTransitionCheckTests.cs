using Autabee.Utility;
using Open.PackML;
using System;
using Xunit;

namespace Open.PackMLTests
{
    public class PmlTransitionCheckTests
    {
        [Theory]
        [InlineData(PmlState.Undefined, true, true, true, true)]
        [InlineData(PmlState.Aborting, false, false, false, false)]
        [InlineData(PmlState.Aborted, false, false, false, false)]
        [InlineData(PmlState.Clearing, true, true, true, true)]
        [InlineData(PmlState.Stopping, true, true, true, true)]
        [InlineData(PmlState.Stopped, true, true, true, true)]
        [InlineData(PmlState.Resetting, true, true, true, true)]
        [InlineData(PmlState.Idle, true, true, true, true)]
        [InlineData(PmlState.Starting, true, true, true, true)]
        [InlineData(PmlState.Execute, true, true, true, true)]
        [InlineData(PmlState.Held, true, true, true, true)]
        [InlineData(PmlState.Holding, true, true, true, true)]
        [InlineData(PmlState.UnHolding, true, true, true, true)]
        [InlineData(PmlState.Suspending, true, true, true, true)]
        [InlineData(PmlState.Suspended, true, true, true, true)]
        [InlineData(PmlState.UnSuspending, true, true, true, true)]
        [InlineData(PmlState.Completing, true, true, true, true)]
        [InlineData(PmlState.Completed, true, true, true, true)]


        public void AbortTest(PmlState state, params bool[] args)
        {
            CheckModes(state, args, PmlTransitionCheck.Abort);
        }

        private static void CheckModes(PmlState state, bool[] args, Func<PmlState, PmlMode, ValidationResult> func)
        {
            var collection = new PmlMode[] { PmlMode.Undefined, PmlMode.Production, PmlMode.Maintenance, PmlMode.Manual };
            for (int i = 0; i < collection.Length; i++)
            {
                var test = func(state, collection[i]);
                Assert.True(args[i] == test.Success, $"Check {state} for {collection[i]} if it accepts {func.Method.Name} PmlCommand {test}");
            }
        }

        [Theory]
        [InlineData(PmlState.Undefined, false, false, false, false)]
        [InlineData(PmlState.Aborting, false, false, false, false)]
        [InlineData(PmlState.Aborted, false, false, false, false)]
        [InlineData(PmlState.Clearing, false, false, false, false)]
        [InlineData(PmlState.Stopping, false, false, false, false)]
        [InlineData(PmlState.Stopped, false, false, false, false)]
        [InlineData(PmlState.Resetting, false, true, true, true)]
        [InlineData(PmlState.Idle, false, true, true, true)]
        [InlineData(PmlState.Starting, false, true, true, true)]
        [InlineData(PmlState.Execute, false, true, true, true)]
        [InlineData(PmlState.Held, false, true, true, true)]
        [InlineData(PmlState.Holding, false, true, true, true)]
        [InlineData(PmlState.UnHolding, false, true, true, true)]
        [InlineData(PmlState.Suspending, false, true, true, true)]
        [InlineData(PmlState.Suspended, false, true, true, true)]
        [InlineData(PmlState.UnSuspending, false, true, true, true)]
        [InlineData(PmlState.Completing, false, true, true, true)]
        [InlineData(PmlState.Completed, false, true, true, true)]
        public void StopTest(PmlState state, params bool[] args)
        {
            CheckModes(state, args, PmlTransitionCheck.Stop);
        }

        [Theory]
        [InlineData(PmlState.Undefined, false, false, false, false)]
        [InlineData(PmlState.Aborting, false, false, false, false)]
        [InlineData(PmlState.Aborted, false, false, false, false)]
        [InlineData(PmlState.Clearing, false, false, false, false)]
        [InlineData(PmlState.Stopping, false, false, false, false)]
        [InlineData(PmlState.Stopped, false, true, true, true)]
        [InlineData(PmlState.Resetting, false, false, false, false)]
        [InlineData(PmlState.Idle, false, false, false, false)]
        [InlineData(PmlState.Starting, false, false, false, false)]
        [InlineData(PmlState.Execute, false, false, false, false)]
        [InlineData(PmlState.Held, false, false, false, false)]
        [InlineData(PmlState.Holding, false, false, false, false)]
        [InlineData(PmlState.UnHolding, false, false, false, false)]
        [InlineData(PmlState.Suspending, false, false, false, false)]
        [InlineData(PmlState.Suspended, false, false, false, false)]
        [InlineData(PmlState.UnSuspending, false, false, false, false)]
        [InlineData(PmlState.Completing, false, false, false, false)]
        [InlineData(PmlState.Completed, false, true, true, true)]
        public void ResetTest(PmlState state, params bool[] args)
        {
            CheckModes(state, args, PmlTransitionCheck.Reset);
        }

        [Theory]
        [InlineData(PmlState.Undefined, false, false, false, false)]
        [InlineData(PmlState.Aborting, false, false, false, false)]
        [InlineData(PmlState.Aborted, false, false, false, false)]
        [InlineData(PmlState.Clearing, false, false, false, false)]
        [InlineData(PmlState.Stopping, false, false, false, false)]
        [InlineData(PmlState.Stopped, false, false, false, false)]
        [InlineData(PmlState.Resetting, false, false, false, false)]
        [InlineData(PmlState.Idle, false, true, true, true)]
        [InlineData(PmlState.Starting, false, false, false, false)]
        [InlineData(PmlState.Execute, false, false, false, false)]
        [InlineData(PmlState.Held, false, false, false, false)]
        [InlineData(PmlState.Holding, false, false, false, false)]
        [InlineData(PmlState.UnHolding, false, false, false, false)]
        [InlineData(PmlState.Suspending, false, false, false, false)]
        [InlineData(PmlState.Suspended, false, false, false, false)]
        [InlineData(PmlState.UnSuspending, false, false, false, false)]
        [InlineData(PmlState.Completing, false, false, false, false)]
        [InlineData(PmlState.Completed, false, false, false, false)]
        public void StartTest(PmlState state, params bool[] args)
        {
            CheckModes(state, args, PmlTransitionCheck.Start);
        }

        [Theory]
        [InlineData(PmlState.Undefined, false, false, false, false)]
        [InlineData(PmlState.Aborting, false, false, false, false)]
        [InlineData(PmlState.Aborted, false, false, false, false)]
        [InlineData(PmlState.Clearing, false, false, false, false)]
        [InlineData(PmlState.Stopping, false, false, false, false)]
        [InlineData(PmlState.Stopped, false, false, false, false)]
        [InlineData(PmlState.Resetting, false, false, false, false)]
        [InlineData(PmlState.Idle, false, false, false, false)]
        [InlineData(PmlState.Starting, false, false, false, false)]
        [InlineData(PmlState.Execute, false, true, false, false)]
        [InlineData(PmlState.Held, false, false, false, false)]
        [InlineData(PmlState.Holding, false, false, false, false)]
        [InlineData(PmlState.UnHolding, false, false, false, false)]
        [InlineData(PmlState.Suspending, false, false, false, false)]
        [InlineData(PmlState.Suspended, false, false, false, false)]
        [InlineData(PmlState.UnSuspending, false, false, false, false)]
        [InlineData(PmlState.Completing, false, false, false, false)]
        [InlineData(PmlState.Completed, false, false, false, false)]
        public void SuspendTest(PmlState state, params bool[] args)
        {
            CheckModes(state, args, PmlTransitionCheck.Suspend);
        }

        [Theory]
        [InlineData(PmlState.Undefined, false, false, false, false)]
        [InlineData(PmlState.Aborting, false, false, false, false)]
        [InlineData(PmlState.Aborted, false, false, false, false)]
        [InlineData(PmlState.Clearing, false, false, false, false)]
        [InlineData(PmlState.Stopping, false, false, false, false)]
        [InlineData(PmlState.Stopped, false, false, false, false)]
        [InlineData(PmlState.Resetting, false, false, false, false)]
        [InlineData(PmlState.Idle, false, false, false, false)]
        [InlineData(PmlState.Starting, false, false, false, false)]
        [InlineData(PmlState.Execute, false, true, false, false)]
        [InlineData(PmlState.Held, false, false, false, false)]
        [InlineData(PmlState.Holding, false, false, false, false)]
        [InlineData(PmlState.UnHolding, false, false, false, false)]
        [InlineData(PmlState.Suspending, false, false, false, false)]
        [InlineData(PmlState.Suspended, false, false, false, false)]
        [InlineData(PmlState.UnSuspending, false, false, false, false)]
        [InlineData(PmlState.Completing, false, false, false, false)]
        [InlineData(PmlState.Completed, false, false, false, false)]
        public void HoldTest(PmlState state, params bool[] args)
        {
            CheckModes(state, args, PmlTransitionCheck.Hold);
        }

        [Theory]
        [InlineData(PmlState.Undefined, false, false, false, false)]
        [InlineData(PmlState.Aborting, false, false, false, false)]
        [InlineData(PmlState.Aborted, false, false, false, false)]
        [InlineData(PmlState.Clearing, false, false, false, false)]
        [InlineData(PmlState.Stopping, false, false, false, false)]
        [InlineData(PmlState.Stopped, false, false, false, false)]
        [InlineData(PmlState.Resetting, false, false, false, false)]
        [InlineData(PmlState.Idle, false, false, false, false)]
        [InlineData(PmlState.Starting, false, false, false, false)]
        [InlineData(PmlState.Execute, false, false, false, false)]
        [InlineData(PmlState.Held, false, true, true, true)]
        [InlineData(PmlState.Holding, false, false, false, false)]
        [InlineData(PmlState.UnHolding, false, false, false, false)]
        [InlineData(PmlState.Suspending, false, false, false, false)]
        [InlineData(PmlState.Suspended, false, false, false, false)]
        [InlineData(PmlState.UnSuspending, false, false, false, false)]
        [InlineData(PmlState.Completing, false, false, false, false)]
        [InlineData(PmlState.Completed, false, false, false, false)]
        public void UnHoldTest(PmlState state, params bool[] args)
        {
            CheckModes(state, args, PmlTransitionCheck.UnHold);
        }

        [Theory]
        [InlineData(PmlState.Undefined, false, false, false, false)]
        [InlineData(PmlState.Aborting, false, false, false, false)]
        [InlineData(PmlState.Aborted, false, false, false, false)]
        [InlineData(PmlState.Clearing, false, false, false, false)]
        [InlineData(PmlState.Stopping, false, false, false, false)]
        [InlineData(PmlState.Stopped, false, false, false, false)]
        [InlineData(PmlState.Resetting, false, false, false, false)]
        [InlineData(PmlState.Idle, false, false, false, false)]
        [InlineData(PmlState.Starting, false, false, false, false)]
        [InlineData(PmlState.Execute, false, false, false, false)]
        [InlineData(PmlState.Held, false, false, false, false)]
        [InlineData(PmlState.Holding, false, false, false, false)]
        [InlineData(PmlState.UnHolding, false, false, false, false)]
        [InlineData(PmlState.Suspending, false, false, false, false)]
        [InlineData(PmlState.Suspended, false, true, true, true)]
        [InlineData(PmlState.UnSuspending, false, false, false, false)]
        [InlineData(PmlState.Completing, false, false, false, false)]
        [InlineData(PmlState.Completed, false, false, false, false)]
        public void UnSuspendTest(PmlState state, params bool[] args)
        {
            CheckModes(state, args, PmlTransitionCheck.UnSuspend);
        }

        [Theory]
        [InlineData(PmlState.Undefined, false, false, false, false)]
        [InlineData(PmlState.Aborting, false, false, false, false)]
        [InlineData(PmlState.Aborted, false, true, true, true)]
        [InlineData(PmlState.Clearing, false, false, false, false)]
        [InlineData(PmlState.Stopping, false, false, false, false)]
        [InlineData(PmlState.Stopped, false, false, false, false)]
        [InlineData(PmlState.Resetting, false, false, false, false)]
        [InlineData(PmlState.Idle, false, false, false, false)]
        [InlineData(PmlState.Starting, false, false, false, false)]
        [InlineData(PmlState.Execute, false, false, false, false)]
        [InlineData(PmlState.Held, false, false, false, false)]
        [InlineData(PmlState.Holding, false, false, false, false)]
        [InlineData(PmlState.UnHolding, false, false, false, false)]
        [InlineData(PmlState.Suspending, false, false, false, false)]
        [InlineData(PmlState.Suspended, false, false, false, false)]
        [InlineData(PmlState.UnSuspending, false, false, false, false)]
        [InlineData(PmlState.Completing, false, false, false, false)]
        [InlineData(PmlState.Completed, false, false, false, false)]
        public void ClearTest(PmlState state, params bool[] args)
        {
            CheckModes(state, args, PmlTransitionCheck.Clear);
        }
    }
}