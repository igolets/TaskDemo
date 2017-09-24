using System.Data.Entity;

namespace TaskDemo.Data.EF
{
    public class TaskContext: DbContext
    {
        public TaskContext() : base("DbConnection")
        {

        }

        public DbSet<Task> Tasks
        {
            get;
            set;
        }

        #region Overrides of DbContext

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //modelBuilder.Entity<Task>()
            //    .HasOptional(x => x.Parent)
            //    .WithMany()
            //    .HasForeignKey(x => x.ParentId);
        }

        #endregion
    }
}
