using UnityEngine;
using UnityEditor;
using System;
using System.Linq;
using System.Collections.Generic;
using AGS.EditorView.ShipEditor.Data;
using AGS.Geom;
using AGS.EditorView.ShipEditor;

namespace AGS.Edit.ShipInteriorEditor.Handlers
{
    public sealed class RoomsEditorHandler: ShipCreatableElements
    {
        private static readonly GUIContent[] EditorButtonsContent = {
            new GUIContent((Texture) EditorGUIUtility.Load("draw.png"), "Draw elements"),
            new GUIContent((Texture) EditorGUIUtility.Load("clear.png"), "Delete elements")
        };

        protected Action<Vector2> editAction;
        private readonly Action<Vector2>[] editorHandlers;

        int _currentRoom;
        bool _roomselected = false;

        public RoomsEditorHandler(ShipLevelEditor parent) : base(parent)
        {
            editorHandlers = new Action<Vector2>[] { OnEditElements, OnRemoveElements };
        }

        public override void OnInspectorGUI()
        {
            EditTool();
            SubHandlerView();
        }

        /// <summary>
        /// 
        /// </summary>
        private void EditTool()
        {
            EditorGUILayout.BeginVertical();

            GUILayout.Label("Field editing tools", EditorStyles.boldLabel);

            var logic = GUILayout.Toolbar(toolIndex, EditorButtonsContent,
                GUILayout.MaxWidth(48 * EditorButtonsContent.Length), GUILayout.MaxHeight(48));

            if (logic != toolIndex)
            {
                toolIndex = logic;

                if (toolIndex >= 0)
                {
                    editAction = editorHandlers[toolIndex];
                    OResetHandlerItemSelected();
                }
            }

            EditorGUILayout.EndVertical();
        }

       void SubHandlerView()
        {
            GUILayout.Space(10);

            GUILayout.Label("Rooms", EditorStyles.boldLabel);

            if (target.Data.Rooms.RoomsList.Count > 0)
            {
                GUILayout.Label("Selected Rooms - " + (_currentRoom + 1).ToString());
            }
            GUILayout.Label("VISIBLE", EditorStyles.boldLabel);
            target.Data.Rooms.Visible = EditorGUILayout.Toggle(target.Data.Rooms.Visible);

            GUILayout.BeginHorizontal();

            if (GUILayout.Button("+"))
            {
                Color background = new Color(
                      UnityEngine.Random.Range(0f, 1f),
                      UnityEngine.Random.Range(0f, 1f),
                      UnityEngine.Random.Range(0f, 1f));

                target.Data.Rooms.RoomsList.Add(new RoomsData.Room()
                {
                    Index = target.Data.Rooms.RoomsList.Count + 1,
                    Name = "Room " + (target.Data.Rooms.RoomsList.Count + 1).ToString(),
                    Color = background
                });

                _currentRoom = 0;
                _roomselected = false;
            }

            GUILayout.EndHorizontal();

            GUILayout.BeginVertical();

            for (int i = 0; i < target.Data.Rooms.RoomsList.Count; i++)
            {
                var rm = target.Data.Rooms.RoomsList[i];
                GUILayout.BeginHorizontal();
                GUILayout.Label("Room " + (i + 1), EditorStyles.boldLabel);
                GUILayout.Label("Doors- " + rm.DoorsPoints.Count);
                GUILayout.Label("Enemies-" + rm.EnemyCount);
                var style = new GUIStyle(GUI.skin.button);

                Texture2D colored = ColorTexture(i);

                style.normal.background = colored;

                GUILayout.Button("color", style);

                if (target.Data.Rooms.RoomsList[i].Points.Count == 0)
                {
                    if (GUILayout.Button("Select", GUILayout.Width(160f)))
                    {
                        _currentRoom = i;
                        _roomselected = true;
                        target.Data.Rooms.RoomsList[i].Points = new List<Point>();
                    }
                }
                else
                {
                    if (target.Data.Rooms.RoomsList[i].Points.Count > 0)
                    {
                        GUILayout.Label("GENERATED!", GUILayout.Width(160f));
                    }

                    if (GUILayout.Button("X", GUILayout.Width(30f)))
                    {
                        target.Data.Rooms.RoomsList.RemoveAt(i);
                        _currentRoom = 0;
                        _roomselected = false;
                    }
                }

                GUILayout.EndHorizontal();
            }

            GUILayout.EndVertical();
        }

        private Texture2D ColorTexture(int i)
        {
            Texture2D colored = new Texture2D(50, 50);

            for (int y = 0; y < colored.height; y++)
            {
                for (int x = 0; x < colored.width; x++)
                {
                    colored.SetPixel(x, y, target.Data.Rooms.RoomsList[i].Color);
                }
            }
            colored.Apply();
            return colored;
        }
       

        public override void OResetHandlerItemSelected()
        {
            if (handler == null)
                return;

            handler.OResetHandlerItemSelected();
        }

        public override void OnEditElements(Vector2 location)
        {
            base.OnEditElements(location);

            if (/*!EditorInfo.IsApruve||*/  _roomselected == false)
                return;

            Debug.Log("Add " + location);

            var spoint = target.ConvertWorldPointToCell(location);

            if (!target.Data.Rooms.RoomsList[_currentRoom].Points.Contains(spoint))
            {
                //zone
                var cell = target.Data.Cells.FirstOrDefault(pr => pr.Point == spoint);
                target.Data.Rooms.RoomsList[_currentRoom].Points.Add(spoint);
                var pr = cell.Elements.FirstOrDefault(pr => pr.Layer == ShipElementLayer.Characters);

                // enemy
                bool enemy = false;

                //door
                var door = false;
                foreach (var dr in cell.Elements)
                {
                    if (dr.Doors) door = true;
                    if (dr.EnemySpawn) enemy = true;
                }

                if (door)
                    target.Data.Rooms.RoomsList[_currentRoom].DoorsPoints.Add(cell.Point);
                else {
                    if (target.Data.Rooms.RoomsList[_currentRoom].DoorsPoints.Contains(cell.Point))
                        target.Data.Rooms.RoomsList[_currentRoom].DoorsPoints.Remove(cell.Point);
                     }

                if (enemy)
                    target.Data.Rooms.RoomsList[_currentRoom].EnemyCount++;
   
            

                parent.SetUnsaved();
            }
        }

        public override void OnRemoveElements(Vector2 location)
        {
            if (handler == null)
                return;

            handler.OnRemoveElements(location);
        }

        public override void OnMouseUp(Vector2 location, Event e)
        {
            if (editAction == null)
                return;

            editAction(location);
        }
    }
}