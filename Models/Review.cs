namespace bugzilla.Models
{
    public class Review
    {
        public int Id { get; set; }
        public int FixId { get; set; }
        public int DevId { get; set; }
        public bool Approved { get; set; }
    }
}