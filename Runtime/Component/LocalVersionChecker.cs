using System;
using UnityEngine;

namespace Juancazzhr.Tools.USemVer.Component
{
    internal class LocalVersionChecker : MonoBehaviour
    {
        public Version GetLocalVersion()
        {
            var version = new Version(1, 0, 0);
            var appVersion = Application.version;
            if (Version.TryParse(appVersion, out var parsedVersion)) 
                version = parsedVersion;
            
            return version;
        }
    }
}