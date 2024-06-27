using System;
using Juancazzhr.Tools.USemVer.Core.Adapter;

namespace Juancazzhr.Tools.USemVer.Core.Domain
{
    internal class VersionSaver
    {
        private readonly IVersionRepository _LocalFileRepository;
        private readonly IVersionRepository _RemoteFileRepository;
        private readonly IVersionRepository _ProjectRepository;

        public VersionSaver(IVersionRepository localFileRepository, IVersionRepository remoteFileRepository,
                            IVersionRepository projectRepository)
        {
            _LocalFileRepository = localFileRepository;
            _RemoteFileRepository = remoteFileRepository;
            _ProjectRepository = projectRepository;
        }

        public void Save(Version version)
        {
            _LocalFileRepository.SaveVersion(version);
            _RemoteFileRepository.SaveVersion(version);
            _ProjectRepository.SaveVersion(version);
        }
    }
}