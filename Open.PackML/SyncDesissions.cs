using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Open.PackML
{
    public static class SyncDesissions
    {
        public static T SyncDesider<T>(bool preferAsync, Func<T> func, Func<Task<T>> asyncFunc)
        {
            if (preferAsync == true)
            {
                var task = asyncFunc();
                task.Wait();
                return task.Result;
            }
            else
            {
                return func();
            }
        }
        public static async Task<T> asyncSyncDesider<T>(bool preferAsync, Func<T> func, Func<Task<T>> asyncFunc)
        {
            if (preferAsync == true)
            {
                var task = await asyncFunc();
                return task;
            }
            else
            {
                return func();
            }
        }
    }
}
