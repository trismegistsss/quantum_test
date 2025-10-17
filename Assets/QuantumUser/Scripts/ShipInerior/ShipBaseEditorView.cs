using AGS.EditorView.ShipEditor.Data;
using AGS.Geom;
using UnityEngine;

namespace AGS.EditorView.ShipEditor
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class ShipBaseEditorView : MonoBehaviour
    {
        protected ShipLevelData data = new ShipLevelData();

        //model properties

        public ShipLevelData Data
        {
            get { return data; }
        }

        public int Width
        {
            get { return data.Rows; }
        }

        public int Height
        {
            get { return data.Colls; }
        }

        public float WSize
        {
            get { return data.WSize; }
        }

        public float HSize
        {
            get { return data.HSize; }
        }

        public float CellSize
        {
            get { return data.Cell; }
        }

        public void Load(ShipLevelData data)
        {
            this.data = data;

            Refresh();
        }

        public void Refresh()
        {
            var collider = GetComponent<BoxCollider2D>();

            collider.size = new Vector2(Width * CellSize, Height * CellSize);
        }

        private Point CorrectCell(Point location)
        {
            if (location.x >= Width)
                location.x = Width - 1;
            if (location.x < 0)
                location.x = 0;

            if (location.y >= Height)
                location.y = Height - 1;
            if (location.y < 0)
                location.y = 0;

            return location;
        }

        public Point ConvertWorldPointToCell(Vector2 point)
        {
            var x = Width * CellSize / 2.0f;
            var y = Height * CellSize / 2.0f;

            point = transform.InverseTransformPoint(point) + new Vector3(x, y);

            var location = new Vector2(Mathf.FloorToInt(point.x / CellSize), Mathf.FloorToInt(point.y / CellSize));

            location.y = Height - location.y - 1;

            return CorrectCell(location.ToPoint());
        }

        public Vector2 ConvertCellToWorldPoint(Point point)
        {
            var x = -(Width * CellSize) / 2.0f;
            var y = Height * CellSize / 2.0f;

            var pt = new Vector2(point.x * CellSize, y - point.y * CellSize) + new Vector2(x + CellSize / 2.0f, -CellSize / 2.0f);

            return pt;
        }
    }
}