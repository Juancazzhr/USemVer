using System;

namespace Juancazzhr.Tools.USemVer.Core
{
    public class SemVerSystem
    {
        private static SemVerSystem _instance;
        public static SemVerSystem Instance => _instance ??= new SemVerSystem();

        private SemVerSystem() => Version = DefaultVersion;
        
        public static Version DefaultVersion => new(0, 1, 0);
        public Version Version { get; private set; }

        private Version _NewVer;


        public Version Increment(VersionPart part)
        {
            _NewVer = part switch
            {
                VersionPart.Major => new Version(Version.Major + 1, 0, 0),
                VersionPart.Minor => new Version(Version.Major, Version.Minor + 1, 0),
                VersionPart.Patch => new Version(Version.Major, Version.Minor, Version.Build + 1),
                _             => DefaultVersion
            };

            Version = _NewVer;
            return Version;
        }

        public Version Reset()
        {
            Version = DefaultVersion;
            return Version;
        }
    }
}