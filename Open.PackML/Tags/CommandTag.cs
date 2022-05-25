using System;
using System.Threading.Tasks;

namespace Open.PackML.Tags
{
    public class CommandTag<T, K> : Tag<T> where K : Enum
    {
        public CommandTag(string name, IPackMLController<K> machineController, string endUserTerm = "", string description = null) : base(name, endUserTerm, description)
        {
            MachineController = machineController;
        }

        private IPackMLController<K> MachineController;

        public override TagType TagType => TagType.Command;

        public void Execute(T data)
        {
            MachineController.ExecutePackTagCommand(Name, data);
        }
        public async Task ExecuteAsync(T data)
        {
            MachineController.AsyncExecutePackTagCommand(Name, data);
        }
    }
}
