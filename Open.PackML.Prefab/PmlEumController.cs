using Open.PackML.Tags;
using Open.PackML.Tags.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Open.PackML.Prefab
{
    public class PmlEumController : PmlTr88Controller
    {
        public PmlEumController(IPmlController controller, IPmlEventStore eventStore, IPmlOemTransitionCheck oemTransitionCheck) : base(controller, eventStore, oemTransitionCheck)
        {

        }

        [TagEndUserTerm("Warning")]
        [TagType(TagType.Admin)]
        public List<PmlWarning> Warning { get; protected set; } = new List<PmlWarning>();


        [TagEndUserTerm("Machine data/parameter")]
        [TagType(TagType.Status | TagType.Command)]
        public List<PmlParameter> Parameter { get; protected set; } = new List<PmlParameter>();

        [TagEndUserTerm("Parameter")]
        [TagType(TagType.Status | TagType.Command)]
        public PmlRemoteInterface RemoteInterface { get; protected set; }
    }
}
