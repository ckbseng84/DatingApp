using System;

namespace DatingApp.API.Models
{
    public class Photo
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public string Description { get; set; }
        public DateTime MyProperty { get; set; }
        public bool IsMain { get; set; }
        public virtual User User { get; set; } // require to declare virtual
        public int UserId { get; set; }

    }
}