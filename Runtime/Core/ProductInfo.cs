using System;
using Application = UnityEngine.Device.Application;

namespace Juancazzhr.Tools.USemVer.Core
{
    public abstract class ProductInfo
    {
        public static Version GetVersion()
        {
            return new Version(Application.version);
        }
    }
}