using AGS.EditorView.ShipEditor;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace AGS.Edit.ShipInteriorEditor.Handlers
{
    public sealed class ShipMapHandler : ShipCreatableElements
    { 
        public ShipMapHandler(ShipLevelEditor parent) : base(parent){ }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            EditorGUILayout.BeginVertical();

            var loader = ShipElementsLoader.Instance;

            if(toolIndex>=0)
                editAction = editorHandlers[toolIndex];

            var dangerousArray = loader.Elements.Where(e => e.Dangerous).ToArray();           
            if (null != dangerousArray && dangerousArray.Length>0)
            {
                GUILayout.Label("Dangerius tile");
                GUILayout.Space(2);
                CreateItem(dangerousArray[0].Layer, dangerousArray);
                GUILayout.Space(10);
            }

            var groundArray = loader.Elements.Where(e => e.Grounds).ToArray();
            if (null != groundArray && groundArray.Length > 0)
            {
                GUILayout.Label("Grounds tile");
                GUILayout.Space(2);
                CreateItem(groundArray[0].Layer, groundArray);
                GUILayout.Space(10);
            }

            var iexitArray = loader.Elements.Where(e => e.Exit).ToArray();
            if (null != iexitArray && iexitArray.Length > 0)
            {
                GUILayout.Label("Exit tile");
                GUILayout.Space(2);
                CreateItem(iexitArray[0].Layer, iexitArray);
                GUILayout.Space(10);
            }

            var invisibleArray = loader.Elements.Where(e => e.Invisible).ToArray();
            if (null != invisibleArray && invisibleArray.Length > 0)
            {
                GUILayout.Label("Invisible tile");
                GUILayout.Space(2);
                CreateItem(invisibleArray[0].Layer, invisibleArray);
                GUILayout.Space(10);
            }

            var barrierArray = loader.Elements.Where(e => e.Barrier).ToArray();
            if (null != barrierArray && barrierArray.Length > 0)
            {
                GUILayout.Label("Block tile");
                GUILayout.Space(2);
                CreateItem(barrierArray[0].Layer, barrierArray);
                GUILayout.Space(10);
            }

            var decorArray = loader.Elements.Where(e => e.Decor).ToArray();
            if (null != decorArray && decorArray.Length > 0)
            {
                GUILayout.Label("Decor tile");
                GUILayout.Space(2);
                CreateItem(decorArray[0].Layer, decorArray);
                GUILayout.Space(10);
            }

            var itemArray = loader.Elements.Where(e => e.Item).ToArray();
            if (null != itemArray && itemArray.Length > 0)
            {
                GUILayout.Label("Items tile");
                GUILayout.Space(2);
                CreateItem(itemArray[0].Layer, itemArray);
                GUILayout.Space(10);
            }

            EditorGUILayout.EndVertical();
        }
    }
}