namespace Feeder4.Entities
{
    public class Timetable
    {
        public Guid TimetableId { get; set; } 
        public Guid FeederId { get; set; } 
        public string Name { get; set; }
        public List<DateTime> DateTimes { get; set; }
    }
}
