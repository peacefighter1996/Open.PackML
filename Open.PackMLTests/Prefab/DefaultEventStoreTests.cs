using System;
using Xunit;
using Open.PackML;
using Open.PackML.Prefab;

namespace Open.PackMLTests.Prefab
{
    public class DefaultEventStoreTests
    {
        EventStore eventStore;

        public DefaultEventStoreTests()
        {
            this.eventStore = new EventStore();
            eventStore.Add(EventHanderEnum1.id1, new PmlEventReaction<Enum>(EventHanderEnum1.id1, PmlState.Idle));
            eventStore.Add(EventHanderEnum1.id2, new PmlEventReaction<Enum>(EventHanderEnum2.id2, PmlState.Aborted));
            eventStore.Add(EventHanderEnum2.id1, new PmlEventReaction<Enum>(EventHanderEnum1.id1, PmlState.Clearing));
            eventStore.Add(EventHanderEnum2.id2, new PmlEventReaction<Enum>(EventHanderEnum2.id2, PmlState.Held));
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

    public enum EventHanderEnum1
    {
        id1,
        id2,
        id3
    }

    public enum EventHanderEnum2
    {
        id1,
        id2,
        id3
    }
}