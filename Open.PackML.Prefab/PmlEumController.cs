using Open.PackML.Interfaces;
using Open.PackML.Tags;
using Open.PackML.Tags.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Open.PackML.Prefab
{
    public class PmlEumController : PmlTr88Controller
    {
        public PmlEumController(IPmlController controller, IPmlEventStore eventStore) : base(controller, eventStore)
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
