using System;
using AGS.EditorView.ShipEditor.Models;
using UnityEngine;

namespace AGS.EditorView.ShipEditor.Data
{
    [Serializable]
    public class ShipElementData
    {
        public ShipElementLayer Layer;
        public int Type = ShipElement.None;
        public bool Maped;

        // type object
        public bool Dangerous;
        public bool TempDangerous;

        public bool Movable;

        public bool Invisible;
        public bool TempInvisible;
        public bool ActionInvisible;

        public bool Grounds;

        public bool Barrier;
        public bool Door;

        public bool Item;
        public bool Decor;

        // type object
        public bool Player;
        // type object
        public bool Enemy;

        // type object
        public bool Exit;
    }
}