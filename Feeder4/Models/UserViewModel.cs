using Feeder4.Entities;

namespace Feeder4.Models
{
    public class UserViewModel
    {
        public Guid UserId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        //public List<Guid> FeedersId { get; set; }
        //public List<FeederWithDispenser> FeedersWithDispensers { get; set; } = new List<FeederWithDispenser>();
        //public List<FeederWithWeightSensor> FeedersWithWeightSensor { get; set; } = new List<FeederWithWeightSensor>();
        public List<Guid> FeedersId { get; set; } = new List<Guid>();
    }
}
