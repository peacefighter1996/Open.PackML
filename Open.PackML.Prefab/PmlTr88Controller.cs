using Autabee.Utility;
using Open.PackML.EventArguments;
using Open.PackML.Interfaces;
using Open.PackML.Tags;
using Open.PackML.Tags.Attributes;
using System;

namespace Open.PackML.Prefab
{
    public class PmlTr88Controller : PmlOemGuardController
    {
        public PmlTr88Controller(IPmlController controller, IPmlEventStore eventStore, IPmlOemTransitionCheck oemTransitionCheck) : base(controller, eventStore, oemTransitionCheck)
        {

        }


        [TagEndUserTerm("State")]
        [TagType(TagType.Status)]
        public int StateCurrent { get => (int)currentState; }

        [TagEndUserTerm("Mode")]
        [TagType(TagType.Status)]
        public int UnitModeCurrent { get => (int)currentMode; }


        [TagEndUserTerm("Nominal Speed")]
        [TagType(TagType.Status)]
        public float MachSpeed { get; set; }

        [TagEndUserTerm("Current Speed")]
        [TagType(TagType.Status)]
        public float CurMachSpeed { get; protected set; }

        [TagEndUserTerm("OEE.Bad count")]
        [TagType(TagType.Admin)]
        public int ProdDefectiveCount { get; protected set; }
        [TagEndUserTerm("DEE.Total count")]
        [TagType(TagType.Admin)]
        public int ProdProcessedCount { get; protected set; }

        [TagEndUserTerm("Stop Reason")]
        public PmlStopReason StopReason { get; protected set; } = new PmlStopReason();

        [TagType(TagType.Status)]
        public PmlEquipmentInterlock EquipmentInterlock { get; protected set; } = new PmlEquipmentInterlock();

        [TagType(TagType.Command)]
        [TagEndUserTerm("Command")]
        public int CntrlCmd { get; set; }
        [TagType(TagType.Command)]
        [TagEndUserTerm("Mode")]
        public int UnitMode { get; set; }

        [TagType(TagType.Command)]
        [TagEndUserTerm("Change mode")]
        public bool UnitModeChangeRequest { get => false; set => UpdatePmlMode(UnitMode); }


        [TagType(TagType.Command)]
        [TagEndUserTerm("Mach Speed")]
        public float SetMachSpeed { get; set; }


        [TagType(TagType.Command)]
        [TagEndUserTerm("Change command")]
        public bool CmdChangeRequest
        {
            get => false; 
            set
            {
                if (Enum.IsDefined(typeof(PmlCommand), CntrlCmd))
                    SendPmlCommand((PmlCommand)CntrlCmd);
            }
        }

    }
}
