using System.Windows;
using System.Windows.Media;

namespace ShemaLavanda.Models
{
    public class EquipmentItemVisual
    {
        public string Id { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }

        public List<GeometryDrawing> GeometryDrawings { get; } = new();
        public List<Rect> Rects { get; } = new();

        public void AddBounds(Rect bounds)
        {
            Rects.Add(bounds);
        }

        public void AddGeometry(GeometryDrawing drawing)
        {
            GeometryDrawings.Add(drawing);
        }
    }
}
