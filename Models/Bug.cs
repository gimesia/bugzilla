using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace bugzilla.Models
{
    public class Bug
    {
        public int Id { get; set; }
        public int DevId { get; set; }
        public bool Closed { get; set; }
    }
}