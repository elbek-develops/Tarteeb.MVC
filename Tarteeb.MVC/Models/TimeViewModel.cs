using System;

namespace Tarteeb.MVC.Models
{
    public class TimeViewModel
    {
        public int Id { get; set; }
        public int TaskId { get; set; }
        public string Comment { get; set; }
        public DateTime Date { get; set; }
        public float WorkHour { get; set; }
    }
}
