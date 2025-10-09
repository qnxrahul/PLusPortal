using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using Steer73.RockIT.Localization;
using Steer73.RockIT.Permissions;
using Steer73.RockIT.MultiTenancy;
using Volo.Abp.SettingManagement.Web.Navigation;
using Volo.Abp.Authorization.Permissions;
// using Volo.Abp.Identity.Web.Navigation; // removed for no-auth/OSS
using Volo.Abp.UI.Navigation;

namespace Steer73.RockIT.Web.Menus;

public class RockITMenuContributor : IMenuContributor
{
    public async Task ConfigureMenuAsync(MenuConfigurationContext context)
    {
        if (context.Menu.Name == StandardMenus.Main)
        {
            await ConfigureMainMenuAsync(context);
        }
    }

    private static Task ConfigureMainMenuAsync(MenuConfigurationContext context)
    {
        var l = context.GetLocalizer<RockITResource>();

        //Home
        context.Menu.AddItem(
            new ApplicationMenuItem(
                RockITMenus.Home,
                l["Menu:Home"],
                "~/Admin/Home",
                icon: "fa fa-home",
                order: 1
            )
        );

        context.Menu.AddItem(
            new ApplicationMenuItem(
                RockITMenus.Companies,
                l["Menu:Companies"],
                url: "/Companies",
                icon: "fa fa-building",
                order: 3,
                requiredPermissionName: RockITSharedPermissions.Companies.Default)
        );

        //Administration
        var administration = context.Menu.GetAdministration();
        administration.Order = 5;

        //Administration->Identity removed

        //Administration->Settings
        administration.SetSubItemOrder(SettingManagementMenuNames.GroupName, 7);

        //Administration->Configuration
        administration.AddItem(
            new ApplicationMenuItem(RockITMenus.Configuration, l["Menu:Configuration"], order: 2)
                    .AddItem(new ApplicationMenuItem(
                        RockITMenus.PracticeGroups,
                        l["Menu:PracticeGroups"],
                        url: "/Sectors",
                        icon: "fa fa-file-alt",
                        requiredPermissionName: RockITSharedPermissions.PracticeGroups.Default)
                    ).AddItem(new ApplicationMenuItem(
                        RockITMenus.PracticeAreas,
                        l["Menu:PracticeAreas"],
                        url: "/Sector-Areas",
                        icon: "fa fa-file-alt",
                        requiredPermissionName: RockITSharedPermissions.PracticeAreas.Default)
                     ));

        context.Menu.AddItem(
            new ApplicationMenuItem(
                RockITMenus.Vacancies,
                l["Menu:Vacancies"],
                url: "/Vacancies",
icon: "fa fa-file-alt",
                requiredPermissionName: RockITSharedPermissions.Vacancies.Default)
        );

        context.Menu.AddItem(
            new ApplicationMenuItem(
                RockITMenus.FormDefinitions,
                l["Menu:FormDefinitions"],
                url: "/FormDefinitions",
icon: "fa fa-file-alt",
                requiredPermissionName: RockITSharedPermissions.FormDefinitions.Default)
        );

        return Task.CompletedTask;
    }
}