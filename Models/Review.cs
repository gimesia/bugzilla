using System;

namespace bugzilla.Models
{
    public class Review
    {
        public Guid Id { get; set; }
        public Fix Fix { get; set; }
        public Developer Dev { get; set; }
        public bool Approved { get; set; }
    }
}