using System;

using UnityEngine;

namespace AGS.Edit.Utils
{
    internal sealed class EditorClickDispatcher
    {
        public delegate void Handler(Event e);

        public event Action<Event> onMouseDown = delegate { };
        public event Action<Event> onMouseMove = delegate { };
        public event Action<Event> onMouseDrag = delegate { };
        public event Action<Event> onMouseUp = delegate { };

        public void DispatchEvent(Event e)
        {
            switch (e.type)
            {
                case EventType.MouseDown: OnMouseDown(e); break;
                case EventType.MouseMove: OnMouseMove(e); break;
                case EventType.MouseUp: OnMouseUp(e); break;
                case EventType.MouseDrag: OnMouseDrag(e); break;
            }
        }

        private void OnMouseDown(Event e)
        {
            onMouseDown(e);
        }

        private void OnMouseMove(Event e)
        {
            onMouseMove(e);
        }

        private void OnMouseUp(Event e)
        {
            onMouseUp(e);
        }

        private void OnMouseDrag(Event e)
        {
            onMouseDrag(e);
        }
    }
}