using Microsoft.EntityFrameworkCore;
using ShemaLavanda.Data;
using ShemaLavanda.Models;
using ShemaLavanda.Services;
using ShemaLavanda.ViewModels;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace ShemaLavanda
{
    public partial class MainWindow : Window
    {
        private readonly string nameFile = "My_shema.svg";

        private MainViewModel vm => (MainViewModel)DataContext;

        private EquipmentItemVisual hoveredItem;

        public MainWindow()
        {
            InitializeComponent();

            SvgSchemeService service = new SvgSchemeService();
            DataContext = new MainViewModel(service);

            using (AppDbContext db = new())
            {
                if (!db.Equipments.Any())
                {
                    db.Equipments.Add(new Equipment
                    {
                        Code = "BE_3.3",
                        Name = "Нория 3.3",
                        Type = "Нория",
                        Description = "Загрузка силоса №3",
                        RpNumber = "RP-12",
                        PlcTagStart = "BE_3_3_START",
                        PlcTagState = "BE_3_3_STATE"
                    });

                    db.SaveChanges();
                }
            }
        }

        private void svgCanvas_Loaded(object sender, RoutedEventArgs e)
        {
            if (svgCanvas.Drawings != null)
                vm.SvgSchemeService.Parse(svgCanvas.Drawings, $"Assets/{nameFile}");
        }

        private void svgCanvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //Point mousePoint = e.GetPosition(svgCanvas);
            //Point svgPoint = GetSvgPoint(mousePoint);

            //foreach (var item in vm.SvgSchemeService.Items.Values)
            //{
            //    foreach(var rect in item.Rects)
            //    {
            //        if (rect.Contains(svgPoint))
            //        {
            //            Highlight(item);
            //            return;
            //        }
            //    }
            //}
        }

        private Point GetSvgPoint(Point mousePoint)
        {
            if (svgCanvas.Drawings == null)
                return mousePoint;

            var bounds = svgCanvas.Drawings.Bounds;

            double viewWidth = svgCanvas.ActualWidth;
            double viewHeight = svgCanvas.ActualHeight;

            double scaleX = viewWidth / bounds.Width;
            double scaleY = viewHeight / bounds.Height;

            double scale = Math.Min(scaleX, scaleY); // для Uniform

            double offsetX = (viewWidth - bounds.Width * scale) / 2;
            double offsetY = (viewHeight - bounds.Height * scale) / 2;

            double svgX = (mousePoint.X - offsetX) / scale;
            double svgY = (mousePoint.Y - offsetY) / scale;

            return new Point(svgX, svgY);
        }

        private void Highlight(EquipmentItemVisual equipment)
        {
            //ResetHighlight();

            foreach (GeometryDrawing geo in equipment.GeometryDrawings)
                geo.Brush = Brushes.LimeGreen;
                
        }

        //private void ResetHighlight()
        //{
        //    foreach (EquipmentItem list in vm.SvgSchemeService.Items.Values)
        //        foreach (GeometryDrawing geo in list.GeometryDrawings)
        //            geo.Brush = new SolidColorBrush(Color.FromRgb(141, 141, 141));
        //}

        private void svgCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            Point mousePoint = e.GetPosition(svgCanvas);
            Point svgPoint = GetSvgPoint(mousePoint);

            EquipmentItemVisual newHovered = null;

            foreach (var item in vm.SvgSchemeService.Items.Values)
            {
                foreach (var rect in item.Rects)
                {
                    if (rect.Contains(svgPoint))
                    {
                        newHovered = item;
                        break;
                    }
                }

                if (newHovered != null)
                    break;
            }

            // 🔥 если навелись на новое оборудование
            if (newHovered != hoveredItem)
            {
                // убрать старую подсветку
                if (hoveredItem != null)
                    ResetHighlight(hoveredItem);

                // подсветить новое
                if (newHovered != null)
                    Highlight(newHovered);

                hoveredItem = newHovered;
            }
        }

        private void ResetHighlight(EquipmentItemVisual item)
        {
            foreach (var geo in item.GeometryDrawings)
                geo.Brush = new SolidColorBrush(Color.FromRgb(141, 141, 141));
        }
    }
}