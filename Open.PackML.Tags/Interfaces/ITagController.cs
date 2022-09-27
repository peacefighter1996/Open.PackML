using Autabee.Utility;
using Open.PackML.Tags;
using Open.PackML.Tags.Prefab;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Open.PackML.Interfaces
{
    public interface ITagController
    {

        //Task<ValidationResult<object>> AsyncExecutePackTagCommand(string name, params object[] args);

        
        ValidationResult<object> ExecutePackTagCommand(string name, params object[] args);
        ValidationResult<object>[] ExecutePackTagCommand(List<(string, object[])> data);

        
        ValidationResult<object> GetTagData(string name);
        ValidationResult<T> GetTagData<T>(string name);
        
        ValidationResult SetTagData(string name, object data);
        ValidationResult SetTagData<T>(string name, T data);
        
        ValidationResult<object> TagCall(TagCall tagCall);

        ValidationResult[] BatchSetTagData(List<(string, object)> data);
        ValidationResult<object>[] BatchGetTagData(List<string> name);
        ValidationResult<object>[] BatchCall(List<TagCall> tagCalls);
    }
}
