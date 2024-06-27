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

        private IncrementerSystem _IncrementerSystem;
        private Mediator _Mediator;

        private Button _BtnIncrement;
        private Button _BtnReset;
        private Label _LabelMajor;
        private Label _LabelMinor;
        private Label _LabelPatch;


        private void OnEnable()
        {
            var localPath = "Assets/Resources/";
            var buildPath = "Builds/";
            _Mediator = new Mediator(new FileVersionRepository(localPath),
                                     new FileVersionRepository(buildPath),
                                     new ProjectVersionRepository());
            
            _IncrementerSystem = new IncrementerSystem(ProductInfo.GetVersion());
        }

        private void OnDisable()
        {
            _IncrementerSystem = null;
        }

        [MenuItem("Tools/Juancazz/USemVer (Dashboard)")]
        public static void ShowWindow()
        {
            var size = new Vector2(400, 200);

            var wnd = GetWindow<DashboardEditorWindow>();
            wnd.titleContent = new GUIContent("USemVer (Dashboard)");
            wnd.minSize = size;
            wnd.maxSize = size;
        }

        public void CreateGUI()
        {
            var root = rootVisualElement;
            var contentFromUxml = visualTreeAsset.Instantiate();
            root.Add(contentFromUxml);

            var labelAuthor = new Label("by Juancazz v1");
            root.Add(labelAuthor);

            // Add handles to command buttons
            _BtnIncrement = contentFromUxml.Q<Button>("btn__increment");
            _BtnReset = contentFromUxml.Q<Button>("btn__reset");

            // Labels Ver parts
            _LabelMajor = contentFromUxml.Q<Label>("label__major");
            _LabelMinor = contentFromUxml.Q<Label>("label__minor");
            _LabelPatch = contentFromUxml.Q<Label>("label__patch");

            // Set initial values
            var currentVersion = _IncrementerSystem.Version;
            _LabelMajor.text = currentVersion.Major.ToString();
            _LabelMinor.text = currentVersion.Minor.ToString();
            _LabelPatch.text = currentVersion.Build.ToString();

            // Add Dropdown handles
            var dropdownVerType = contentFromUxml.Q<DropdownField>("dd__version-part");
            dropdownVerType.choices = new List<string> { "Major", "Minor", "Patch" };
            dropdownVerType.RegisterValueChangedCallback(ev =>
            {
                switch (ev.newValue)
                {
                    case "Major":
                        SetSelectedStyle(_LabelMajor);
                        SetUnselectedStyle(_LabelMinor);
                        SetUnselectedStyle(_LabelPatch);
                        break;

                    case "Minor":
                        SetUnselectedStyle(_LabelMajor);
                        SetSelectedStyle(_LabelMinor);
                        SetUnselectedStyle(_LabelPatch);
                        break;

                    case "Patch":
                        SetUnselectedStyle(_LabelMajor);
                        SetUnselectedStyle(_LabelMinor);
                        SetSelectedStyle(_LabelPatch);
                        break;
                }

                return;

                void SetSelectedStyle(Label element) => element.AddToClassList("version-part-selected");
                void SetUnselectedStyle(Label element) => element.RemoveFromClassList("version-part-selected");
            });
            dropdownVerType.value = "Major";

            // Interact with labels
            _LabelMajor.RegisterCallback<ClickEvent>(ev => dropdownVerType.value = "Major");
            _LabelMinor.RegisterCallback<ClickEvent>(ev => dropdownVerType.value = "Minor");
            _LabelPatch.RegisterCallback<ClickEvent>(ev => dropdownVerType.value = "Patch");

            _BtnIncrement.clicked += Increment;
            _BtnReset.clicked += Reset;
            return;

            void Increment()
            {
                var verType = dropdownVerType.value switch
                {
                    "Major" => VersionPart.Major,
                    "Minor" => VersionPart.Minor,
                    "Patch" => VersionPart.Patch,
                    _       => VersionPart.Patch
                };
                var newVersion = _IncrementerSystem.Increment(verType);
                _Mediator.SaveVersion(newVersion);
                _LabelMajor.text = newVersion.Major.ToString();
                _LabelMinor.text = newVersion.Minor.ToString();
                _LabelPatch.text = newVersion.Build.ToString();
            }

            void Reset()
            {
                var newVersion = _IncrementerSystem.Reset();
                _Mediator.SaveVersion(newVersion);
                _LabelMajor.text = newVersion.Major.ToString();
                _LabelMinor.text = newVersion.Minor.ToString();
                _LabelPatch.text = newVersion.Build.ToString();
                dropdownVerType.value = "Major";
            }
        }
    }
}