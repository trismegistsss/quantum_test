using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using AGS.EditorView.ShipEditor.Objects;
using AGS.Edit.Utils;

namespace AGS.Edit.ShipInteriorEditor.Window
{
    internal class ShipSelectElementsWindow : ShipAbstractWindow
    {
        private ShipElementView selected;

        private readonly List<ShipElementView> elements = new List<ShipElementView>();

        public new delegate void Event(ShipSelectElementsWindow w);

        public event Event onSelectElement = delegate { };

        public static ShipSelectElementsWindow Window(IEnumerable<ShipElementView> elements)
        {
            var w = GetWindowWithRect<ShipSelectElementsWindow>(new Rect(0, 0, 315, 400), true, "Select Element");

            w.elements.Clear();
            w.elements.AddRange(elements);

            w.elements.Sort((a, b) => a.Type.CompareTo(b.Type));
            w.elements.Add(null);

            return w;
        }

        private void OnGUI()
        {
            EditorGUILayout.BeginVertical();

            for (var j = 0; j < elements.Count; j += 6)
            {
                EditorGUILayout.BeginHorizontal();

                for (var k = j; k < elements.Count && k < j + 6; ++k)
                {
                    var view = elements[k];

                    if (!EditorUtils.ElementButton(elements[k]))
                        continue;

                    selected = view;

                    OnSelect();
                }

                EditorGUILayout.EndHorizontal();
            }

            EditorGUILayout.EndVertical();
        }

        private void OnSelect()
        {
            Close();

            onSelectElement(this);
        }

        private void OnLostFocus()
        {
            Focus();
        }

        public ShipElementView Selected
        {
            get { return selected; }
        }
    }
}