using System;
using System.Collections.Generic;
using System.Linq;

namespace Bong.Services.Install
{
    public interface  IInstallService
    {
        void InstallData(string defaultUserEmail="admin@bong.com", string defaultUserPassword="admin", bool installSampleData = true);
    }
}
