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
            var pathToSave = Path.Combine(_Path, _FILE_NAME);
            var pathExist = Directory.Exists(pathToSave);
            if (pathExist == false)
                Directory.CreateDirectory(_Path);

            File.WriteAllText(pathToSave, version.ToString());
            return version;
        }

        public Version GetVersion()
        {
            var pathToRead = Path.Combine(_Path, _FILE_NAME);
            var pathExist = File.Exists(pathToRead);
            return pathExist == false ? ZeroVersion() : DefaultVersion();
        }

        private static Version ZeroVersion() => new(0, 0, 0);
        private static Version DefaultVersion() => new(1, 0, 0);
    }
}