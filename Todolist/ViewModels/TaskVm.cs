using System;
using System.ComponentModel.DataAnnotations;
using Todolist.ContextDb;

/*
 * 
 * @author Vladislav Kozlov <k2v.akosa@gmail.com>
*/
namespace Todolist.ViewModels
{
    public class TaskVm
    {
        public int TodolistId { get; set; }

        [Display(Name = "Description")]
        public string TaskDescription { get; set; }

        public DateTime EnrollmentDate { get; set; }

        [Display(Name = "Status")]
        public bool Approved { get; set; }

        public string Title { get; set; }

        public TaskVm()
        {
        }

        public TaskVm(TodolistModel todolistModel)
        {
            TodolistId = todolistModel.TodolistId;
            TaskDescription = todolistModel.TaskDescription;
            EnrollmentDate = todolistModel.EnrollmentDate;
            Approved = todolistModel.Approved;
        }
    }
}