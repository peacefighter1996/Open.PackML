using Open.PackML.Interfaces;
using Open.PackML.Prefab;
using System;
using System.Threading.Tasks;

namespace Open.PackML.Tags
{
    public class FunctionCommandTag<T, K> : Tag<T> where K : Enum
    {
        public FunctionCommandTag(PackMLController<K> machineController, TagConfig tagConfig) : base(tagConfig)
        {
            MachineController = machineController;
        }

        private IPackMLController<K> MachineController;

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
        public DataCommandTag(IPackMLController<K> machineController, DataTagConfig tagConfig) : base(tagConfig)
        {
            MachineController = machineController;
        }

        private IPackMLController<K> MachineController;

        public override TagType TagType => TagType.Command;
    }
}
