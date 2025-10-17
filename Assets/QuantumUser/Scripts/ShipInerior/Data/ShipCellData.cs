using AGS.Geom;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace AGS.EditorView.ShipEditor.Data
{
    [Serializable]
    public class ShipElementInfo
    {
        public string Id;
        public ShipElementLayer Layer;
        public int Element;
        public bool IsAdd;
        public bool Doors;
        public bool PlayerSpawn;
        public bool EnemySpawn;
        public bool BossSpawn;
    }

    [Serializable]
    public sealed class ShipCellData
    {
        //common props
        public Point Point;

        public List<ShipElementInfo> Elements = new List<ShipElementInfo>();

        //disabled(invisible)
        public bool Disabled;      
    }
}
