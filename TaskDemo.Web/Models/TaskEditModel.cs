using System;
using System.ComponentModel.DataAnnotations;

namespace TaskDemo.Web.Models
{
    public class TaskEditModel
    {
        public int Id
        {
            get;
            set;
        }

        public int? ParentId
        {
            get;
            set;
        }

        [Display(Name = "Task Name")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Task name is required")]
        [MaxLength(250, ErrorMessage = "Task name should not have more than 250 chars")]
        public String Name
        {
            get;
            set;
        }
    }
}