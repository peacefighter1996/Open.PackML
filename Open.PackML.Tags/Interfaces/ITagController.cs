using Autabee.Utility;
using Open.PackML.Tags;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Open.PackML.Interfaces
{
    public interface ITagController
    {
        ValidationResult SetTagData<T>(string name, T data);
        ValidationResult SetTagData(string name, object data);
        ValidationResult<T> GetTagData<T>(string name);
        ValidationResult<object> GetTagData(string name);
        ValidationResult<object> ExecutePackTagCommand(string name, params object[] args);
        Task<ValidationResult<object>> AsyncExecutePackTagCommand(string name, params object[] args);
        
    }
}
