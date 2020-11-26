using System;

namespace bugzilla.Models
{
    public class Developer
    {
        public Guid Id { get; set; }
        public Role Role { get; set; }
        public string Name { get; set; }
    }
}