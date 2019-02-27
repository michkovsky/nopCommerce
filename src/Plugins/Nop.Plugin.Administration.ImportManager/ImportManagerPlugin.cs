using Microsoft.AspNetCore.Routing;
using Nop.Core;
using Nop.Core.Plugins;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Services.Logging;
using Nop.Web.Framework.Menu;
using System;
using System.Linq;

namespace Nop.Plugin.Administration.ImportManager
{
    
    public class ImportManagerPlugin : BasePlugin, IAdminMenuPlugin
    {
        private readonly ILocalizationService _localizationService;
        private readonly ISettingService _settingService;
        private readonly ILogger _logger;
        private readonly IWebHelper _webHelper;
        public ImportManagerPlugin(ILocalizationService localizationService,
            ISettingService settingsService,
            ILogger logger,
            IWebHelper webHelper
            )
        {
            _localizationService = localizationService;
            _settingService = settingsService;
            _logger = logger;
            _webHelper = webHelper;
        }

        public override string GetConfigurationPageUrl()
        {
            var url = $"{_webHelper.GetStoreLocation()}Admin/ImportManager/Configure";
            _logger.Information(url);
            return url;
        }

        public override void Install()
        {
            _logger.Information($"Begin install {GetType()}");
            var settings = new ImportManagerSettings
            {
                IsDebugMode = true,
            };
            _logger.Information($"Write settings {settings}");
            _settingService.SaveSetting(settings);
            
            base.Install();
            _logger.Information($"Installation of {GetType()} complete");


        }

        public void ManageSiteMap(SiteMapNode rootNode)
        {
            var menuItem = new SiteMapNode()
            {
                SystemName = "ImportManager",
                Title = "Import Manager",
                ControllerName = "ImportManager",
                ActionName = "RunImport",
                Visible = true,
                RouteValues = new RouteValueDictionary() { { "area", "Admin" } },
            };
            var pluginNode = rootNode.ChildNodes.FirstOrDefault(x => x.SystemName == "Third party plugins");
            if (pluginNode != null)
                pluginNode.ChildNodes.Add(menuItem);
            else
                rootNode.ChildNodes.Add(menuItem);
        }

        public override void Uninstall()
        {
            _settingService.DeleteSetting<ImportManagerSettings>();
            base.Uninstall();
        }
    }
}
