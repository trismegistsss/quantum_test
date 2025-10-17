using System;
using UnityEditor;
using UnityEngine;

namespace AGS.Edit.ShipInteriorEditor.Handlers
{
    public abstract class ShipEditorToolBase : ShipBaseHandler
    {
        private bool _isHero;
        protected Sprite ItemView;
        private static readonly GUIContent[] EditorButtonsContent = {
            new GUIContent((Texture) EditorGUIUtility.Load("draw.png"), "Draw elements"),
            new GUIContent((Texture) EditorGUIUtility.Load("clear.png"), "Delete elements")
        };

        protected Action<Vector2> editAction;
        protected readonly Action<Vector2>[] editorHandlers;

        public ShipEditorToolBase(ShipLevelEditor parent) : base(parent)
        {
            editorHandlers = new Action<Vector2>[] { OnEditElements, OnRemoveElements};
        }

        public override void OnInspectorGUI()
        {
            EditorGUILayout.BeginVertical();

            GUILayout.Label("Field editing tools", EditorStyles.boldLabel);

            var logic = GUILayout.Toolbar(toolIndex, EditorButtonsContent,
                GUILayout.MaxWidth(48 * EditorButtonsContent.Length), GUILayout.MaxHeight(48));

            if (logic != toolIndex)
            {
                toolIndex = logic;

                ItemView = null;

                if (toolIndex >= 0)
                {
                    editAction = editorHandlers[toolIndex];
                    OResetHandlerItemSelected();
                }
            }

            EditorGUILayout.EndVertical();
        }

        public override void OResetHandlerItemSelected()
        {
            if (handler == null)
                return;

            handler.OResetHandlerItemSelected();
        }

        public override void OnEditElements(Vector2 location)
        {
            if (handler == null)
                return;

            handler.OnEditElements(location);
        }

        public override void OnRemoveElements(Vector2 location)
        {
            if (handler == null)
                return;

            handler.OnRemoveElements(location);
        }

        // Hero
        private void OnHeroInsert(Vector2 location)
        {
            var cell = GetCell(location);

            if (cell == null)
                return;

            for (var i = 0; i< cell.Elements.Count; i++)
            {
                if (_isHero && !cell.Elements[i].PlayerSpawn)
                    return;
                else
                {
                    cell.Elements[i].PlayerSpawn = !cell.Elements[i].PlayerSpawn;
                    _isHero = cell.Elements[i].PlayerSpawn;
                }
            }
            
        }

        private void OnEnemyInsert(Vector2 location)
        {
            var cell = GetCell(location);

            if (cell == null)
                return;

            //.EnemySpawn = !cell.EnemySpawn;
        }

        private void OnBossInsert(Vector2 location)
        {
            var cell = GetCell(location);

            if (cell == null)
                return;

          //  cell.BossSpawn = !cell.BossSpawn;
        }

    }
}
