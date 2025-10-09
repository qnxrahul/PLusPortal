using System;

namespace Steer73.RockIT.JobApplications;

public class GetJobApplicationFileInput
{
    public Guid JobApplicationId { get; set; }

    public string FileType { get; set; } = null!;
}

