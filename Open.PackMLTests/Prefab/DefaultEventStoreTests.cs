using System;
using Xunit;

namespace Open.PackML.Prefab.Tests
{
    public class DefaultEventStoreTests
    {


        IPackMLEventStore<Enum> eventStore;

        public DefaultEventStoreTests()
        {
            this.eventStore = new EventStore();
            eventStore.Add(EventHanderEnum1.id1, new EventReaction<Enum>(EventHanderEnum1.id1, State.Idle));
            eventStore.Add(EventHanderEnum1.id2, new EventReaction<Enum>(EventHanderEnum2.id2, State.Aborted));
            eventStore.Add(EventHanderEnum2.id1, new EventReaction<Enum>(EventHanderEnum1.id1, State.Clearing));
            eventStore.Add(EventHanderEnum2.id2, new EventReaction<Enum>(EventHanderEnum2.id2, State.Held));
        }

        [Theory]
        [InlineData(EventHanderEnum1.id1, PmlState.Idle)]
        [InlineData(EventHanderEnum1.id2, PmlState.Aborted)]
        [InlineData(EventHanderEnum2.id1, PmlState.Clearing)]
        [InlineData(EventHanderEnum2.id2, PmlState.Held)]
        public void ProcessEventTest(Enum arg, PmlState expectation)
        {
            Assert.Equal(expectation, eventStore.GetMachineEvent(arg).StateChangeId);
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