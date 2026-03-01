using GalaSoft.MvvmLight.Command;
using ShemaLavanda.Services;
using ShemaLavanda.Views;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;

namespace ShemaLavanda.ViewModels
{
    internal class MainViewModel : ViewModelBase
    {
        private string date = "";
        public string Date
        {
            get => date;
            set => Set(ref date, value);
        }
        public SvgSchemeService SvgSchemeService { get; }

        public RelayCommand ExitProgramCommand => new RelayCommand(() => Application.Current.Shutdown());
        public RelayCommand OpenEquipmentListWindowCommand => new RelayCommand(() =>
        {
            EquipmentListWindow equipmentListWindow = new EquipmentListWindow();
            equipmentListWindow.Owner = Application.Current.MainWindow;
            equipmentListWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            equipmentListWindow.Show();
        });

        private string selectedEquipmentId;
        public string SelectedEquipmentId
        {
            get => selectedEquipmentId;
            set => Set(ref selectedEquipmentId, value);
        }

        private DispatcherTimer timer;

        public MainViewModel(SvgSchemeService service)
        {
            SvgSchemeService = service;

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += (s, e) =>
            {
                Date = DateTime.Now.ToString("dddd, dd MMMM yyyy") + " г. | " + DateTime.Now.ToString("HH:mm:ss");
            };
            timer.Start();
        }

        private void ShowDate()
        {
            Date = DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss");
        }

        //public void OnSvgLoaded(Drawing drawing)
        //{
        //    SvgSchemeService.Parse(drawing);
        //}

        //public void OnMouseDown(SharpVectors.Converters.SvgViewbox svgView, Point point)
        //{
        //    //HitTestResult hit = VisualTreeHelper.HitTest(svgView, point);
        //    //if (hit == null)
        //    //    return;

        //    //if (hit.VisualHit is DrawingVisual dv)
        //    //{
        //    //    DrawingGroup drawing = dv.Drawing;
        //    //    //HandleDrawingClick(drawing);
        //    //}


        //    //if (!selectionRoute)
        //    //{
        //    //    if (!highlight)
        //    //    {
        //    //        svgSchemeService.Highlight("BE_3.3", Brushes.Yellow);
        //    //        highlight = true;
        //    //    }
        //    //    else
        //    //    {
        //    //        svgSchemeService.Highlight("BE_3.3", Brushes.Gray);
        //    //        highlight = false;
        //    //    }
        //    //    //svgSchemeService.Highlight("BE_3.3", Brushes.Orange);

        //    //    //EquipmentWindow equipmentWindow = new EquipmentWindow();
        //    //    //equipmentWindow.Owner = Application.Current.MainWindow;
        //    //    //equipmentWindow.Show();
        //    //}

        //    ////string id = svgSchemeService.HitTest(point);
        //    ////if (id == null) return;

        //    ////SelectEquipment(id);
        //}
    }
}
