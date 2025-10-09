using System;

namespace Steer73.RockIT.Shared;

public class GetFileInput
{
    public string DownloadToken { get; set; } = null!;

    public Guid FileId { get; set; }
}