using System.Collections.Generic;
using Todolist.ContextDb;

/*
 * 
 * @author Vladislav Kozlov <k2v.akosa@gmail.com>
*/
namespace Todolist.ViewModels
{
    public class TasksVm
    {
        public List<TodolistModel> Tasks { get; set; }
        public IEnumerable<TodolistModel> TasksPage { get; set; }
        public PagingInfo PagingInfoVm { get; set; }
    }
}