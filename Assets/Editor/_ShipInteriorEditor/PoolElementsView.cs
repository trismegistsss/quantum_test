using AGS.EditorView.ShipEditor.Data;
using AGS.EditorView.ShipEditor.Objects;
using System.Collections.Generic;
using UnityEngine;

namespace AGS.Edit.ShipInteriorEditor
{
    public static class PoolElementsView
    {
        public static List<ShipElementView> MapElements = new List<ShipElementView>();

        public static void ResetElement(ShipElementView e)
        {
            GameObject.DestroyImmediate(e.gameObject);
            MapElements.Remove(e);
        }

        public static void ResetElement()
        {
            MapElements.RemoveAll(pr =>
            {
                if(null!= pr|| null!= pr.gameObject)
                    GameObject.DestroyImmediate(pr.gameObject);
                return true;
            });

            MapElements.Clear();
        }
    }
}