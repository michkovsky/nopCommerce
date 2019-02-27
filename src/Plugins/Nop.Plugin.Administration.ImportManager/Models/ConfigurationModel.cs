using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nop.Plugin.Administration.ImportManager.Models
{
    public class ConfigurationModel:BaseNopModel
    {
        [NopResourceDisplayName("My xslt value")]
        public string MyXslt { get; set; }
    }
}
