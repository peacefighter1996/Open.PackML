using System;
using System.Threading.Tasks;

namespace Open.PackML.Interfaces
{
    public interface ITagController
    {
        void ExecutePackTagCommand<G>(string name, G data);
        Task AsyncExecutePackTagCommand<G>(string name, G data);
        void ExecutePackTagCommand<G>(int id, G data);
        Task AsyncExecutePackTagCommand<G>(int id, G data);

        K ExecutePackTagCommand<G, K>(string name, G data);
        Task<K> AsyncExecutePackTagCommand<G, K>(string name, G data);
        K ExecutePackTagCommand<G, K>(int id, G data);
        Task<K> AsyncExecutePackTagCommand<G, K>(int id, G data);
    }
}
