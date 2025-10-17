using System;
using System.Collections.Generic;
using System.Linq;
using AGS.EditorView.ShipEditor.Data.Conditions;
using AGS.Geom;
using UnityEngine;

namespace AGS.EditorView.ShipEditor.Data
{
	public enum ConditionType
	{
		allitems
	}

	[Serializable]
	public struct ProbabilityInfo
	{
		public int Lives;

		public KeyValuePair<int, int>[] Percentages;
	}

	[Serializable]
	public sealed class ProbabilitySchema
	{
		private List<ProbabilityInfo> config = new List<ProbabilityInfo>();

		private List<int> elements = new List<int>();

		public List<int> Elements
		{
			get { return elements; }
			set { elements = value; }
		}

		public List<ProbabilityInfo> Config
		{
			get { return config; }
			set { config = value; }
		}
	}

	public class ShipLevelData
	{
		private ProbabilitySchema probability = new ProbabilitySchema();
		private ShipConditionData gameCondition = new ShipConditionData();
		private List<ShipCellData> cells = new List<ShipCellData>();
		private List<ShipPlatformData> platforms = new List<ShipPlatformData>();

		private RoomsData rooms = new RoomsData();

		public int width;
		public int height;

		private int colls;
		private int rows;

		private ConditionType conditiontype;

		[NonSerialized]
		private readonly Dictionary<Point, ShipCellData> field = new Dictionary<Point, ShipCellData>();

		public ConditionType ConditionType
		{
			get { return conditiontype; }
			set { conditiontype = value; }
		}

		public int Width
		{
			get { return width; }
			set { width = value; }
		}

		public int Height
		{
			get { return height; }
			set { height = value;}
		}

		public int Colls
		{
			get { return colls; }
			set { colls = value; Resize(); }
		}

		public int Rows
		{
			get { return rows; }
			set { rows = value; Resize(); }
		}

		public float WSize;
		public float HSize;

		public float Cell
		{
			get; set;
		}

		public List<ShipCellData> Cells
		{
			get { return cells; }
			set { cells = value; }
		}

		public ProbabilitySchema Probability
		{
			get { return probability; }
			set
			{
				probability = value;
			}
		}

		public List<ShipPlatformData> Platforms
		{
			get { return platforms; }
			set { platforms = value; }
		}

		public RoomsData Rooms
		{
			get { return rooms; }
			set { rooms = value; }
		}

		public ShipCellData GetCell(Point p)
		{
			if (!field.Any())
				Resize();

			if (field.ContainsKey(p))
				return field[p];

			var cell = new ShipCellData { Point = p };

			field[p] = cell;
			cells.Add(cell);

			return cell;
		}



		private void Resize()
		{
			cells.RemoveAll(e => e.Point.x >= rows || e.Point.y >= colls);

			field.Clear();

			cells.ForEach(e => field[e.Point] = e);
		}
	}
}