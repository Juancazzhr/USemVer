using System;
using TMPro;
using UnityEngine;

namespace Juancazzhr.Tools.USemVer.Component
{
    [AddComponentMenu("USemVer/Components/Version Display")]
    [RequireComponent(typeof(VersionChecker))]
    public class VersionDisplay : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI labelVersion;
        [SerializeField] private TextMeshProUGUI labelGuid;
        [SerializeField] private TextMeshProUGUI labelMessage;
        [SerializeField] private GameObject updateView;

        [SerializeField] private VersionChecker versionChecker;


        private void Awake()
        {
            versionChecker = GetComponent<VersionChecker>();
            versionChecker.OnCheckVersion += Handle__OnCheckVersion;
        }

        private void Start()
        {
            labelGuid.text = string.Empty;
            labelMessage.text = string.Empty;
            labelVersion.text = string.Empty;
        }

        private void OnDestroy()
        {
            versionChecker.OnCheckVersion -= Handle__OnCheckVersion;
        }

        private void Handle__OnCheckVersion(VersionChecker.TestResult arg)
        {
            labelGuid.text = "...";
            labelMessage.text = "...";
            labelVersion.text = "...";
            
            var (status, localVersion, remoteVersion, buildGuid, message) = arg;

            ColorUtility.TryParseHtmlString("#32cd32", out var passColor);
            ColorUtility.TryParseHtmlString("#ed2839", out var wrongColor);

            if (remoteVersion == default) 
                remoteVersion = new Version(0, 0, 0);

            //labelVersion.color = labelColor;
            labelVersion.text = status == VersionChecker.Status.Updated
                                    ? $"v{arg.LocalVersion}"
                                    : $"Actual: <color=#ed2839>v{localVersion}</color> / Nueva: <color=#32cd32>v{remoteVersion}</color>";
            
            labelVersion.text = status == VersionChecker.Status.Error
                                    ? $"Error: {message}"
                                    : labelVersion.text;

            labelGuid.text = buildGuid.ToString();
            labelMessage.text = message;

            updateView.SetActive(status != VersionChecker.Status.Updated);
        }
    }
}