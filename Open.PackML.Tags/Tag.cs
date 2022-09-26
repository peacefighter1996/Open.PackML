using System;
using System.Threading.Tasks;

namespace Open.PackML.Tags
{
    //public abstract class Tag<T> : TagConfig
    //{
    //    public Tag(TagConfig tagConfig) : base(tagConfig)
    //    {
    //        DataType = typeof(T);
    //    }

    //}

    //public class StatusTag<T> : DataTag<T> where T : IComparable
    //{
    //    public StatusTag(DataTagConfig dataTagConfig) : base(dataTagConfig)
    //    {
    //    }

    //    public override TagType TagType => TagType.Status;
    //}

    //public class AdminTag<T> : DataTag<T> where T : IComparable
    //{
    //    public AdminTag(DataTagConfig dataTagConfig) : base(dataTagConfig)
    //    {
    //    }

    //    public override TagType TagType => TagType.Admin;
    //}

    //public class DataTagConfig : TagConfig
    //{
    //    public object DefaultValue;
    //}

    //public class DataTag<T> : Tag<T> where T : IComparable
    //{
    //    private T tagValue;


    //    public DataTag(DataTagConfig dataTagConfig) : base(dataTagConfig)
    //    {
    //        tagValue = default;
    //    }


    //    public DataTag(DataTagConfig config, T Initvalue) : base(config)
    //    {
    //        tagValue = Initvalue;
    //    }

    //    public T Value
    //    {
    //        get => tagValue; set
    //        {
    //            if (tagValue.CompareTo(value) != 0)
    //            {
    //                tagValue = value;
    //                ValueUpdated?.BeginInvoke(this, value, null, new object());
    //            }
    //        }
    //    }

    //    public override TagType TagType => throw new NotImplementedException();

    //    public event EventHandler<T> ValueUpdated;

    //}

    //public class CommandTagConfig : TagConfig
    //{
    //    public Type EnumType;
    //    public object value;
    //}

    //public class FunctionCommandTag<T> : Tag<T>
    //{
    //    private readonly object baseObject;

    //    public FunctionCommandTag(object baseObject, TagConfig tagConfig) : base(tagConfig)
    //    {
    //        this.baseObject = baseObject;
    //    }

    //    public override TagType TagType => TagType.Command;

    //    public void Execute(T data)
    //    {
    //        //MachineController.ExecutePackTagCommand(Name, data);
    //    }
    //    public async Task ExecuteAsync(T data)
    //    {
    //        // await MachineController.AsyncExecutePackTagCommand(Name, data);
    //    }
    //}

    //public class DataCommandTag<T, K> : DataTag<T> where K : Enum where T : IComparable
    //{
    //    public DataCommandTag(IPmlController<K> machineController, DataTagConfig tagConfig) : base(tagConfig)
    //    {
    //        MachineController = machineController;
    //    }

    //    private IPmlController<K> MachineController;

    //    public override TagType TagType => TagType.Command;
    //}
}
