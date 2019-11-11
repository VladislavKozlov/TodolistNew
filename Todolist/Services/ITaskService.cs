using Todolist.Models;
using Todolist.ViewModels;

/*
 * 
 * @author Vladislav Kozlov <k2v.akosa@gmail.com>
*/
namespace Todolist.Services
{
    public interface ITaskService
    {
        TasksVm GetTasks(string sortColumn = "", bool descending = false);
        void Add(TaskInput taskInput);
        void Edit(TaskInput taskInput);
        void Remove(int id);
        TaskVm Get(int id);
        bool SearchTaskDescription(string taskDescription, int todolistIdOrZero);
    }
}
