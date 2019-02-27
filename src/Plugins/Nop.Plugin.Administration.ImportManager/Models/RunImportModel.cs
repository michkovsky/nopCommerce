using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nop.Plugin.Administration.ImportManager.Models
{
    public class RunImportModel : BaseNopModel
    {
        [NopResourceDisplayName("Import data url")]
        public Uri ImportUrl { get; set; }
    }
}
