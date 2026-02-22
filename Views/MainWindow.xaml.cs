using ShemaLavanda.Services;
using ShemaLavanda.ViewModels;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Xml.Linq;

namespace ShemaLavanda
{
    public partial class MainWindow : Window
    {
        private MainViewModel vm => (MainViewModel)DataContext;

        public MainWindow()
        {
            InitializeComponent();

            SvgSchemeService service = new SvgSchemeService();
            DataContext = new MainViewModel(service);
        }

        //public void Highlight(string equipmentId)
        //{
        //    MessageBox.Show($"Highlighting equipment: {equipmentId}");
        //    ResetHighlight();

        //    if (!vm.SvgSchemeService.Elements.TryGetValue(equipmentId, out List<GeometryDrawing> list))
        //        return;

        //    foreach (GeometryDrawing geo in list)
        //        geo.Brush = Brushes.YellowGreen;
        //}

        //private void ResetHighlight()
        //{
        //    foreach (var list in vm.SvgSchemeService.Elements.Values)
        //        foreach (var geo in list)
        //            geo.Brush = Brushes.Gray;
        //}

        //private Dictionary<string, Rect> elementBounds = new Dictionary<string, Rect>();

        private void svgCanvas_Loaded(object sender, RoutedEventArgs e)
        {
            //if (svgCanvas.Drawings != null)
            //{
            //    vm.SvgSchemeService.Parse(svgCanvas.Drawings);
            //}
            //ParseSvgFile();

            if (svgCanvas.Drawings != null)
                vm.SvgSchemeService.Parse(svgCanvas.Drawings, "Assets/My_shema.svg");
        }

        private void svgCanvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //Point clickPoint = e.GetPosition(svgCanvas);
            //Debug.WriteLine($"\nClick at: X={clickPoint.X}, Y={clickPoint.Y}");

            //foreach (var element in elementBounds.Reverse())
            //{
            //    if (element.Value.Contains(clickPoint))
            //    {
            //        Debug.WriteLine($"Hit: {element.Key}");
            //        MessageBox.Show($"Clicked ID: {element.Key}");
            //        return;
            //    }
            //}

            //MessageBox.Show("Clicked outside elements");

            //HitTestResult hit = VisualTreeHelper.HitTest(svgCanvas, e.GetPosition(svgCanvas));
            //if (hit?.VisualHit is not DrawingVisual dv)
            //    return;

            //GeometryDrawing geo = FindGeometry(dv.Drawing);
            //if (geo == null)
            //    return;

            //if (!vm.SvgSchemeService.HitZones.TryGetValue(geo, out string equipmentId))
            //    return;

            //vm.SelectedEquipmentId = equipmentId;
            //MessageBox.Show($"Clicked on equipment: {equipmentId}");
            //Highlight(equipmentId);
        }

        private GeometryDrawing FindGeometry(Drawing drawing)
        {
            if (drawing is GeometryDrawing geo)
                return geo;

            if (drawing is DrawingGroup group)
            {
                foreach (var child in group.Children)
                {
                    var found = FindGeometry(child);
                    if (found != null)
                        return found;
                }
            }

            return null;
        }
        //private void Highlight(string equipmentId)
        //{
        //    ResetHighlight();

        //    if (!vm.SvgSchemeService.Visuals.TryGetValue(equipmentId, out var list))
        //        return;

        //    foreach (var geo in list)
        //        geo.Brush = Brushes.LimeGreen;
        //}

        //private void ResetHighlight()
        //{
        //    foreach (var list in vm.SvgSchemeService.Visuals.Values)
        //        foreach (var geo in list)
        //            geo.Brush = Brushes.LightGray;
        //}

        //private void ParseSvgFile()
        //{
        //    XDocument doc = XDocument.Load("Assets/My_shema.svg");
        //    XNamespace ns = "http://www.w3.org/2000/svg";

        //    foreach (var element in doc.Descendants())
        //    {
        //        var id = element.Attribute("id")?.Value;

        //        if (!string.IsNullOrEmpty(id) && id.StartsWith("BE_") && id.EndsWith("_hit"))
        //        {
        //            Debug.WriteLine($"Found element: {element.Name.LocalName}, ID: {id}");

        //            Rect? bounds = GetElementBounds(element);

        //            if (bounds.HasValue)
        //            {
        //                //elementBounds[id] = bounds.Value;
        //                Debug.WriteLine($"  Bounds: X={bounds.Value.X}, Y={bounds.Value.Y}, W={bounds.Value.Width}, H={bounds.Value.Height}");
        //            }
        //        }
        //    }
        //}

        
    }
}