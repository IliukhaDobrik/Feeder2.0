namespace Feeder4.Entities
{
    public class Feeder
    {
        public Guid FeederId { get; set; }
        public Guid? TimetableId { get; set; }
        public bool State { get; set; }
        public string Name { get; set;}
        public string? Mark { get; set;}
    }
}
