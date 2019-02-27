using Microsoft.AspNetCore.Mvc;
using Nop.Core.Domain.Catalog;
using Nop.Plugin.Administration.ImportManager.Models;
using Nop.Plugin.Administration.ImportManager.Services;
using Nop.Services.Catalog;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Services.Logging;
using Nop.Services.Security;
using Nop.Web.Framework;
using Nop.Web.Framework.Controllers;
using Nop.Web.Framework.Mvc.Filters;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.XPath;

namespace Nop.Plugin.Administration.ImportManager.Controllers
{
    public class ImportManagerController:BasePluginController
    {
        private readonly IPermissionService _permissionService;
        private readonly ISettingService _settingService;
        private readonly ILocalizationService _localizationService;
        private readonly IYmlImportService _ymlImportService;
        private readonly ILogger _logger;

        public ImportManagerController(
            IPermissionService permissionService,
            ISettingService settingService,
            ILocalizationService localizationService,
            IYmlImportService ymlImportService,
            ILogger logger
            )
        {
            _permissionService = permissionService;
            _settingService = settingService;
            _localizationService = localizationService;
            _ymlImportService = ymlImportService;
            _logger = logger;
        }
        [AuthorizeAdmin]
        [Area(AreaNames.Admin)]
        public IActionResult Configure()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageExternalAuthenticationMethods))
                return AccessDeniedView();

            var settings = _settingService.LoadSetting<ImportManagerSettings>();
            var model = new ConfigurationModel
            {
                MyXslt = settings.MyXslt
            };

            return View("~/Plugins/Administration.ImportManager/Views/Configure.cshtml", model);
        }

        [HttpPost]
        [AdminAntiForgery]
        [AuthorizeAdmin]
        [Area(AreaNames.Admin)]
        public IActionResult Configure(ConfigurationModel model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageExternalAuthenticationMethods))
                return AccessDeniedView();

            if (!ModelState.IsValid)
                return Configure();

            //save settings
            var settings = new ImportManagerSettings
            {
                MyXslt = model.MyXslt
            };

            _settingService.SaveSetting(settings);

            SuccessNotification(_localizationService.GetResource("Admin.Plugins.Saved"));

            return Configure();
        }

        [AuthorizeAdmin]
        [Area(AreaNames.Admin)]
        public IActionResult RunImport()
        {
            var model = new RunImportModel { };
            return View("~/Plugins/Administration.ImportManager/Views/RunImport.cshtml",model);
        }

        [HttpPost]
        [AdminAntiForgery]
        [AuthorizeAdmin]
        [Area(AreaNames.Admin)]
        public IActionResult RunImport(RunImportModel model)
        {
            var path = model.ImportUrl;
            var doc = _ymlImportService.Load(path.AbsoluteUri);
            _ymlImportService.Convert(doc);
            return RunImport();
        }



    }
}
