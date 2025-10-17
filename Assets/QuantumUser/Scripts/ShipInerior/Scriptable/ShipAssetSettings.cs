using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AGS.EditorView.ShipEditor.Scriptable
{ 
    public class ShipAssetSettings : ScriptableObject
    {
        [SerializeField]
        public string Elements = "ShipMapElements/";
    }
}
