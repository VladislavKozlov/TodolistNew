using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

/*
 * 
 * @author Vladislav Kozlov <k2v.akosa@gmail.com>
*/
namespace Todolist.ContextDb
{
    public class TodolistModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TodolistId { get; set; }

        [Required]
        [StringLength(100)]
        public string TaskDescription { get; set; }

        public DateTime EnrollmentDate { get; set; }

        public bool Approved { get; set; }
    }
}