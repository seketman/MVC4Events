using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace MVC4Events.Models
{
    [Table("Events")]
    public class EventModel
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Technology { get; set; }
        public DateTime StartingDate { get; set; }
        public string RegistrationLink { get; set; }
    }
}