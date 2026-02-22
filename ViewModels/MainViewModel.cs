using GalaSoft.MvvmLight.Command;
using ShemaLavanda.Services;
using ShemaLavanda.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace ShemaLavanda.ViewModels
{
    internal class MainViewModel : ViewModelBase
    {
        public SvgSchemeService SvgSchemeService { get; }

        public RelayCommand ExitProgramCommand => new RelayCommand(() => Application.Current.Shutdown());
        public RelayCommand OpenEquipmentListWindowCommand => new RelayCommand(() =>
        {
            EquipmentListViewModel vm = new();

            foreach (var item in SvgSchemeService.Equipment)
                vm.Items.Add(item);

            EquipmentListWindow equipmentWindow = new(vm);
            equipmentWindow.Owner = Application.Current.MainWindow;
            equipmentWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            equipmentWindow.Show();
        });

        private string selectedEquipmentId;
        public string SelectedEquipmentId
        {
            get => selectedEquipmentId;
            set => Set(ref selectedEquipmentId, value);
        }

        public MainViewModel(SvgSchemeService service)
        {
            SvgSchemeService = service;
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
