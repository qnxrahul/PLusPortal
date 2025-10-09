using Microsoft.AspNetCore.Builder;
using Steer73.RockIT;
using Volo.Abp.AspNetCore.TestBase;

var builder = WebApplication.CreateBuilder();
await builder.RunAbpModuleAsync<RockITWebTestModule>();

public partial class Program
{
}
