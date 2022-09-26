using Open.PackML.Interfaces;
using Open.PackML.Tags;
using Open.PackML.Tags.Attributes;
using System;
using System.Linq;

namespace Open.PackML.Prefab
{
    public class PackMLEumController<T> : PackMLTr88Controller<T> where T : Enum
    {
        public PackMLEumController(IPmlController<T> controller, IPmlEventStore<T> eventStore) : base(controller, eventStore)
        {

        }

        [TagEndUserTerm("Warning")]
        [TagType(TagType.Admin)]
        public PmlWarning[] Warning { get; protected set; } = new PmlWarning[10];


        [TagEndUserTerm("Parameter")]
        [TagType(TagType.Status)]
        public PmlParameter[] Parameter { get; protected set; } = new PmlParameter[10];

        [TagEndUserTerm("Parameter")]
        [TagType(TagType.Status)]
        public PmlRemoteInterface RemoteInterface { get; protected set; }
    }
}
