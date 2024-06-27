using TMPro;
using UnityEngine;

namespace Juancazzhr.Tools.USemVer.Component
{
    [AddComponentMenu("USemVer/Components/U Version Display")]
    [RequireComponent(typeof(VersionChecker))]
    public class UVersionDisplay : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI labelVersion;
        [SerializeField] private TextMeshProUGUI labelGuid;
        [SerializeField] private TextMeshProUGUI labelMessage;
        [SerializeField] private VersionChecker versionChecker;


        private void Awake()
        {
            versionChecker = GetComponent<VersionChecker>();
            versionChecker.OnCheckVersion += Handle__OnCheckVersion;
        }

        private void OnDestroy()
        {
            versionChecker.OnCheckVersion -= Handle__OnCheckVersion;
        }

        private void Handle__OnCheckVersion(VersionChecker.TestResult arg)
        {
            var (alreadyUpdated, localVersion, remoteVersion, buildGuid, message) = arg;

            var labelColor = alreadyUpdated ? Color.green : Color.red;

            labelVersion.color = labelColor;
            labelVersion.text = $"v{arg.LocalVersion}";
            
            labelGuid.text = buildGuid.ToString();
            labelMessage.text = message;
        }
    }
}