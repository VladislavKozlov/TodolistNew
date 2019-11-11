using System.Data.Entity;

/*
 * 
 * @author Vladislav Kozlov <k2v.akosa@gmail.com>
*/
namespace Todolist.ContextDb
{
    public class TodolistDbContext : DbContext, ITodolistDbContext
    {
        public DbSet<TodolistModel> Todolists { get; set; }

        public override int SaveChanges()
        {
            return base.SaveChanges();
        }
    }
}