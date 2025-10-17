using UnityEngine;
using UnityEngine.SceneManagement;
using AGS.EditorView.ShipEditor.Config;
using AGS.EditorView.ShipEditor.Objects;

namespace AGS.EditorView.ShipEditor
{
    public sealed class ShipEditorController
    {
        private static ShipEditorController instance;
        private string levelName;

        public static ShipEditorController Instance
        {
            get
            {
                if (instance != null)
                    return instance;

                instance = new ShipEditorController();
                instance.Start();

                return instance;
            }
        }

        private void Start()
        {
            //initialize common game variables

            Application.targetFrameRate = 60;

            ShipConfig.Load();
        }

        public void LoadGameScene(string levelName)
        {
            this.levelName = levelName;

            SceneManager.LoadScene("Graber");
        }

        public string LevelName
        {
            get { return levelName; }
        }
    }
}
