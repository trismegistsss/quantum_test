using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using AGS.EditorView.ShipEditor.Objects;
using AGS.Edit;
using AGS.EditorView.ShipEditor;
using AGS.Edit.Utils;

namespace AGS.Edit.ShipInteriorEditor
{
    [CustomEditor(typeof(ShipElementView))]
    public class ShipElementEditor : BaseEditor
    {
        public static readonly int None = -1;

        private ShipElementView Target
        {
            get { return target as ShipElementView; }
        }

        protected override void OnDisable()
        {
            SaveChanges(Target);
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            var loader = ShipElementsLoader.Instance;
            var types = new HashSet<string>(loader.TypeNames) { Target.name, ShipEditorMain.UnknownType };

            EditorGUILayout.BeginVertical();

            GUILayout.Label("Element Settings", GuiStyleRed);

            var typeStr = EditorUtils.Popup("ElementType", types.ToArray(), EditorUtils.GetElementTypeStr(Target.Type, types.ToArray()), types.Count - 1);
            var data = Target.Data;

            GUILayout.Label("Common properties", GuiStyleYellow);
            data.Type = EditorUtils.GetElementTypeInt(typeStr);
            GUILayout.Label("Item Type : " + data.Type, GuiStyleYellow);
            data.Layer = (ShipElementLayer)EditorGUILayout.EnumPopup("Layer", Target.Layer);
            GUILayout.Space(10);
            GUILayout.Label("Item Settings :", GuiStyleBlue);
            GUILayout.Space(2);
            data.Maped = EditorGUILayout.Toggle("Maped", data.Maped);
            data.Grounds = EditorGUILayout.Toggle("Grounds", data.Grounds);
            data.Barrier = EditorGUILayout.Toggle("Barrier", data.Barrier);
            data.Door = EditorGUILayout.Toggle("Door", data.Door);
            data.Movable = EditorGUILayout.Toggle("Movable", data.Movable);
            data.Dangerous = EditorGUILayout.Toggle("Dangerous", data.Dangerous);
            data.Invisible = EditorGUILayout.Toggle("Invisible", data.Invisible);
            data.Item = EditorGUILayout.Toggle("Item", data.Item);
            data.Decor = EditorGUILayout.Toggle("Decor", data.Decor);
            data.Exit = EditorGUILayout.Toggle("Exit", data.Exit);
            

            GUILayout.Space(10);
            GUILayout.Label("Character Settings :", GuiStyleBlue);
            GUILayout.Space(2);
            data.Player = EditorGUILayout.Toggle("Player", data.Player);
            GUILayout.Space(2);
            data.Enemy = EditorGUILayout.Toggle("Enemy", data.Enemy);

            if (data.Dangerous!=null)
            {

            }

            if (data.Door)
            {
                GUILayout.Space(10);
                GUILayout.Label("Door Settings :", GuiStyleBlue);
                GUILayout.Space(2);

                Target.DoorObject = (Transform)EditorGUILayout.ObjectField("Door", Target.DoorObject, typeof(Transform), true);
                Target.DoorCollider = (BoxCollider)EditorGUILayout.ObjectField("DoorCollider", Target.DoorCollider, typeof(BoxCollider), true);
            }

            EditorGUILayout.EndVertical();
        }
    }
}