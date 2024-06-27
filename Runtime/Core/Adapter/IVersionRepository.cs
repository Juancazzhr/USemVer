using System;

namespace Juancazzhr.Tools.USemVer.Core.Adapter
{
    public interface IVersionRepository
    {
        public Version SaveVersion(Version version);
        public Version GetVersion();
    }
}