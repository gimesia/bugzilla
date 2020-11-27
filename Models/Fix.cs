using System;
using System.ComponentModel.DataAnnotations;

namespace bugzilla.Models
{
    public class Fix
    {
        [Key] public Guid Id { get; set; }
        [Required] public Bug Bug { get; set; }
        [Required] public Developer Dev { get; set; }
    }
}