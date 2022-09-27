using Open.PackML.Interfaces;
using Open.PackML.Tags;
using Open.PackML.Tags.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Open.PackML.Prefab
{
    public class PmlEumController<T> : PmlTr88Controller<T> where T : Enum
    {
        public PmlEumController(IPmlController<T> controller, IPmlEventStore<T> eventStore) : base(controller, eventStore)
        {

        }

        [TagEndUserTerm("Warning")]
        [TagType(TagType.Admin)]
        public List<PmlWarning> Warning { get; protected set; } = new List<PmlWarning>();


        [TagEndUserTerm("Parameter")]
        [TagType(TagType.Status)]
        public List<PmlWarning> Parameter { get; protected set; } = new List<PmlWarning>();

        [TagEndUserTerm("Parameter")]
        [TagType(TagType.Status)]
        public PmlRemoteInterface RemoteInterface { get; protected set; }
    }
}
