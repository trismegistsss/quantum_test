using System.Xml.Serialization;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEditor;
using CustomUtils;
using AGS.EditorView.ShipEditor.Config;
using AGS.EditorView.ShipEditor;
using AGS.Edit.Utils;
using AGS.EditorView.ShipEditor.Data;

namespace AGS.Edit.ShipInteriorEditor
{
    internal static class ShipEditorMain
    {
        public static readonly string DefaultCameraName = "MainCamera";
        public static readonly string UnknownType = "None";

        public static readonly Vector2 WinSize = new Vector2(1024, 768);

        [MenuItem("Editor/Setup Camera")]
        public static void SetupCamera()
        {
            var camera = Camera.main;

            if (camera == null)
            {
                var obj = new GameObject();

                obj.AddComponent<AudioListener>();

                camera = obj.AddComponent<Camera>();
                camera.tag = DefaultCameraName;
            }

            camera.name = DefaultCameraName;
            camera.orthographic = true;
            camera.orthographicSize = WinSize.y / 2;
            camera.transform.position = new Vector3(0, 0, -100);
        }

        [MenuItem("Editor/Create Level %q")]
        public static void CreateNewLevel()
        {
            RemoveLevelEditors();

            var obj = new GameObject();
            var data = XMLHelper.DeserealizeFromXML<ShipLevelData>(ShipConfig.ShipDefaultLevelPath, ShipElementDataTypes.Serializable);

            var view = obj.AddComponent<ShipLevelEditorView>();

            view.Load(data);

            var path = EditorUtility.SaveFilePanel("Select XML file", ShipConfig.ShipDefaultLevelsFolder, "", "xml");

            if (string.IsNullOrEmpty(path))
                return;

            var formatter = new XmlSerializer(typeof(ShipLevelData), ShipElementDataTypes.Serializable);

            using (var f = new StreamWriter(path, false, Encoding.UTF8))
            {
                formatter.Serialize(f, view.Data);

                view.name = Path.GetFileNameWithoutExtension(path);
                view.Path = EditorUtils.FSPathToAssetPath(path);

                Debug.LogFormat("Create level at {0}", view.Path);
            }

            Selection.activeGameObject = view.gameObject;

            AssetDatabase.Refresh();
        }

        [MenuItem("Editor/Load Level %e")]
        public static void LoadLevel()
        {
            string[] filters = { "XML Files", "xml" };
            var path = EditorUtility.OpenFilePanelWithFilters("Open XML File", ShipConfig.ShipDefaultLevelsFolder, filters);

            if (string.IsNullOrEmpty(path))
                return;

            RemoveLevelEditors();

            var obj = new GameObject();
            var view = obj.AddComponent<ShipLevelEditorView>();

            var formatter = new XmlSerializer(typeof(ShipLevelData), ShipElementDataTypes.Serializable);

            using (var fs = new FileStream(path, FileMode.OpenOrCreate))
            {
                var data = formatter.Deserialize(fs) as ShipLevelData;

                view.Load(data);

                view.name = Path.GetFileNameWithoutExtension(path);
                view.Path = EditorUtils.FSPathToAssetPath(path);

                Debug.LogFormat("Load level at {0}", view.Path);
            }

            Selection.activeGameObject = view.gameObject;
        }

        private static void RemoveLevelEditors()
        {
            var levels = Object.FindObjectsOfType<ShipBaseEditorView>();

            foreach (var l in levels) Object.DestroyImmediate(l.gameObject);
        }
    }
}
