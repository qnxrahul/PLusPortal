using System;

namespace Steer73.RockIT.JobApplications;

public abstract class JobApplicationDownloadTokenCacheItemBase
{
    public string Token { get; set; } = null!;
}