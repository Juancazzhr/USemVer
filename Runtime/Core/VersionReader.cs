using System;
using UnityEngine.Device;

namespace Juancazzhr.Tools.USemVer.Core
{
    internal abstract class VersionReader
    {
        public static Version GetVersion()
        {
            return new Version(Application.version);
        }
    }
}