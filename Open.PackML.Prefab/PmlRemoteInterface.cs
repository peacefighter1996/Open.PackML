using Open.PackML.Tags.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Open.PackML.Prefab
{
    public class PmlRemoteInterface
    {
        [TagEndUserTerm("Additional production data")]
        public List<PmlParameter> Parameter { get; protected set; } = new List<PmlParameter>();
    }
}
