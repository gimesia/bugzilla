using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace bugzilla.Models
{
    public class Bug
    {
        [Key] public Guid Id { get; set; }
        [Required] public Developer Dev { get; set; }
        public string Description { get; set; }
        public bool Closed { get; set; }
    }
}