﻿namespace TaskDemo.Web.Models
{
    public class TaskDeleteModel
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

        public string Message
        {
            get;
            set;
        }
    }
}