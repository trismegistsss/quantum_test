using AGS.EditorView.ShipEditor;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace AGS.Edit.ShipInteriorEditor.Handlers
{
    public class ShipCharactersHandler : ShipCreatableElements
    {
        public ShipCharactersHandler(ShipLevelEditor parent) : base(parent) { }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            EditorGUILayout.BeginVertical();

            var loader = ShipElementsLoader.Instance;

            if (toolIndex >= 0)
                editAction = editorHandlers[toolIndex];

            var player = loader.Elements.Where(e => e.Player || e.Enemy).ToArray();
            if (null != player && player.Length > 0)
            {
                GUILayout.Label("Player");
                GUILayout.Space(2);
                CreateItem(player[0].Layer, player);
                GUILayout.Space(10);
            }

            EditorGUILayout.EndVertical();
        }
    }
}
