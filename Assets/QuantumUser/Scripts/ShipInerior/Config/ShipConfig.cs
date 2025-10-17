using AGS.EditorView.ShipEditor.Scriptable;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AGS.EditorView.ShipEditor.Config
{ 
    public class ShipConfig
    {
        public const float ShipPlayerSpeed = 0.5f;

        public const string ResourcesPath = "Resources/";

        // ship section data
        public const string ShipAutosavePath = "Assets/Resources/Levels/Ships/Default/autosave.xml";
        public const string ShipDefaultLevelsFolder = "Resources/Levels/Ships";
        public const string ShipDefaultLevelPath = "Levels/Ships/Default";

        private const string assets = "Settings/ShipAssetSettings";
        private const string platforms = "Settings/ShipPlatformSettings";

        public static ShipAssetSettings Assets;
        public static ShipPlatformSettings Platforms;

        public static void Load()
        {
            Assets = Resources.Load<ShipAssetSettings>(assets) ?? ScriptableObject.CreateInstance<ShipAssetSettings>();
            Platforms = Resources.Load<ShipPlatformSettings>(platforms) ?? ScriptableObject.CreateInstance<ShipPlatformSettings>();
        }
    }
}
