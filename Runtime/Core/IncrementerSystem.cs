using System;

namespace Juancazzhr.Tools.USemVer.Core
{
    public class IncrementerSystem
    {
        private Version _NewVer;

        public IncrementerSystem(Version currentVersion)
        {
            Version = currentVersion;
        }

        public Version Version { get; private set; }

        public Version Increment(VersionPart part)
        {
            _NewVer = part switch
            {
                VersionPart.Major => new Version(Version.Major + 1, 0, 0),
                VersionPart.Minor => new Version(Version.Major, Version.Minor + 1, 0),
                VersionPart.Patch => new Version(Version.Major, Version.Minor, Version.Build + 1),
                _             => new Version(0, 0, 0)
            };

            Version = _NewVer;
            return Version;
        }

        public Version Reset()
        {
            Version = new Version(1, 0, 0);
            return Version;
        }
    }
}