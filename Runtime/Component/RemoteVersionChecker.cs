using System;
using Cysharp.Threading.Tasks;
using Juancazzhr.Tools.USemVer.Common;
using UnityEngine;
using UnityEngine.Networking;

namespace Juancazzhr.Tools.USemVer.Component
{
    public class RemoteVersionChecker : MonoBehaviour
    {
        [SerializeField] private string host = "http://localhost:8080";
        [SerializeField] private string path = $"/udearroba/creacion_digital/server/version.txt";

        internal async UniTask<Result<Version>> GetRemoteVersion()
        {
            Version remoteVersion;
            var endpoint = new Uri($"{host}{path}");
            using var webRequest = UnityWebRequest.Get(endpoint);

            try
            {
                var webResponse = await webRequest.SendWebRequest();
                var parsedVersion = ParseVersion(webResponse.downloadHandler.text);
                remoteVersion = parsedVersion;
            }
            catch (UnityWebRequestException e)
            {
                return Result<Version>.Failure(e.Message);
            }

            return Result<Version>.Success(remoteVersion);
        }

        private static Version ParseVersion(string version)
        {
            return Version.TryParse(version, out var parsedVersion) ? parsedVersion : new Version(1, 0, 0);
        }
    }
}