using System.ComponentModel.DataAnnotations;

namespace TaskDemo.Web.Models
{
    public class TaskCreateModel
    {
        public int? ParentId
        {
            get;
            set;
        }

        [Display(Name = "Task Name")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Task name is required")]
        [MaxLength(250, ErrorMessage = "Task name should not have more than 250 chars")]
        public string Name
        {
            get;
            set;
        }
    }
}