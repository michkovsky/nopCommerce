using Nop.Core.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nop.Plugin.Administration.ImportManager
{
    public class ImportManagerSettings:ISettings
    {
        public bool IsDebugMode { get; set; }
        
        public string MyXslt { get; set; }
        
    }
}
