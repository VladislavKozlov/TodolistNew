using System;
using System.ComponentModel.DataAnnotations;

/*
 * 
 * @author Vladislav Kozlov <k2v.akosa@gmail.com>
*/
namespace Todolist.Models
{
    public class TaskInput
    {
        public int TodolistId { get; set; }

        [Required(ErrorMessage = "Task description is required!")]
        [StringLength(100, ErrorMessage = "Task description cannot be more than 100 characters!")]
        public string TaskDescription { get; set; }

        public DateTime EnrollmentDate { get; set; }

        public bool Approved { get; set; }
    }
}