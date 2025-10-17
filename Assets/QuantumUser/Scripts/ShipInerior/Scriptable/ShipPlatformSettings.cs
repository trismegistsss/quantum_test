using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AGS.EditorView.ShipEditor.Scriptable
{
    public class ShipPlatformSettings : ScriptableObject
    {
        [Serializable]
        public struct Entry
        {
            public GameObject Sprite;
            public float RotationAngle;
        }

        // Mask: 3210
        // 0 - up
        // 1 - right
        // 2 - down
        // 3 - left
        [SerializeField]
        public Entry[] Entries = { };
    }
}