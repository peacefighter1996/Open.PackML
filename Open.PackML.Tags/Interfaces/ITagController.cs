using Autabee.Utility;
using Open.PackML.Tags;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Open.PackML.Interfaces
{
    public interface ITagController
    {
        ValidationResult SetTagData(string name, object data);
        ValidationResult<object> GetTagData(string name);
        object[] ExecutePackTagCommand(string name, params object[] args);
        Task<object[]> AsyncExecutePackTagCommand(string name, params object[] args);
        
    }
}
