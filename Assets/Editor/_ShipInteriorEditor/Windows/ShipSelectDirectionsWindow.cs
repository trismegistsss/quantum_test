using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;
using AGS.Edit.Utils;

namespace AGS.Edit.ShipInteriorEditor.Window
{
    internal class ShipSelectDirectionsWindow : ShipAbstractWindow
    {
        private const int size = 48;
        private const int intent = 5;

        private static readonly Vector2[,] Directions = { { Vector2.left + Vector2.up, Vector2.up, Vector2.right + Vector2.up },
                                                { Vector2.left, Vector2.zero, Vector2.right },
                                                { Vector2.left + Vector2.down, Vector2.down, Vector2.right + Vector2.down } };

        private static GUIContent[,] content;

        private readonly HashSet<Vector2> selected = new HashSet<Vector2>();

        public static ShipSelectDirectionsWindow Window(IEnumerable<Vector2> directions)
        {
            var window = GetWindowWithRect<ShipSelectDirectionsWindow>(new Rect(0, 0, (size + intent) * 3, (size + intent) * 3), true, "Select Cells");

            window.selected.AddRange(directions);

            return window;
        }

        private void OnEnable()
        {
            content = new[,]
            {
            {
                new GUIContent((Texture)EditorGUIUtility.Load("arrow-top-left.png"), "{-1, 1}"),
                new GUIContent((Texture)EditorGUIUtility.Load("arrow-top.png"), "{0, 1}"),
                new GUIContent((Texture)EditorGUIUtility.Load("arrow-top-right.png"), "{1, 1}")
            },
            {
                new GUIContent((Texture)EditorGUIUtility.Load("arrow-left.png"), "{-1, 0}"),
                new GUIContent((Texture)EditorGUIUtility.Load("center.png"), "{0, 0}"),
                new GUIContent((Texture)EditorGUIUtility.Load("arrow-right.png"), "{1, 0}")
            },
            {
                new GUIContent((Texture)EditorGUIUtility.Load("arrow-bottom-left.png"), "{-1, -1}"),
                new GUIContent((Texture)EditorGUIUtility.Load("arrow-bottom.png"), "{0, -1}"),
                new GUIContent((Texture)EditorGUIUtility.Load("arrow-bottom-right.png"), "{1, -1}")
            }
        };
        }

        private void OnGUI()
        {
            EditorGUILayout.BeginVertical();

            for (var i = 0; i < 3; ++i)
            {
                EditorGUILayout.BeginHorizontal();

                for (var j = 0; j < 3; ++j)
                {
                    var dir = Directions[i, j];
                    var toggle = selected.Contains(dir);

                    toggle = GUILayout.Toggle(toggle, content[i, j], "Button", EditorUtils.Rect(size, size));

                    if (!toggle)
                        selected.Remove(dir);
                    else
                        selected.Add(dir);
                }

                EditorGUILayout.EndHorizontal();
            }

            EditorGUILayout.EndVertical();
        }

        private void OnLostFocus()
        {
            Focus();
        }

        public Vector2[] Selected
        {
            get { return selected.ToArray(); }
        }
    }
}
