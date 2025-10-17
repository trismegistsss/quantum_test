using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace AGS.Edit
{
    public class BaseEditor : Editor
    {
        protected GUIStyle GuiStyleRed;
        protected GUIStyle GuiStyleYellow;
        protected GUIStyle GuiStyleBlue;

        public override void OnInspectorGUI()
        {
            GuiStyleRed = new GUIStyle();
            GuiStyleRed.fontSize = 18;
            GuiStyleRed.normal.textColor = Color.red;

            GuiStyleYellow = new GUIStyle();
            GuiStyleYellow.fontSize = 16;
            GuiStyleYellow.normal.textColor = Color.yellow;

            GuiStyleBlue = new GUIStyle();
            GuiStyleBlue.fontSize = 14;
            GuiStyleBlue.normal.textColor = Color.blue;
        }

        protected virtual void OnDisable()
        {
            SaveChanges(target);
        }


        protected virtual void OnSceneGUI()
        {

        }

        protected virtual void Line()
        {
            Color color = GUI.color;
            GUI.color = Color.blue;
            GUILayout.Box("", new GUILayoutOption[] { GUILayout.ExpandWidth(true), GUILayout.Height(2) });
            GUI.color = color;
        }

        protected virtual void SaveChanges(Object targ)
        {
            if (targ != null)
            {
                AssetDatabase.Refresh();
                EditorUtility.SetDirty(targ);
                AssetDatabase.SaveAssets();
            }
        }
    }
}