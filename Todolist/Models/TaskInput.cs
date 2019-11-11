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

        [Required(ErrorMessage = "Описание задачи должно быть заполнено!")]
        [StringLength(100, ErrorMessage = "Описание не может быть более длинным чем 100 символов!")]
        public string TaskDescription { get; set; }

        public DateTime EnrollmentDate { get; set; }

        public bool Approved { get; set; }
    }
}