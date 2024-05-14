using Open.PackML.Tags.Attributes;

namespace Open.PackML.Prefab
{
    public class PmlEquipmentInterlock
    {
        [TagEndUserTerm("Blockage")]
        public bool Blocker { get; set; }

        [TagEndUserTerm("Starvation")]
        public bool Starve { get; set; }
    }
}
