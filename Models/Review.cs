using System;
using System.ComponentModel.DataAnnotations;

namespace bugzilla.Models
{
    public class Review
    {
        [Key] public Guid Id { get; set; }
        [Required] public Fix Fix { get; set; }
        [Required] public Developer Dev { get; set; }
        public bool Approved { get; set; }
    }
}