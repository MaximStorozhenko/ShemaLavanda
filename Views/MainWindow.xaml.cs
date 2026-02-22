using SharpVectors.Converters;
using SharpVectors.Dom.Svg;
using SharpVectors.Runtime;
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
        private readonly string nameShema = "My_shema.svg";

        private string selectedEquipment;
        MainViewModel vm = new();

        public MainWindow()
        {
            InitializeComponent();
            DataContext = vm;
        }

        private void SvgView_Loaded(object sender, RoutedEventArgs e)
        {
            SvgViewbox svgView = (SvgViewbox)sender;

            if (svgView.Drawings is DrawingGroup drawing)
            {
                ((MainViewModel)DataContext).OnSvgLoaded(drawing);
            }
        }

        public void Highlight(string equipmentId)
        {
            MessageBox.Show($"Highlighting equipment: {equipmentId}");
            ResetHighlight();

            if (!vm.SvgSchemeService.Elements.TryGetValue(equipmentId, out List<GeometryDrawing> list))
                return;

            foreach (GeometryDrawing geo in list)
                geo.Brush = Brushes.YellowGreen;
        }

        private void ResetHighlight()
        {
            foreach (var list in vm.SvgSchemeService.Elements.Values)
                foreach (var geo in list)
                    geo.Brush = Brushes.Gray;
        }

        private Dictionary<string, Rect> elementBounds = new Dictionary<string, Rect>();

        private void svgCanvas_Loaded(object sender, RoutedEventArgs e)
        {
            //ParseSvgFile();
        }

        private void svgCanvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Point clickPoint = e.GetPosition(svgCanvas);
            Debug.WriteLine($"\nClick at: X={clickPoint.X}, Y={clickPoint.Y}");

            foreach (var element in elementBounds.Reverse())
            {
                if (element.Value.Contains(clickPoint))
                {
                    Debug.WriteLine($"Hit: {element.Key}");
                    MessageBox.Show($"Clicked ID: {element.Key}");
                    return;
                }
            }

            MessageBox.Show("Clicked outside elements");
        }

        private void ParseSvgFile()
        {
            XDocument doc = XDocument.Load($"Assets/{nameShema}");
            XNamespace ns = "http://www.w3.org/2000/svg";

            foreach (var element in doc.Descendants())
            {
                var id = element.Attribute("id")?.Value;

                if (!string.IsNullOrEmpty(id) && id.StartsWith("BE_") && id.EndsWith("_hit"))
                {
                    Debug.WriteLine($"Found element: {element.Name.LocalName}, ID: {id}");

                    Rect? bounds = GetElementBounds(element);

                    if (bounds.HasValue)
                    {
                        elementBounds[id] = bounds.Value;
                        Debug.WriteLine($"  Bounds: X={bounds.Value.X}, Y={bounds.Value.Y}, W={bounds.Value.Width}, H={bounds.Value.Height}");
                    }
                }
            }
        }

        private Rect? GetElementBounds(XElement element)
        {
            string localName = element.Name.LocalName;

            try
            {
                double x = ParseDouble(element.Attribute("x")?.Value);
                double y = ParseDouble(element.Attribute("y")?.Value);
                double width = ParseDouble(element.Attribute("width")?.Value);
                double height = ParseDouble(element.Attribute("height")?.Value);

                if (width > 0 && height > 0)
                    return new Rect(x, y, width, height);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error parsing bounds: {ex.Message}");
            }

            return null;
        }

        private double ParseDouble(string value)
        {
            if (string.IsNullOrEmpty(value))
                return 0;

            // Убираем единицы измерения (px, pt и т.д.)
            value = new string(value.TakeWhile(c => char.IsDigit(c) || c == '.' || c == '-').ToArray());

            double.TryParse(value,
                System.Globalization.NumberStyles.Any,
                System.Globalization.CultureInfo.InvariantCulture,
                out double result);

            return result;
        }
    }
}