using Autabee.Utility;
using Open.PackML.EventArguments;
using Open.PackML.Interfaces;
using Open.PackML.Tags;
using Open.PackML.Tags.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Open.PackML.Prefab
{
    public class PmlTr88Controller : PmlGuardController 
    {
        public PmlTr88Controller(IPmlController controller, IPmlEventStore eventStore) : base(controller, eventStore)
        {

        }

        [TagEndUserTerm("OEE.Bad count")]
        [TagType(TagType.Admin)]
        public int ProdDefectiveCount { get; protected set; }
        [TagEndUserTerm("OEE.Bad count")]
        [TagType(TagType.Admin)]
        public int ProdProcessedCount { get; protected set; }

        [TagEndUserTerm("Stop Reason")]
        public PmlStopReason StopReason { get; protected set; } = new PmlStopReason();

    }
}
