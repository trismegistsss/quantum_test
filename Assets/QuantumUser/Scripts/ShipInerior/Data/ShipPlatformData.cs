using AGS.Geom;

namespace AGS.EditorView.ShipEditor.Data
{
    public class ShipPlatformData 
    {
		public Point Center;
		public int Angle;
		public int Id;

		public Point[] RelCells = {
			new Point(-1,-1), new Point(0,-1), new Point(1, -1),
			new Point(-1,0), new Point(0,0), new Point(1, 0),
			new Point(-1,1), new Point(0,1), new Point(1, 1)
		};
	}
}
