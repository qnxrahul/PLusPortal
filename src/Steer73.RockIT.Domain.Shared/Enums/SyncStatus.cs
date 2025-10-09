using System.ComponentModel;

namespace Steer73.RockIT.Enums;
public enum SyncStatus
{
    [Description("Pending")] 
    Pending = 1,
    [Description("Synced")]
    Synced = 2,
    [Description("Error")] 
    Error = 3,
}

