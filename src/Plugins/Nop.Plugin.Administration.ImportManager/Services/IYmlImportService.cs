using Nop.Plugin.Administration.ImportManager.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nop.Plugin.Administration.ImportManager.Services
{
    public interface IYmlImportService
    {
        YmlDocument Load(string path);
        void Convert(YmlDocument doc);
    }
}
