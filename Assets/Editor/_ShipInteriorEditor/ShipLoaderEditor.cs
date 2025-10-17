using AGS.Edit.Utils;
using AGS.EditorView.ShipEditor;
using AGS.EditorView.ShipEditor.Config;
using System;
using UnityEditor;
using UnityEngine;

namespace AGS.Edit.ShipInteriorEditor
{
    [CustomEditor(typeof(ShipElementsLoader))]
    public class ShipLoaderEditor : BaseEditor
    {
        private ShipElementsLoader Target
        {
            get { return target as ShipElementsLoader; }
        }

        private void OnEnable()
        {
            ShipConfig.Load();

            Target.Refresh();
        }

        public override void OnInspectorGUI()
        {
            EditorGUILayout.BeginVertical();

            GUILayout.Label("Loaded Elements:", EditorStyles.boldLabel);

            var loader = ShipElementsLoader.Instance;
            var elements = loader.Elements;

            Array.Sort(elements, (a, b) => a.Type.CompareTo(b.Type));

            for (var j = 0; j < elements.Length; j += 6)
            {
                EditorGUILayout.BeginHorizontal();

                for (var k = j; k < elements.Length && k < j + 6; ++k)
                    if (EditorUtils.ElementButton(elements[k]))
                        Selection.activeObject = elements[k].gameObject;

                EditorGUILayout.EndHorizontal();
            }

            if (GUILayout.Button("Refresh", GUILayout.MaxWidth(128)))
                Target.Refresh();

            EditorGUILayout.EndVertical();
        }

    }
}
