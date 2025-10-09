using Volo.Abp.Identity;

namespace Steer73.RockIT;

public static class RockITConsts
{
    public const string DbTablePrefix = "App";
    public const string? DbSchema = null;
    public const string DiversityDbSchema = "diversity";
    public const string AdminEmailDefaultValue = IdentityDataSeedContributor.AdminEmailDefaultValue;
    public const string AdminPasswordDefaultValue = IdentityDataSeedContributor.AdminPasswordDefaultValue;
}
