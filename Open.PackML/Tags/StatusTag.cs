using System;

namespace Open.PackML
{
    public class StatusTag<T> : Tag<T> where T : IComparable
    {
        private T tagValue;

        public StatusTag(string name, string endUserTerm = "", string description = "") : base(name, endUserTerm, description)
        {
            tagValue = default;
        }

        public StatusTag(string name, T Initvalue, string endUserTerm = "", string description = "") : base(name, endUserTerm, description)
        {
            tagValue = Initvalue;
        }
        public override TagType TagType => TagType.Status;

        public T Value { get => tagValue;  set
            {
                if (tagValue.CompareTo(value) != 0)
                {
                    tagValue = value;
                    ValueUpdated?.BeginInvoke(this, value, null, new object());

                }
            }
        }
        public event EventHandler<T> ValueUpdated;

    }


    public class CommandTag<T,K> : Tag<T>
    {
        public CommandTag(string name, IPackMLController<K> machineController, string endUserTerm = "", string description = "") : base(name, endUserTerm, description)
        {
            MachineController = machineController;
        }

        private IPackMLController<K> MachineController;

        public void Execute(T data)
        {
            MachineController.ExecutePackTagCommand<T>(Name, data);
        }
    }
}
