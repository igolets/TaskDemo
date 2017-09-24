using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskDemo.Data.EF
{
    [Table("Task")]
    public class Task
    {
        [Required]
        public int Id
        {
            get;
            set;
        }

        [Required]
        [StringLength(250)]
        public string Name
        {
            get;
            set;
        }

        public int? ParentId
        {
            get;
            set;
        }

        [ForeignKey("ParentId")]
        public virtual Task Parent
        {
            get;
            set;
        }

        [ForeignKey("ParentId")]
        public virtual ICollection<Task> Children
        {
            get;
            set;
        }
    }
}
