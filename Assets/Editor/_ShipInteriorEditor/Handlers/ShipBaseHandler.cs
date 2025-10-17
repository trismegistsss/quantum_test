using AGS.EditorView.ShipEditor;
using AGS.EditorView.ShipEditor.Data;
using AGS.Geom;
using UnityEngine;

namespace AGS.Edit.ShipInteriorEditor.Handlers
{
	public abstract class ShipBaseHandler
	{
		protected ShipLevelEditor parent;
		protected ShipLevelEditorView target;

		protected ShipBaseHandler handler;
		protected int handlerIndex;
		protected int toolIndex = -1;

		protected ShipBaseHandler(ShipLevelEditor parent)
		{
			this.parent = parent;
			target = parent.Target;
		}

		public abstract void OnInspectorGUI();

		public void OnSceneGUI()
		{
		}

		public virtual void Reload()
		{

		}

		public virtual void DrawCell(Point cell, Rect rect)
		{
		}

		public virtual void OnMouseDown(Vector2 location, Event e)
		{
		}

		public virtual void OnMouseUp(Vector2 location, Event e)
		{
		}

		public virtual void OnMouseMove(Vector2 location, Event e)
		{
		}

		public virtual void OnMouseDrag(Vector2 location, Event e)
		{
		}

		public virtual void OResetHandlerItemSelected()
		{
		}

		public virtual void OnEditElements(Vector2 location)
		{
		}

		public virtual void OnRemoveElements(Vector2 location)
		{
		}

		protected ShipCellData GetCell(Vector2 location)
		{
			var pos = target.ConvertWorldPointToCell(location);

			return target.Data.GetCell(pos);
		}
	}
}