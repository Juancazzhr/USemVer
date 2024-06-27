using System;
using System.IO;
using Juancazzhr.Tools.USemVer.Core.Adapter;

namespace Juancazzhr.Tools.USemVer.Core.Infrastructure
{
    public class FileVersionRepository : IVersionRepository
    {
        private readonly string _Path;
        private const string _FILE_NAME = "version.txt";

        public FileVersionRepository(string path)
        {
            _Path = path;
        }

        public Version SaveVersion(Version version)
        {
            var completePath = Path.Combine(_Path, _FILE_NAME);
            var exists = Directory.Exists(completePath);
            if (exists == false)
                Directory.CreateDirectory(_Path);

            File.WriteAllText(completePath, version.ToString());
            return version;
        }

        public Version GetVersion()
        {
            return new Version(1, 0, 0);
        }
    }
}