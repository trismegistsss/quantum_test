using AGS.EditorView.ShipEditor.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AGS.EditorView.ShipEditor
{
    [Serializable]
    public enum ShipElementLayer
    {
        Danger,
        Ground,
        Upground1,
        Upground2,
        Characters,
        Overground
    }

    [Serializable]
    public enum VirusStrategy
    {
        Expand = 0,
        Upgrade = 1,
        ExpandAndUpgrade = 2
    }

    [Serializable]
    public enum GameEndStatus
    {
        Victory, Lose, NotEnoughtMatches
    }

    public static class ShipElementDataTypes
    {
        public static readonly Type[] Serializable = {
            typeof(ShipElementData),
            typeof(ShipCellData)
            };
    }
}