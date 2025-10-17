using System;
using UnityEngine;
using UnityEditor;
using AGS.EditorView.ShipEditor.Objects;

namespace AGS.Edit.Utils
{
    internal static class EditorUtils
    {
        public static readonly string DefaultCameraName = "MainCamera";
        public static readonly string UnknownType = "None";

        public static string Popup(string label, string[] content, string val, int defaultIndex)
        {
            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.LabelField(label);

            var index = val != null ? Array.IndexOf(content, val) : -1;

            index = index == -1 ? defaultIndex : index;

            index = EditorGUILayout.Popup(index, content);

            EditorGUILayout.EndHorizontal();

            return content[index];
        }

        public static int Popup(string label, string[] content, int index)
        {
            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.LabelField(label);

            index = EditorGUILayout.Popup(index, content);

            EditorGUILayout.EndHorizontal();

            return index;
        }

        public static string GetElementTypeStr(int type, string[] types)
        {
            var find = Array.Find(types, e => e.GetHashCode() == type);

            return string.IsNullOrEmpty(find) ? UnknownType : find;
        }

        public static int GetElementTypeInt(string type)
        {
            return type != UnknownType ? type.GetHashCode() : -1;
        }

        public static Texture2D GetPreview(GameObject view)
        {
            if (view == null)
                return null;

            var renderer = view.GetComponent<SpriteRenderer>();

            return AssetPreview.GetAssetPreview(renderer.sprite);
        }

        static readonly GUILayoutOption[] PreviewSize = Rect(48, 48);

        public static bool SpriteButton(GameObject view)
        {
            return view == null ? GUILayout.Button(UnknownType, PreviewSize) : GUILayout.Button(GetPreview(view), PreviewSize);
        }


        public static bool ElementButton(ShipElementView view)
        {
            return SpriteButton(view != null ? view.gameObject : null);
        }

        public static GUILayoutOption[] Rect(int width, int height)
        {
            return new[] { GUILayout.MaxWidth(width), GUILayout.MaxHeight(height), GUILayout.MinWidth(width), GUILayout.MinHeight(height) };
        }

        internal static Texture ElementContent(ShipElementView shipElementView)
        {
            throw new NotImplementedException();
        }

        public static GUIContent SpriteContent(Sprite sprite)
        {
            var preview = AssetPreview.GetAssetPreview(sprite);
            var tooltip = sprite.name;
            return new GUIContent(preview, tooltip);
        }

        public static string FSPathToAssetPath(string path)
        {
            var n = path.IndexOf("Assets", StringComparison.Ordinal);

            return path.Substring(n);
        }
    }
}