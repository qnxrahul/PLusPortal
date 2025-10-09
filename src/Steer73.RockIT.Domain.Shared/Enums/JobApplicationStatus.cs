using System.ComponentModel;

namespace Steer73.RockIT.Enums;
public enum JobApplicationStatus
{
    [Description("Pending")] 
    Pending = 1,
    [Description("Synced")]
    Approved = 2,
    [Description("Error")] 
    Rejected = 3,
}

