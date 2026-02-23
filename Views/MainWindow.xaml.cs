using ShemaLavanda.Models;
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
        private readonly string nameFile = "My_shema.svg";

        private MainViewModel vm => (MainViewModel)DataContext;

        public MainWindow()
        {
            InitializeComponent();

            SvgSchemeService service = new SvgSchemeService();
            DataContext = new MainViewModel(service);
        }

        private void svgCanvas_Loaded(object sender, RoutedEventArgs e)
        {
            if (svgCanvas.Drawings != null)
                vm.SvgSchemeService.Parse(svgCanvas.Drawings, $"Assets/{nameFile}");
        }

        private void svgCanvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Point clickPoint = e.GetPosition(svgCanvas);

            foreach(var item in vm.SvgSchemeService.Items.Values)
            {
                foreach(var rect in item.Rects)
                {
                    if (rect.Contains(clickPoint))
                    {
                        Highlight(item);
                        return;
                    }
                }
            }
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

        private void Highlight(EquipmentItem equipment)
        {
            ResetHighlight();

            foreach (GeometryDrawing geo in equipment.GeometryDrawings)
                geo.Brush = Brushes.LimeGreen;
                
        }

        private void ResetHighlight()
        {
            foreach (EquipmentItem list in vm.SvgSchemeService.Items.Values)
                foreach (GeometryDrawing geo in list.GeometryDrawings)
                    geo.Brush = new SolidColorBrush(Color.FromRgb(141, 141, 141)); //Brushes.DarkGray;
        }
    }
}