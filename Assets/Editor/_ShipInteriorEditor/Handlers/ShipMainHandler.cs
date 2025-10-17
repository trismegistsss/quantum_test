using AGS.Edit.Utils;
using AGS.EditorView.ShipEditor;
using AGS.EditorView.ShipEditor.Data;
using AGS.Geom;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace AGS.Edit.ShipInteriorEditor.Handlers
{
    public sealed class ShipMainHandler : ShipBaseHandler
    {
        public ShipMainHandler(ShipLevelEditor parent) : base(parent){}

		public override void OnInspectorGUI()
		{
			var data = target.Data;

			EditorGUILayout.BeginVertical();

			data.Rows = EditorGUILayout.IntField("LevelWidth", target.Width);
			data.Colls = EditorGUILayout.IntField("LevelHeight", target.Height);
			data.Cell = EditorGUILayout.FloatField("Cell Size", target.CellSize);
			
			GUILayout.Space(10);
	
			LevelСonditions();

			GUILayout.Space(10);

			EditPercentages();

			GUILayout.Space(10);

			GUILayout.Label("Crop level", EditorStyles.boldLabel);

			var crop = new Dictionary<string, Action> { { "Crop up", CropUp }, { "Crop down", CropDown }, { "Crop left", CropLeft }, { "Crop right", CropRight } };

			foreach (var it in crop)
				if (GUILayout.Button(it.Key, GUILayout.MaxWidth(200)))
					it.Value();

			if (GUI.changed)
				parent.SetUnsaved();

			EditorGUILayout.EndVertical();
		}

		private void LevelСonditions()
		{
			var data = target.Data;

			EditorGUILayout.BeginVertical();

			GUILayout.Label("Level Сonditions", EditorStyles.boldLabel);

			data.ConditionType = (ConditionType)EditorGUILayout.EnumPopup("Condition", data.ConditionType);

			EditorGUILayout.EndVertical();
		}


		private void EditPercentages()
		{
			var loader = ShipElementsLoader.Instance;
			var elements = loader.Elements.Where(e => e.Maped).ToList();

			var data = target.Data;
			var probability = data.Probability;

			probability.Elements.RemoveAll(e => loader.FindElement(e) == null);

			EditorGUILayout.BeginVertical();

			GUILayout.Label("Percentage of tiles", EditorStyles.boldLabel);

			EditorGUILayout.BeginVertical();
			
			var countItems = 6;

			for (var j = 0; j < elements.Count; j += countItems)
			{
				EditorGUILayout.BeginHorizontal();
	
				if (j == 0)
				{
					GUILayout.Button("Lives", EditorUtils.Rect(48, 48));
					countItems -= 1;
				}
				else
					countItems = 6;

				for (var k = j; k < elements.Count && k < j + countItems; ++k)
				{
					var contains = probability.Elements.Contains(elements[k].Type);
					var toggle = GUILayout.Toggle(contains, EditorUtils.ElementContent(elements[k]), "Button", EditorUtils.Rect(48, 48));

					if (toggle == contains)
						continue;

					if (contains)
						probability.Elements.Remove(elements[k].Type);
					else
						probability.Elements.Add(elements[k].Type);
				}

				EditorGUILayout.EndHorizontal();
			}

			EditorGUILayout.EndVertical();

			EditorGUILayout.EndVertical();
		}

		private void CropUp()
		{
			RemoveRow(0);
		}

		private void CropDown()
		{
			RemoveRow(target.Height - 1);
		}

		private void CropLeft()
		{
			RemoveColl(0);
		}

		private void CropRight()
		{
			RemoveColl(target.Width - 1);
		}

		private void RemoveRow(int y)
		{
			if (target.Height <= 1)
				return;

			var cells = target.Data.Cells;

			cells.RemoveAll(e => e.Point.y == y);

			if (y < target.Height - 1)
				foreach (var cell in cells)
					cell.Point -= new Point(0, 1);

			--target.Data.Colls;

			parent.SetUnsaved();
			target.Refresh();
		}

		private void RemoveColl(int x)
		{
			if (target.Width <= 1)
				return;

			var cells = target.Data.Cells;

			cells.RemoveAll(e => e.Point.x == x);

			if (x < target.Width - 1)
				foreach (var cell in cells)
					cell.Point -= new Point(1, 0);

			--target.Data.Rows;

			parent.SetUnsaved();
			target.Refresh();
		}
	}
}
