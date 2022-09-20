using Open.PackML.Interfaces;
using Open.PackML.Prefab;
using System;
using System.Threading.Tasks;

namespace Open.PackML.Tags
{
    public class FunctionCommandTag<T> : Tag<T> 
    {
        private readonly object baseObject;

        public FunctionCommandTag(object baseObject, TagConfig tagConfig) : base(tagConfig)
        {
            this.baseObject = baseObject;
        }

        public override TagType TagType => TagType.Command;

        public void Execute(T data)
        {
            //MachineController.ExecutePackTagCommand(Name, data);
        }
        public async Task ExecuteAsync(T data)
        {
           // await MachineController.AsyncExecutePackTagCommand(Name, data);
        }
    }

    public class DataCommandTag<T, K> : DataTag<T> where K : Enum where T : IComparable
    {
        public DataCommandTag(IPmlController<K> machineController, DataTagConfig tagConfig) : base(tagConfig)
        {
            MachineController = machineController;
        }

        private IPmlController<K> MachineController;

        public override TagType TagType => TagType.Command;
    }
}
