using System;
using Juancazzhr.Tools.USemVer.Core.Adapter;

namespace Juancazzhr.Tools.USemVer.Core.Domain
{
    /*
     * Incrementar la versión de la aplicación en la configuración y el archivo local (version.txt) ubicado en el directorio resources.
     *  - Verificar si la versión es inferior a la actual. (Mostar mensaje de advertencia).
     *  - Verificar la existencia del archivo de versión (version.txt) y el directorio resources.
     *      - SI no existe, crear el archivo y el directorio.
     *
     *  - SI existe, leer el archivo y comparar la versión almacenada en el PlayerSettings.
     *  - SI son diferentes, mostrar mensaje de advertencia.
     *
     * Guardar el archivo de versión (version.txt) en la carpeta de la build.
     *  - Revisar para permitir que el usuario elija la ubicación del archivo de versión (version.txt).
     */

    public class SemVerMediator
    {
        private readonly VersionSaver _VersionSaver;
        private readonly VersionReader _VersionReader;

        public SemVerMediator(IVersionRepository localFileRepository,
                        IVersionRepository remoteFileRepository,
                        IVersionRepository projectRepository)
        {
            _VersionSaver = new VersionSaver(localFileRepository, remoteFileRepository, projectRepository);
        }

        public void SaveVersion(Version version)
        {
            _VersionSaver.Save(version);
        }
    }
}