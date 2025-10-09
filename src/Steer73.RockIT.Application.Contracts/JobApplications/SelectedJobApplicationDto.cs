using System;
using Volo.Abp.Domain.Entities;

namespace Steer73.RockIT.JobApplications;

public class SelectedJobApplicationDto : IHasConcurrencyStamp
{
	public Guid Id { get; set; }

	public string ConcurrencyStamp { get; set; } = null!;
}


