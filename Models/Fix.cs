using System;

namespace bugzilla.Models
{
    public class Fix
    {
        public int Id { get; set; }
        public int BugId { get; set; }
        public int DevId { get; set; }
    }
}