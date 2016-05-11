using System;
using System.Collections.Generic;

using Bong.Core.Configuration;

namespace Bong.Services.Setting
{
    public interface ISettingService
    {
        T LoadSetting<T>() where T : ISettings, new();
    }
}
