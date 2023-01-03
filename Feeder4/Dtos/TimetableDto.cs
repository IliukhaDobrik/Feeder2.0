namespace Feeder4.Dtos
{
    public class TimetableDto
    {
        public string Name { get; set; }
        public Guid FeederId { get; set; }
        public List<DateTime> DateTimes { get; set; }
    }
}
