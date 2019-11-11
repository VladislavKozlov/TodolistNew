using System.Collections.Generic;
using Todolist.ContextDb;

/*
 * 
 * @author Vladislav Kozlov <k2v.akosa@gmail.com>
*/
namespace Todolist.Repositories
{
    public interface ITaskRepository
    {
        List<TodolistModel> GetTasks(string sortColumn = "", bool descending = false);
        void Add(TodolistModel todolist);
        void Save();
        void Remove(TodolistModel todolist);
        TodolistModel Get(int id);
        bool Search(string taskDescription, int todolistIdOrZero);
    }
}
