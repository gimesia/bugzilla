using System;
using System.ComponentModel.DataAnnotations;

namespace bugzilla.Models
{
    public class Fix
    {

        public Guid Id { get; set; }
        public Bug Bug { get; set; }
        public Developer Dev { get; set; }
    }
}