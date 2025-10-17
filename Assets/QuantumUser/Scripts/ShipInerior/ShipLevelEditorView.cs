using AGS.EditorView.ShipEditor.Config;
using AGS.EditorView.ShipEditor.Data;
using System;
using UnityEngine;

namespace AGS.EditorView.ShipEditor
{
    public class ShipLevelEditorView : ShipBaseEditorView
    {
        [SerializeField]
        public string Path;

        [NonSerialized]
        public bool LevelLoaded;

        public object EditorController { get; private set; }

        private void Start()
        {
            var n = Path.IndexOf(ShipConfig.ResourcesPath, StringComparison.Ordinal);

            var path = Path.Substring(n + ShipConfig.ResourcesPath.Length);

            n = path.IndexOf(".xml", StringComparison.Ordinal);

            path = path.Substring(0, n);

            ShipEditorController.Instance.LoadGameScene(path);
        }

        public new void Load(ShipLevelData data)
        {
            base.Load(data);

            LevelLoaded = true;
        }
    }
}