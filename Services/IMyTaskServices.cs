using MyTaskApp.ClassLibrary;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyTaskApp.Client.Services
{
    public interface IMyTaskServices
    {
        Task<List<SingleTaskModel>> DeleteTasks(List<SingleTaskModel> models);
        Task<List<SingleTaskModel>> GetTasks();
        Task<string> PostMyTask(SingleTaskModel model);
        Task<string> UpdateItem(SingleTaskModel model);
    }
}