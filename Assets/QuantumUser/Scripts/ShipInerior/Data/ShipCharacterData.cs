using AGS.EditorView.ShipEditor.Models;
using AGS.Geom;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AGS.EditorView.ShipEditor.Data
{
    [Serializable]
    public class ShipCharacterData
    {

    }

	[Serializable]
	public class RoomsData
	{
		public List<Room> RoomsList = new List<Room>();
		public bool Visible = true;
		[Serializable]
		public class Room
		{
			public int Index;
			public string Name;
			public List<Point> Points = new List<Point>();
			public Color Color;
			public int EnemyCount;
			public List<Point> DoorsPoints = new List<Point>();
		}
	}
}