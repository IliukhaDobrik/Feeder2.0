using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Feeder4.Enums
{
    public enum FeederType
    {
        [Display(Name = "WithDispenser")]
        WithDispenser,

        [Display(Name = "WithWeightSensor")]
        WithWeightSensor
    }
}
