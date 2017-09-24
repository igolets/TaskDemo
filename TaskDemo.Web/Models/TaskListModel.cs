namespace TaskDemo.Web.Models
{
    public class TaskListModel
    {
        public int? Id
        {
            get;
            set;
        }

        public int? ParentId
        {
            get;
            set;
        }

        public TaskModel[] Tasks
        {
            get;
            set;
        }
    }
}