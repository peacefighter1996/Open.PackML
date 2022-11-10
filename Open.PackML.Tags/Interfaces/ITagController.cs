using Autabee.Utility;
using Open.PackML.Tags;
using System;
using System.Collections.Generic;

namespace Open.PackML.Interfaces
{
    public interface ITagController
    {

        //Task<ValidationResult<object>> AsyncExecutePackTagCommand(string tagName, params object[] args);


        ValidationResult<object> ExecutePackTagCommand(string tagName, params object[] args);
        ValidationResult<object>[] ExecutePackTagCommand(List<(string, object[])> data);


        ValidationResult<object> GetTagData(string tagName);
        ValidationResult<T> GetTagData<T>(string tagName);

        ValidationResult SetTagData(string tagName, object data);
        ValidationResult SetTagData<T>(string tagName, T data);

        ValidationResult<object> TagCall(TagCall tagCall);

        ValidationResult[] BatchSetTagData(List<(string, object)> data);
        ValidationResult<object>[] BatchGetTagData(List<string> tagName);
        ValidationResult<object>[] BatchCall(List<TagCall> tagCalls);
        //ValidationResult<object>[] BatchCall(BatchTagCall tagCalls);
    }
}
