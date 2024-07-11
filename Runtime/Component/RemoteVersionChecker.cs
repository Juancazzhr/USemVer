using System;
using Cysharp.Threading.Tasks;
using Juancazzhr.Tools.USemVer.Common;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Networking;

namespace Juancazzhr.Tools.USemVer.Component
{
    internal class RemoteVersionChecker : MonoBehaviour
    {
        [SerializeField] private string host = string.Empty;
        [SerializeField] private string path = string.Empty;

        private void OnValidate()
        {
            Assert.IsFalse(string.IsNullOrEmpty(host), $"'{nameof(host)}' is required");
            Assert.IsFalse(string.IsNullOrEmpty(path), $"'{nameof(path)}' is required");
        }

        internal async UniTask<Result<Version>> GetRemoteVersion()
        {
            Version remoteVersion;
            var endpoint = new Uri($"{host}{path}", UriKind.RelativeOrAbsolute);
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
            return Version.TryParse(version, out var parsedVersion) ? parsedVersion : new Version(0, 0, 0);
        }
    }
}