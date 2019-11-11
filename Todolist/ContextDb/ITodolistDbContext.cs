using System.Data.Entity;

/*
 * 
 * @author Vladislav Kozlov <k2v.akosa@gmail.com>
*/
namespace Todolist.ContextDb
{
    public interface ITodolistDbContext
    {
        DbSet<TodolistModel> Todolists { get; set; }
        int SaveChanges();
    }
}