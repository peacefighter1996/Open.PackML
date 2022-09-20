using System;
using System.Threading.Tasks;

namespace Autabee.Utility
{
    //public static class SyncDecisions
    //{
    //    public static T SyncDecider<T>(bool preferAsync, Func<T> func, Func<Task<T>> asyncFunc)
    //    {
    //        if (preferAsync == true)
    //        {
    //            var task = asyncFunc();
    //            task.Wait();
    //            return task.Result;
    //        }
    //        else
    //        {
    //            return func();
    //        }
    //    }
    //    public static async Task<T> asyncSyncDecider<T>(bool preferAsync, Func<T> func, Func<Task<T>> asyncFunc)
    //    {
    //        if (preferAsync == true)
    //        {
    //            var task = await asyncFunc();
    //            return task;
    //        }
    //        else
    //        {
    //            return func();
    //        }
    //    }
    //}
}
