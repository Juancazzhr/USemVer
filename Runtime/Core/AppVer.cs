using System;
using Application = UnityEngine.Device.Application;

namespace Juancazzhr.Tools.USemVer.Core
{
    public abstract class AppVer
    {
        public static Version GetVersion()
        {
            var semVerSystem = SemVerSystem.Instance;
            var appVer = Application.version;
            return string.IsNullOrEmpty(appVer)
                       ? SemVerSystem.DefaultVersion
                       : new Version(appVer);
        }
    }
}