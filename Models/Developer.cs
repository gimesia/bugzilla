using System;
using System.ComponentModel.DataAnnotations;

namespace bugzilla.Models
{
    public class Developer
    {
        [Key] public Guid Id { get; set; }
        [Required] public Role Role { get; set; }
        public string Name { get; set; }
    }
}