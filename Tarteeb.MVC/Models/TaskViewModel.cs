using System;

namespace Tarteeb.MVC.Models
{
    public class TaskViewModel
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public DateTime? Deadline { get; set; }
    }
}
