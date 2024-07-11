using System.Collections.Generic;
using Juancazzhr.Tools.USemVer.Core;
using Juancazzhr.Tools.USemVer.Core.Domain;
using Juancazzhr.Tools.USemVer.Core.Infrastructure;
using Juancazzhr.Tools.USemVer.Editor.Core.Infrastructure;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Juancazzhr.Tools.USemVer.Editor.Dashboard
{
    public class DashboardEditorWindow : EditorWindow
    {
        [SerializeField] private VisualTreeAsset visualTreeAsset;

        private SemVerSystem semVerSystem;
        private SemVerMediator semVerMediator;

        private Button _BtnIncrement;
        private Button _BtnReset;
        private Label _LabelMajor;
        private Label _LabelMinor;
        private Label _LabelPatch;
        private Button _BtnOnlineRef;


        private void OnEnable()
        {
            var appVer = AppVer.GetVersion();

            const string localPath = "Assets/Resources/";
            const string buildPath = "Builds/";
            semVerMediator = new SemVerMediator(new FileVersionRepository(localPath),
                                     new FileVersionRepository(buildPath),
                                     new ProjectVersionRepository(appVer));

            semVerSystem = SemVerSystem.Instance;
        }

        private void OnDisable()
        {
            semVerSystem = default;
        }

        [MenuItem("Tools/Juancazz/USemVer (Dashboard)")]
        public static void ShowWindow()
        {
            var size = new Vector2(240, 280);

            var wnd = GetWindow<DashboardEditorWindow>();
            wnd.titleContent = new GUIContent("USemVer (Dashboard)");
            wnd.minSize = size;
            wnd.maxSize = size;
        }

        public void CreateGUI()
        {
            var root = rootVisualElement;
            var contentFromUxml = visualTreeAsset.Instantiate();
            contentFromUxml.contentContainer.style.width = Length.Percent(100);
            contentFromUxml.contentContainer.style.height = Length.Percent(100);
            root.Add(contentFromUxml);

            // Add handles to command buttons
            _BtnIncrement = contentFromUxml.Q<Button>("btn__increment");
            _BtnReset = contentFromUxml.Q<Button>("btn__reset");
            _BtnOnlineRef = contentFromUxml.Q<Button>("btn__online-ref");

            // Labels Ver parts
            _LabelMajor = contentFromUxml.Q<Label>("version-part__major");
            _LabelMinor = contentFromUxml.Q<Label>("version-part__minor");
            _LabelPatch = contentFromUxml.Q<Label>("version-part__patch");

            // Set initial values
            var currentVersion = semVerSystem.Version;
            _LabelMajor.text = currentVersion.Major.ToString();
            _LabelMinor.text = currentVersion.Minor.ToString();
            _LabelPatch.text = currentVersion.Build.ToString();

            // Add Dropdown handles
            var dropdownVerType = contentFromUxml.Q<DropdownField>("dd__version-part");
            dropdownVerType.choices = new List<string> { "MAJOR", "MINOR", "PATCH" };
            dropdownVerType.RegisterValueChangedCallback(ev =>
            {
                switch (ev.newValue)
                {
                    case "MAJOR":
                        SetSelectedStyle(_LabelMajor);
                        SetUnselectedStyle(_LabelMinor);
                        SetUnselectedStyle(_LabelPatch);
                        break;

                    case "MINOR":
                        SetUnselectedStyle(_LabelMajor);
                        SetSelectedStyle(_LabelMinor);
                        SetUnselectedStyle(_LabelPatch);
                        break;

                    case "PATCH":
                        SetUnselectedStyle(_LabelMajor);
                        SetUnselectedStyle(_LabelMinor);
                        SetSelectedStyle(_LabelPatch);
                        break;
                }

                return;

                void SetSelectedStyle(Label element) => element.AddToClassList("version-part-selected");
                void SetUnselectedStyle(Label element) => element.RemoveFromClassList("version-part-selected");
            });
            dropdownVerType.value = "MAJOR";

            // Interact with labels
            _LabelMajor.RegisterCallback<ClickEvent>(ev => dropdownVerType.value = "MAJOR");
            _LabelMinor.RegisterCallback<ClickEvent>(ev => dropdownVerType.value = "MINOR");
            _LabelPatch.RegisterCallback<ClickEvent>(ev => dropdownVerType.value = "PATCH");

            _BtnIncrement.clicked += Increment;
            _BtnReset.clicked += Reset;
            _BtnOnlineRef.clicked += () => { Application.OpenURL("https://semver.org/"); };
            return;

            void Increment()
            {
                var verType = dropdownVerType.value switch
                {
                    "MAJOR" => VersionPart.Major,
                    "MINOR" => VersionPart.Minor,
                    "PATCH" => VersionPart.Patch,
                    _       => VersionPart.Patch
                };
                var newVersion = semVerSystem.Increment(verType);
                semVerMediator.SaveVersion(newVersion);
                _LabelMajor.text = newVersion.Major.ToString();
                _LabelMinor.text = newVersion.Minor.ToString();
                _LabelPatch.text = newVersion.Build.ToString();
            }

            void Reset()
            {
                var alertTitle = "(USemVer) Reset Version";
                var alertMessage =
                    $"Are you sure you want to reset the version from {semVerSystem.Version} to {SemVerSystem.DefaultVersion}?";
                var alertResponse = EditorUtility.DisplayDialog(alertTitle, alertMessage, "Yes", "No");
                if (alertResponse == false) return;

                var newVersion = semVerSystem.Reset();
                semVerMediator.SaveVersion(newVersion);
                _LabelMajor.text = newVersion.Major.ToString();
                _LabelMinor.text = newVersion.Minor.ToString();
                _LabelPatch.text = newVersion.Build.ToString();
                dropdownVerType.value = "MAJOR";
            }
        }
    }
}