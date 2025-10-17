using AGS.Edit.ShipInteriorEditor.Handlers;
using AGS.Edit.Utils;
using AGS.EditorView.ShipEditor;
using AGS.EditorView.ShipEditor.Config;
using AGS.EditorView.ShipEditor.Data;
using AGS.EditorView.ShipEditor.Objects;
using AGS.Geom;
using CustomUtils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using UnityEditor;
using UnityEngine;

namespace AGS.Edit.ShipInteriorEditor
{
    [CustomEditor(typeof(ShipLevelEditorView))]
    public class ShipLevelEditor : BaseEditor
    {
		private readonly EditorClickDispatcher dispatcher = new EditorClickDispatcher();
		private ShipBaseHandler handler;
		private int propIndex;
		private bool unsavedChanges;
		private Dictionary<ShipElementLayer, bool> visibleLayer = new Dictionary<ShipElementLayer, bool>();
		internal struct HandlerInfo
		{
			public string Name;
			public Type HandlerType;

			public HandlerInfo(string n, Type t)
			{
				Name = n;
				HandlerType = t;
			}
		}

		private readonly HandlerInfo[] handlers =
		{
			new HandlerInfo("Main", typeof(ShipMainHandler)),
			new HandlerInfo("Map", typeof(ShipMapHandler)),
			new HandlerInfo("Caracters", typeof(ShipCharactersHandler)),
			new HandlerInfo("Rooms", typeof(RoomsEditorHandler)),
		};

		public ShipLevelEditorView Target
		{
			get { return target as ShipLevelEditorView; }
		}

		public Dictionary<ShipElementLayer, bool> VisibleLayer
		{
			get { return visibleLayer; }
		}

		public void SetUnsaved()
		{
			unsavedChanges = true;
			EditorUtility.SetDirty(Target);
		}

		private void OnEnable()
		{
			var gobj = GameObject.FindObjectsOfType<ShipElementView>();

			if (null != gobj)
				foreach (var obj in gobj)
					if (null != obj && null != obj.gameObject)
						GameObject.DestroyImmediate(obj.gameObject);

			for (var i = ShipElementLayer.Danger; i <= ShipElementLayer.Overground; i++)
				visibleLayer[i] = true;

			ShipConfig.Load();

			ShipElementsLoader.Instance.Refresh();

			if (!Target.LevelLoaded)
				Load();

			dispatcher.onMouseUp += OnMouseUp;
			dispatcher.onMouseDown += OnMouseDown;
			dispatcher.onMouseMove += OnMouseMove;
			dispatcher.onMouseDrag += OnMouseDrag;
		}

		private void OnDisable()
		{

			dispatcher.onMouseDown -= OnMouseDown;

			PoolElementsView.ResetElement();

			if (!unsavedChanges)
				return;

			unsavedChanges = false;
			Save(ShipConfig.ShipAutosavePath);
		}

		private void Save(string path)
		{
			var serializer = new XmlSerializer(typeof(ShipLevelData), ShipElementDataTypes.Serializable);

			using (var f = new StreamWriter(path, false, Encoding.UTF8))
			{
				serializer.Serialize(f, Target.Data);

				Debug.LogFormat("Save level to {0}", path);

				AssetDatabase.Refresh();
			}
		}

		private void Load()
		{
			var asset = AssetDatabase.LoadAssetAtPath<TextAsset>(Target.Path);

			var data = XMLHelper.DeserealizeFromXML<ShipLevelData>(asset, ShipElementDataTypes.Serializable);

			if (data != null)
				Target.Load(data);

			if (handler != null)
				handler.Reload();
		}

		public override void OnInspectorGUI()
		{
			EditorGUILayout.BeginVertical();

			EditorGUILayout.BeginHorizontal();

			GUI.enabled = unsavedChanges;

			if (GUILayout.Button("Save"))
			{
				GUI.changed = unsavedChanges = false;

				Save(Target.Path);
			}

			if (GUILayout.Button("Load"))
			{
				GUI.changed = unsavedChanges = false;

				Load();
			}

			EditorGUILayout.EndHorizontal();

			GUI.enabled = true;

			EditorGUILayout.BeginHorizontal();

			EditorGUILayout.LabelField(Target.Path);

			if (GUILayout.Button("Change"))
			{
				var path = EditorUtility.SaveFilePanel("Select XML file", ShipConfig.ShipDefaultLevelsFolder, "", "xml");

				if (!string.IsNullOrEmpty(path))
					Target.Path = EditorUtils.FSPathToAssetPath(path);
			}

			EditorGUILayout.EndHorizontal();

			GUILayout.Space(10);

			GUILayout.Label("Visual setting", EditorStyles.boldLabel);

			EditorGUILayout.BeginHorizontal();
			if (GUILayout.Button("Reset Visual"))
				PoolElementsView.ResetElement();

			EditorGUILayout.EndHorizontal();

			GUILayout.Space(10);

			GUILayout.Label("Model properties", EditorStyles.boldLabel);

			if (GUI.changed)
			{
				SetUnsaved();

				Target.Refresh();
			}

			var strings = handlers.Select(h => h.Name).ToArray();
			propIndex = GUILayout.SelectionGrid(propIndex, strings, 3, GUILayout.MinHeight(24 * ((strings.Length + 2) / 3)));

			EditorGUILayout.EndVertical();

			GUILayout.Space(10);

			if (propIndex < 0 || propIndex > handlers.Length)
				return;

			var handlerInfo = handlers[propIndex];
			if (handler == null || handlerInfo.HandlerType != handler.GetType())
				handler = Activator.CreateInstance(handlerInfo.HandlerType, new object[] { this }) as ShipBaseHandler;

			if (handler == null)
				return;

			GUI.changed = false;
			handler.OnInspectorGUI();
		}

		private void OnSceneGUI()
		{
			var control = GUIUtility.GetControlID(FocusType.Passive);
			HandleUtility.AddDefaultControl(control);

			dispatcher.DispatchEvent(Event.current);

			// Cells coords
			var style = new GUIStyle();
			var data = Target.Data;

			style.normal.textColor = Color.yellow;
			style.fontSize = 18;
			style.fontStyle = FontStyle.Bold;

			var x = Target.transform.position.x - Target.Width * Target.CellSize / 2;
			var y = Target.transform.position.y - Target.Height * Target.CellSize / 2;

			var ch = 'A';

			for (var i = 0; i < Target.Width; ++i)
			{
				var pos = new Vector3(x + i * Target.CellSize, y);

				Handles.Label(pos + new Vector3(Target.CellSize / 2, 0), ch.ToString(), style);
				ch++;
			}

			for (var i = 0; i < Target.Height; ++i)
			{
				var pos = new Vector3(x, y + i * Target.CellSize);

				Handles.Label(pos + new Vector3(-Target.CellSize / 2, Target.CellSize), (Target.Height - i).ToString(), style);
			}

			foreach (var pd in data.Platforms)
			{
				var backgroundColor = GUI.backgroundColor;
				GUI.backgroundColor = Color.black;

				var center = pd.Center;

				foreach (var cell in pd.RelCells)
				{
					var pt = center + cell;
					var position = Target.ConvertCellToWorldPoint(pt);
					var size = Target.CellSize / 2;

					var rect = new Rect(position.x - size, position.y - size, size * 2, size * 2);

					Handles.DrawSolidRectangleWithOutline(rect, new Color(0, 0, 1, 0.3f), Color.black);
				}

				GUI.backgroundColor = backgroundColor;
			}

			for (var i = 0; i < Target.Width; ++i)
			{
				for (var j = 0; j < Target.Height; ++j)
				{
					var p = new Point(i, j);
					var cell = data.GetCell(p);
					
					var tp = Target.transform.TransformPoint(new Vector2(x + p.x * Target.CellSize, y + (Target.Height - 1 - p.y) * Target.CellSize));
					var rect = new Rect(tp.x, tp.y + Target.CellSize, Target.CellSize, -Target.CellSize);

					if (cell.Disabled)
						continue;

					// ELEMENTS DROW
					var loader = ShipElementsLoader.Instance;
					var elements = cell.Elements.Select(e => loader.FindElement(e.Element)).ToList();

					for (var k = ShipElementLayer.Danger; k <= ShipElementLayer.Overground; k++)
					{
						if (!visibleLayer[k])
							continue;

						var layer = k;
						var select = elements.Where(e => e.Layer == layer);

						foreach (var el in select)
						{
							var texture = EditorUtils.GetPreview(el.gameObject);

							if (texture == null)
								continue;

							Graphics.DrawTexture(rect, texture);
						
							AddObjectInMap(cell, el);
						}
					}

					// CHARACTERS
					/*var textures = new[] { "heroicon.png", "enemyicon.png", "bossicon.png" };
					var conditions = new[] { cell.PlayerSpawn, cell.EnemySpawn, cell.BossSpawn };

					for (var k = 0; k < conditions.Length; ++k)
					{
						if (!conditions[k])
							continue;

						var texture = (Texture)EditorGUIUtility.Load(textures[k]);

						Graphics.DrawTexture(rect, texture);
					}*/
					
					Handles.DrawSolidRectangleWithOutline(rect, new Color(0, 0, 0, 0f), Color.red);
					
					if(cell.Elements.Count==0)
						Handles.DrawSolidRectangleWithOutline(rect, new Color(0.3f, 0.3f, 0.3f, 0.5f), new Color(0, 0, 0, 0f));

					if (handler != null)
						handler.DrawCell(p, rect);
				}
			}

			// color 
			if (data.Rooms.RoomsList.Count > 0 && data.Rooms.Visible)
			{
				foreach (var room in data.Rooms.RoomsList)
                {
					var i = 0;
					foreach (var p in room.Points)
					{
						var tp = Target.transform.TransformPoint(new Vector2(x + p.x * Target.CellSize, y + (Target.Height - 1 - p.y) * Target.CellSize));
						var rect = new Rect(tp.x, tp.y + Target.CellSize, Target.CellSize, -Target.CellSize);
						var nc = new Color(room.Color.r, room.Color.g, room.Color.b, 0.2f);
						Handles.DrawSolidRectangleWithOutline(rect, nc, new Color(0, 0, 0, 0f));
						
						if(i == 0)
							Handles.Label(Target.ConvertCellToWorldPoint(p), room.Index.ToString(), style);

						i++;
					}
				}
			}

			CameraLook();
		}

		private void CameraLook()
		{
			try
			{
				var sv = SceneView.lastActiveSceneView;
				var cm = Camera.main;


				cm.transform.position = new Vector3(
					sv.camera.transform.position.x,
					160,//cm.transform.position.y,
					sv.camera.transform.position.y-40f);
			}
			catch
			{

			}

		}

		private void AddObjectInMap(ShipCellData cell, ShipElementView item)
		{
			var fitem = PoolElementsView.MapElements.FirstOrDefault(pr => pr.Cell == cell.Point && pr.Layer == item.Layer);
			if (null == fitem)
            {
				var elobj = PrefabUtility.InstantiatePrefab(item.gameObject) as GameObject;
				var el = elobj.GetComponent<ShipElementView>();
				var spoint = Target.ConvertCellToWorldPoint(cell.Point);
				el.Cell = cell.Point;
				elobj.transform.position = new Vector3(spoint.x, 100, spoint.y);

				PoolElementsView.MapElements.Add(el);
			}
		}

		public void RemoveObjectInMap(ShipCellData cell, ShipElementInfo iteminfo)
		{
			var fitem = PoolElementsView.MapElements.FirstOrDefault(pr => pr.Cell == cell.Point && pr.Layer == iteminfo.Layer);

			if(null!= fitem)
				PoolElementsView.ResetElement(fitem);
		}

		private void OnMouseDown(Event e)
		{
			var loc = HandleEvent(e);

			if (loc == null)
				return;

			handler.OnMouseDown((Vector2)loc, e);
		}

		private void OnMouseUp(Event e)
		{
			var loc = HandleEvent(e);

			if (loc == null)
				return;

			handler.OnMouseUp((Vector2)loc, e);
		}

		private void OnMouseMove(Event e)
		{
			var loc = HandleEvent(e);

			if (loc == null)
				return;

			handler.OnMouseMove((Vector2)loc, e);
		}

		private void OnMouseDrag(Event e)
		{
			var loc = HandleEvent(e);

			if (loc == null)
				return;

			handler.OnMouseDrag((Vector2)loc, e);
		}

		private Vector2? HandleEvent(Event e)
		{
			if (Target == null || handler == null)
				return null;

			e.Use();

			var mouse = HandleUtility.GUIPointToWorldRay(e.mousePosition);
			var collider = Target.GetComponent<BoxCollider2D>();

			var intercept = Physics2D.GetRayIntersection(mouse);

			if (intercept.collider != collider)
				return null;

			return mouse.origin;
		}
	}
}