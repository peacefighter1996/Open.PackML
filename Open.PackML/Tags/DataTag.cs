using Open.PackML.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Open.PackML.Tags
{
    public class DataTag<T> : Tag<T> where T : IComparable
    {
        private T tagValue;


        public DataTag(DataTagConfig dataTagConfig) : base(dataTagConfig)
        {

        }

        public DataTag(string name, string endUserTerm = "", string description = "") : base(name, endUserTerm, description)
        {
            tagValue = default;
        }

        public DataTag(string name, T Initvalue, string endUserTerm = "", string description = "") : base(name, endUserTerm, description)
        {
            tagValue = Initvalue;
        }

        public T Value
        {
            get => tagValue; set
            {
                if (tagValue.CompareTo(value) != 0)
                {
                    tagValue = value;
                    ValueUpdated?.BeginInvoke(this, value, null, new object());
                }
            }
        }

        public override TagType TagType => throw new NotImplementedException();

        public event EventHandler<T> ValueUpdated;

    }
}
