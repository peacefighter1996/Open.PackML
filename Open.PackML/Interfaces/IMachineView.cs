using System;
using System.Collections.Generic;
using System.Text;

namespace Open.PackML
{
    public interface IMachineView
    {
        void MachineEvent(int Id, DateTime dateTime);
    }
}
