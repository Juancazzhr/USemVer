using System.IO;

namespace Juancazzhr.Tools.USemVer.Core
{
    internal class Parser
    {
        public void SaveVersion(string path, string version)
        {
            if (!FileExists(path))
            {
                CreateFile(path);
            }
            
            File.WriteAllText(path, version);
        }
        
        
        private void CreateFile(string path)
        {
            File.Create(path).Close();
        }

        private bool FileExists(string path)
        {
            return File.Exists(path);
        }
    }
}