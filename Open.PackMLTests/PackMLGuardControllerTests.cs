using Xunit;
using Open.PackML.Prefab;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Open.PackML.Interfaces;
using Open.PackML.EventArguments;
using Autabee.Utility;
using Moq;
using Open.PackML;
using Open.PackMLTests.TestObjects;

namespace Open.PackMLTests
{
    public class PackMLGuardControllerTests
    {
        [Theory]
        [InlineData(PmlState.Aborted, PmlMode.Undefined, PmlMode.Maintenance, true)]
        [InlineData(PmlState.Aborted, PmlMode.Undefined, PmlMode.Production, true)]
        [InlineData(PmlState.Aborted, PmlMode.Undefined, PmlMode.Manual, true)]
        [InlineData(PmlState.Aborted, PmlMode.Undefined, PmlMode.Undefined, false)]
        [InlineData(PmlState.Aborted, PmlMode.Maintenance, PmlMode.Maintenance, false)]
        [InlineData(PmlState.Aborted, PmlMode.Maintenance, PmlMode.Production, true)]
        [InlineData(PmlState.Aborted, PmlMode.Maintenance, PmlMode.Manual, true)]
        [InlineData(PmlState.Aborted, PmlMode.Maintenance, PmlMode.Undefined, false)]
        [InlineData(PmlState.Aborted, PmlMode.Production, PmlMode.Maintenance, true)]
        [InlineData(PmlState.Aborted, PmlMode.Production, PmlMode.Production, false)]
        [InlineData(PmlState.Aborted, PmlMode.Production, PmlMode.Manual, true)]
        [InlineData(PmlState.Aborted, PmlMode.Production, PmlMode.Undefined, false)]
        [InlineData(PmlState.Aborted, PmlMode.Manual, PmlMode.Maintenance, true)]
        [InlineData(PmlState.Aborted, PmlMode.Manual, PmlMode.Production, true)]
        [InlineData(PmlState.Aborted, PmlMode.Manual, PmlMode.Manual, false)]
        [InlineData(PmlState.Aborted, PmlMode.Manual, PmlMode.Undefined, false)]

        [InlineData(PmlState.Idle, PmlMode.Undefined, PmlMode.Maintenance, true)]
        [InlineData(PmlState.Idle, PmlMode.Undefined, PmlMode.Production, true)]
        [InlineData(PmlState.Idle, PmlMode.Undefined, PmlMode.Manual, true)]
        [InlineData(PmlState.Idle, PmlMode.Undefined, PmlMode.Undefined, false)]
        [InlineData(PmlState.Idle, PmlMode.Maintenance, PmlMode.Maintenance, false)]
        [InlineData(PmlState.Idle, PmlMode.Maintenance, PmlMode.Production, true)]
        [InlineData(PmlState.Idle, PmlMode.Maintenance, PmlMode.Manual, true)]
        [InlineData(PmlState.Idle, PmlMode.Maintenance, PmlMode.Undefined, false)]
        [InlineData(PmlState.Idle, PmlMode.Production, PmlMode.Maintenance, true)]
        [InlineData(PmlState.Idle, PmlMode.Production, PmlMode.Production, false)]
        [InlineData(PmlState.Idle, PmlMode.Production, PmlMode.Manual, true)]
        [InlineData(PmlState.Idle, PmlMode.Production, PmlMode.Undefined, false)]
        [InlineData(PmlState.Idle, PmlMode.Manual, PmlMode.Maintenance, true)]
        [InlineData(PmlState.Idle, PmlMode.Manual, PmlMode.Production, true)]
        [InlineData(PmlState.Idle, PmlMode.Manual, PmlMode.Manual, false)]
        [InlineData(PmlState.Idle, PmlMode.Manual, PmlMode.Undefined, false)]


        [InlineData(PmlState.Stopped, PmlMode.Undefined, PmlMode.Maintenance, true)]
        [InlineData(PmlState.Stopped, PmlMode.Undefined, PmlMode.Production, true)]
        [InlineData(PmlState.Stopped, PmlMode.Undefined, PmlMode.Manual, true)]
        [InlineData(PmlState.Stopped, PmlMode.Undefined, PmlMode.Undefined, false)]
        [InlineData(PmlState.Stopped, PmlMode.Maintenance, PmlMode.Maintenance, false)]
        [InlineData(PmlState.Stopped, PmlMode.Maintenance, PmlMode.Production, true)]
        [InlineData(PmlState.Stopped, PmlMode.Maintenance, PmlMode.Manual, true)]
        [InlineData(PmlState.Stopped, PmlMode.Maintenance, PmlMode.Undefined, false)]
        [InlineData(PmlState.Stopped, PmlMode.Production, PmlMode.Maintenance, true)]
        [InlineData(PmlState.Stopped, PmlMode.Production, PmlMode.Production, false)]
        [InlineData(PmlState.Stopped, PmlMode.Production, PmlMode.Manual, true)]
        [InlineData(PmlState.Stopped, PmlMode.Production, PmlMode.Undefined, false)]
        [InlineData(PmlState.Stopped, PmlMode.Manual, PmlMode.Maintenance, true)]
        [InlineData(PmlState.Stopped, PmlMode.Manual, PmlMode.Production, true)]
        [InlineData(PmlState.Stopped, PmlMode.Manual, PmlMode.Manual, false)]
        [InlineData(PmlState.Stopped, PmlMode.Manual, PmlMode.Undefined, false)]
        public void UpdatePackMLModeTest(PmlState state, PmlMode mode, PmlMode newMode, bool succes)
        {
            var passedInternal = false;
            var catchFunc = delegate ()
            {
                passedInternal = true;
            };

            var moqController = new Mock<IPmlController<Enum>>();
            moqController.Setup(x => x.SendPmlCommand(It.IsAny<PmlCommand>())).Returns(new ValidationResult(true));
            moqController.Setup(x => x.UpdatePmlMode(It.IsAny<PmlMode>())).Callback(catchFunc).Returns(new ValidationResult(true));
            moqController.Setup(x => x.CurrentPmlState()).Returns(state);
            moqController.Setup(x => x.CurrentPmlMode()).Returns(mode);
            var eventStore = new EventStore();
            var controller = new PackMLGuardController<Enum>(moqController.Object, eventStore);

            var result = controller.UpdatePmlMode(newMode);
            Assert.Equal(succes, result.Success);
            Assert.Equal(succes, passedInternal);
        }

        [Theory]
        [InlineData(PmlMode.Undefined, PmlMode.Maintenance)]
        [InlineData(PmlMode.Undefined, PmlMode.Production)]
        [InlineData(PmlMode.Undefined, PmlMode.Manual)]
        [InlineData(PmlMode.Undefined, PmlMode.Undefined)]
        [InlineData(PmlMode.Maintenance, PmlMode.Maintenance)]
        [InlineData(PmlMode.Maintenance, PmlMode.Production)]
        [InlineData(PmlMode.Maintenance, PmlMode.Manual)]
        [InlineData(PmlMode.Maintenance, PmlMode.Undefined)]
        [InlineData(PmlMode.Production, PmlMode.Maintenance)]
        [InlineData(PmlMode.Production, PmlMode.Production)]
        [InlineData(PmlMode.Production, PmlMode.Manual)]
        [InlineData(PmlMode.Production, PmlMode.Undefined)]
        [InlineData(PmlMode.Manual, PmlMode.Maintenance)]
        [InlineData(PmlMode.Manual, PmlMode.Production)]
        [InlineData(PmlMode.Manual, PmlMode.Manual)]
        [InlineData(PmlMode.Manual, PmlMode.Undefined)]

        public void UpdatePackMLModeTestFail(PmlMode mode, PmlMode newMode)
        {
            bool passedInternal;
            var catchFunc = delegate ()
            {
                passedInternal = true;
            };


            var moqController = new Mock<IPmlController<Enum>>();
            moqController.Setup(x => x.SendPmlCommand(It.IsAny<PmlCommand>())).Returns(new ValidationResult(true));
            moqController.Setup(x => x.UpdatePmlMode(It.IsAny<PmlMode>())).Callback(catchFunc).Returns(new ValidationResult(true));

            moqController.Setup(x => x.CurrentPmlMode()).Returns(mode);
            var eventStore = new EventStore();

            PmlState[] pmlStates = new PmlState[]{
                PmlState.Undefined ,
                PmlState.Clearing ,
                PmlState.Starting,
                PmlState.Suspended,
                PmlState.Execute ,
                PmlState.Stopping,
                PmlState.Aborting,
                PmlState.Holding ,
                PmlState.Held ,
                PmlState.UnHolding,
                PmlState.Suspending,
                PmlState.UnSuspending ,
                PmlState.Resetting ,
                PmlState.Completing,
                PmlState.Completed,
            };

            for (var i = 0; i < pmlStates.Length; i++)
            {
                moqController.Setup(x => x.CurrentPmlState()).Returns(pmlStates[i]);
                passedInternal = false;
                var controller = new PackMLGuardController<Enum>(moqController.Object, eventStore);

                var result = controller.UpdatePmlMode(newMode);
                Assert.False(result.Success);
                Assert.False(passedInternal);
            }
        }

        [Fact()]
        public void UpdatePackMLStateTest()
        {
            List<PmlState> catchStates = new List<PmlState>();
            void Controller_UpdateCurrentState(object sender, PmlStateChangeEventArg e)
            {
                catchStates.Add(e.CurrentState);
            }
            var testController = new TestPmlController();
            testController.CurrentState = PmlState.Aborted;
            testController.CurrentMode = PmlMode.Manual;
            var eventStore = new EventStore();
            var controller = new PackMLGuardController<Enum>(testController, eventStore);
            controller.UpdateCurrentState += Controller_UpdateCurrentState;

            //var result = controller.SendPmlCommand(PmlCommand.Abort);
            //Assert.True(result.Success, result.FailString());
            //controller.UpdatePmlMode(PmlMode.Production);
            var result = controller.SendPmlCommand(PmlCommand.Clear);

            Assert.True(result.Success, result.FailString());
            Assert.Equal(PmlState.Clearing, catchStates[0]);
            Assert.Equal(PmlState.Stopped, catchStates[1]);
        }

        [Fact()]
        public void MachineEvent()
        {
            var testController = new TestPmlController();
            testController.CurrentState = PmlState.Aborted;
            testController.CurrentMode = PmlMode.Manual;
            var eventStore = new EventStore();
            eventStore.Add(EventHanderEnum1.id1, new PmlEventReaction<Enum>(EventHanderEnum1.id1, PmlState.Stopping));
           
            var controller = new PackMLGuardController<Enum>(testController, eventStore);
            testController.InvokeEvent(EventHanderEnum1.id1);
            //var result = controller.SendPmlCommand(PmlCommand.Abort);
            //Assert.True(result.Success, result.FailString());
            //controller.UpdatePmlMode(PmlMode.Production);

            Assert.Equal(PmlState.Stopping, controller.CurrentPmlState());
        }
    }
}