using System;
using UnityEngine;
using UnityEngine.Events;

namespace Juancazzhr.Tools.USemVer.Component
{
    [RequireComponent(typeof(RemoteVersionChecker), typeof(LocalVersionChecker))]
    [DefaultExecutionOrder(-1)]
    public class VersionChecker : MonoBehaviour
    {
        /// <summary>
        /// Struct to store the result of the version check.
        /// </summary>
        public struct TestResult
        {
            /// <summary>
            /// If the version is already updated.
            /// </summary>
            public bool IsUpdated;

            /// <summary>
            /// The local version.
            /// </summary>
            public Version LocalVersion;

            /// <summary>
            /// The remote version.
            /// </summary>
            public Version RemoteVersion;

            /// <summary>
            /// GUID of the build.
            /// </summary>
            public Guid BuildGuid;

            /// <summary>
            /// Message to display.
            /// </summary>
            public string Message;

            public void Deconstruct(out bool alreadyUpdated,
                                    out Version localVersion,
                                    out Version remoteVersion,
                                    out Guid buildGuid,
                                    out string message)
            {
                alreadyUpdated = IsUpdated;
                localVersion = LocalVersion;
                remoteVersion = RemoteVersion;
                buildGuid = BuildGuid;
                message = Message;
            }
        }

        [SerializeField] private RemoteVersionChecker remoteVersionChecker;
        [SerializeField] private LocalVersionChecker localVersionChecker;

        /// <summary>
        /// Event triggered when the version is checked.
        /// </summary>
        [SerializeField] private UnityEvent<TestResult> onCheckVersion;

        public event UnityAction<TestResult> OnCheckVersion
        {
            add => onCheckVersion.AddListener(value);
            remove => onCheckVersion.RemoveListener(value);
        }


        private void Awake()
        {
            remoteVersionChecker = GetComponent<RemoteVersionChecker>();
            localVersionChecker = GetComponent<LocalVersionChecker>();
        }

        private async void Start()
        {
            var remoteVer = await remoteVersionChecker.GetRemoteVersion();
            var localVer = localVersionChecker.GetLocalVersion();

            var message = "";
            var alreadyUpdated = false;

            if (remoteVer.IsFailure)
            {
                message = remoteVer.Error;
                onCheckVersion?.Invoke(new TestResult
                {
                    IsUpdated = false,
                    LocalVersion = localVer,
                    RemoteVersion = null,
                    BuildGuid = Guid.Parse(Application.buildGUID),
                    Message = message
                });
                return;
            }

            if (localVer == remoteVer.Value)
            {
                message = "The version is up to date.";
                alreadyUpdated = true;
            }
            else if (localVer < remoteVer.Value)
                message = "There is a new version available.";
            else if (localVer > remoteVer.Value)
                message = "The local version is newer than the remote version.";
            else if (localVer == null || remoteVer.Value == null)
                message = "There was an error checking the version.";

            onCheckVersion?.Invoke(new TestResult
            {
                IsUpdated = alreadyUpdated,
                LocalVersion = localVer,
                RemoteVersion = remoteVer.Value,
                BuildGuid = Guid.Parse(Application.buildGUID),
                Message = message
            });
        }
    }
}