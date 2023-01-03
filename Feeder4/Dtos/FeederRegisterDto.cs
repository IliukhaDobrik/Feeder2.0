using Feeder4.Enums;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Feeder4.Dtos
{
    public class FeederRegisterDto
    {
        public Guid UserId { get; set; }
        public string Name { get; set; }
        public FeederType Type { get; set; }
    }
}
