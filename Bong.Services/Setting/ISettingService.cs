using System;
using System.Collections.Generic;

using Bong.Core.Configuration;

namespace Bong.Services.Setting
{
    public class SettingService : ISettingService
    {
        public T LoadSetting<T>() where T : ISettings, new()
        {
            return new T();
        }
    }
}
