using System;
using UnityEngine;
using UnityEngine.Events;

namespace Juancazzhr.Tools.USemVer.Component
{
    [RequireComponent(typeof(RemoteVersionChecker), typeof(LocalVersionChecker))]
    [DefaultExecutionOrder(-1)]
    public class VersionChecker : MonoBehaviour
    {
        public enum Status
        {
            Updated,
            Outdated,
            Error
        }
        
        /// <summary>
        /// Struct to store the result of the version check.
        /// </summary>
        public struct TestResult
        {
            /// <summary>
            /// Status of the version check.
            /// </summary>
            public Status Status;

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

            public void Deconstruct(out Status alreadyUpdated,
                                    out Version localVersion,
                                    out Version remoteVersion,
                                    out Guid buildGuid,
                                    out string message)
            {
                alreadyUpdated = Status;
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
            var localVer = LocalVersionChecker.GetLocalVersion();

            var message = "";
            var alreadyUpdated = false;

            if (remoteVer.IsFailure)
            {
                message = remoteVer.Error;
                onCheckVersion?.Invoke(new TestResult
                {
                    Status = Status.Error,
                    LocalVersion = localVer,
                    RemoteVersion = remoteVer.Value,
                    BuildGuid = Guid.Parse(Application.buildGUID),
                    Message = message
                });
                return;
            }

            if (localVer == remoteVer.Value)
            {
                message = "Tienes la última versión.";
                alreadyUpdated = true;
            }
            else if (localVer < remoteVer.Value)
                message = "Hay una nueva versión disponible.";
            else if (localVer > remoteVer.Value)
                message = "La versión local es más reciente que la versión remota.";
            else if (localVer == null || remoteVer.Value == null)
                message = "No se pudo obtener la versión.";

            onCheckVersion?.Invoke(new TestResult
            {
                Status = alreadyUpdated ? Status.Updated : Status.Outdated,
                LocalVersion = localVer,
                RemoteVersion = remoteVer.Value,
                BuildGuid = Guid.Parse(Application.buildGUID),
                Message = message
            });
        }
    }
}