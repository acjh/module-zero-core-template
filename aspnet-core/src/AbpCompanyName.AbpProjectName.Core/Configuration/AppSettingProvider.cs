﻿using System.Collections.Generic;
using Abp.Configuration;
using Abp.Localization;

namespace AbpCompanyName.AbpProjectName.Configuration
{
    public class AppSettingProvider : SettingProvider
    {
        public static List<SettingDefinitionGroup> Groups = new List<SettingDefinitionGroup>();

        public override IEnumerable<SettingDefinition> GetSettingDefinitions(SettingDefinitionProviderContext context)
        {
            var group1 = new SettingDefinitionGroup("group1", new LocalizableString("group1_displayName", "sourceName"));
            var group2 = new SettingDefinitionGroup("group2", new LocalizableString("group2_displayName", "sourceName"));

            Groups.Add(group1);
            Groups.Add(group2);
            group1.AddChild(group2);

            return new[]
            {
                // Root
                new SettingDefinition(AppSettingNames.UiTheme,
                    "red",
                    scopes: SettingScopes.Application | SettingScopes.Tenant | SettingScopes.User,
                    isVisibleToClients: true
                ),

                // Grouped
                new SettingDefinition("setting1_name",
                    "setting1_defaultValue",
                    group: group1
                ),
                new SettingDefinition("setting2_name",
                    "setting2_defaultValue",
                    group: group2
                )
            };
        }
    }
}
