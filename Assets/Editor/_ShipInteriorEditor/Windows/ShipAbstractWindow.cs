using UnityEditor;

namespace AGS.Edit.ShipInteriorEditor.Window
{ 
    internal abstract class ShipAbstractWindow : EditorWindow
    {
        public delegate void Event(ShipAbstractWindow w);

        public event Event onWindowClosed = delegate { };

        private void OnDestroy()
        {
            onWindowClosed(this);
        }
    }
}