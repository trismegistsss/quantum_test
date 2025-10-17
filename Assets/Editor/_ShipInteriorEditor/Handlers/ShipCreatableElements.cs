using AGS.Edit.Utils;
using AGS.EditorView.ShipEditor;
using AGS.EditorView.ShipEditor.Data;
using AGS.EditorView.ShipEditor.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace AGS.Edit.ShipInteriorEditor.Handlers
{
    public class ShipCreatableElements : ShipEditorToolBase
    {
        private ShipElementView _itemObject;

        public ShipCreatableElements(ShipLevelEditor parent) : base(parent) { }

        protected void CreateItem(ShipElementLayer type, ShipElementView[] prefabs)
        {
            // destroy shadows
            var mapItems = new List<ShipElementView>(prefabs);

            EditorGUILayout.BeginVertical();

            for (var j = 0; j < mapItems.Count; j += 6)
            {
                EditorGUILayout.BeginHorizontal();

                for (var k = j; k < mapItems.Count && k < j + 6; ++k)
                {
                    var view = mapItems[k].GetView();
                    if (null == view) continue;

                    var toggle = GUILayout.Toggle(ItemView == view, EditorUtils.SpriteContent(view), "Button", EditorUtils.Rect(64, 64));

                    if (!toggle) continue;

                    _itemObject = mapItems[k];
                    ItemView = view;

                    toolIndex = 0;
                }

                EditorGUILayout.EndHorizontal();
            }

            EditorGUILayout.EndVertical();
        }

        public override void OnMouseUp(Vector2 location, Event e)
        {
            if (editAction == null)
                return;

            editAction(location);
        }

        public override void OnEditElements(Vector2 location)
        {
            if (_itemObject == null)
                return;

            var cell = GetCell(location);

            if (cell == null)
                return;

            cell.Elements.RemoveAll(e =>
            {

                if (e.Layer == _itemObject.Layer)
                {
                    parent.RemoveObjectInMap(cell, e);
                    return true;
                }

                return false;
            });

            var ed = new ShipElementInfo
            {
                Element = _itemObject.Type,
                Layer = _itemObject.Layer,
                Id = Guid.NewGuid().ToString(),
                Doors = _itemObject.Data.Door,
                EnemySpawn = _itemObject.Data.Enemy,
                PlayerSpawn = _itemObject.Data.Player,
                //BossSpawn = _itemObject.Data.Enemy
            };

            cell.Elements.Add(ed);
            parent.SetUnsaved();
        }


        public override void OnRemoveElements(Vector2 location)
        {
            var cell = GetCell(location);

            if (cell == null)
                return;

            var select = cell.Elements.OrderByDescending(e => e.Layer);

            foreach (var el in select)
            {
                if (!parent.VisibleLayer[el.Layer])
                    continue;

                cell.Elements.Remove(el);


                parent.RemoveObjectInMap(cell, el);

                break;
            }

            parent.SetUnsaved();
        }
    }
}
