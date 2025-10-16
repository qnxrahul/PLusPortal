using System;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Identity;

namespace Steer73.RockIT.HttpApi.Client.ConsoleTestApp;

public class ClientDemoService : ITransientDependency
{
    private readonly IIdentityUserAppService _identityUserAppService;

    public ClientDemoService(
        IIdentityUserAppService identityUserAppService)
    {
        _identityUserAppService = identityUserAppService;
    }

    public async Task RunAsync()
    {
        var resultDto = await _identityUserAppService.GetListAsync(new GetIdentityUsersInput());
        Console.WriteLine($"Total users: {resultDto.TotalCount}");
        foreach (var identityUserDto in resultDto.Items)
        {
            Console.WriteLine($"- [{identityUserDto.Id}] {identityUserDto.Name}");
        }
    }
}
