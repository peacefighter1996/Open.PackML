using System;
using Xunit;
using Open.PackML;
using Open.PackML.Prefab;
using Open.PackMLTests.TestObjects;

namespace Open.PackMLTests.Prefab
{
    public class DefaultEventStoreTests
    {
        PmlEventStore eventStore;

        public DefaultEventStoreTests()
        {
            this.eventStore = new PmlEventStore();
            eventStore.Add(new PmlEventReaction(EventHanderEnum1.id1, PmlState.Idle));
            eventStore.Add(new PmlEventReaction(EventHanderEnum1.id2, PmlState.Aborted));
            eventStore.Add(new PmlEventReaction(EventHanderEnum2.id1, PmlState.Clearing));
            eventStore.Add(new PmlEventReaction(EventHanderEnum2.id2, PmlState.Held));
        }

        [Theory]
        [InlineData(EventHanderEnum1.id1, PmlState.Idle)]
        [InlineData(EventHanderEnum1.id2, PmlState.Aborted)]
        [InlineData(EventHanderEnum2.id1, PmlState.Clearing)]
        [InlineData(EventHanderEnum2.id2, PmlState.Held)]
        public void ProcessEventTest(Enum arg, PmlState expectation)
        {
            var newState = eventStore.ProcessEvent(arg);
            Assert.Equal(expectation, newState.Object);
        }

        [Theory]
        [InlineData(EventHanderEnum1.id3)]
        [InlineData(EventHanderEnum2.id3)]
        public void FailProcessEventTest(Enum arg)
        {
            Assert.Equal(null, eventStore.GetMachineEvent(arg));
        }
    }
}