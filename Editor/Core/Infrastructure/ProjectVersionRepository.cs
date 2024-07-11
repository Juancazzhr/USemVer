using System;
using Juancazzhr.Tools.USemVer.Core.Adapter;
using UnityEditor;

namespace Juancazzhr.Tools.USemVer.Editor.Core.Infrastructure
{
    public class ProjectVersionRepository : IVersionRepository
    {
        private const string _DEFAULT_VERSION = "0.0.0";
        
        public ProjectVersionRepository(Version version = null)
        {
            PlayerSettings.bundleVersion = version?.ToString() ?? _DEFAULT_VERSION;
        }
        
        public Version SaveVersion(Version version)
        {
            PlayerSettings.bundleVersion = version.ToString();
            AssetDatabase.Refresh();
            return version;
        }

        public Version GetVersion()
        {
            var version = PlayerSettings.bundleVersion;
            return new Version(version);
        }
    }
}