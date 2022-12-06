using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Todolist.ContextDb;

/*
 * 
 * @author Vladislav Kozlov <k2v.akosa@gmail.com>
*/
namespace Todolist.Repositories
{
    public class TaskRepository : ITaskRepository
    {
        private ITodolistDbContext _dbContext;

        public TaskRepository(ITodolistDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<TodolistModel> GetTasks(string sortColumn = "", bool descending = false)
        {
            var tasks = _dbContext.Todolists.AsEnumerable();
            sortColumn = sortColumn ?? (sortColumn = "");

            foreach (var prop in typeof(TodolistModel).GetProperties())
            {
                if (sortColumn.IndexOf(prop.Name, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    tasks = descending
                    ? tasks.OrderByDescending(x => prop.GetValue(x, null))
                    : tasks.OrderBy(x => prop.GetValue(x, null));
                    return tasks.ToList();
                }
            }
            return tasks.OrderByDescending(a => a.EnrollmentDate).ToList();
        }

        public bool Search(string taskDescription, int todolistIdOrZero)
        {
            if (todolistIdOrZero == 0)
            {
                int countResult = _dbContext.Todolists.Count(a => a.TaskDescription.ToLower() == taskDescription.ToLower());
                return countResult >= 1 ? true : false;
            }
            else
            {
                int countResultAndId = _dbContext.Todolists.Count(a => a.TaskDescription.ToLower() == taskDescription.ToLower() && a.TodolistId != todolistIdOrZero);
                return countResultAndId >= 1 ? true : false;
            }
        }

        public void Add(TodolistModel todolist)
        {
            _dbContext.Todolists.Add(todolist);
            _dbContext.SaveChanges();
        }

        public void Save()
        {
            _dbContext.SaveChanges();
        }

        public void Remove(TodolistModel todolist)
        {
            _dbContext.Todolists.Remove(todolist);
            _dbContext.SaveChanges();
        }

        public TodolistModel Get(int id)
        {
            return _dbContext.Todolists.SingleOrDefault(a => a.TodolistId == id);
        }
    }
}