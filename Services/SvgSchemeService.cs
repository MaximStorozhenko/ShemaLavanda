using SharpVectors.Runtime;
using ShemaLavanda.Models;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Media;

namespace ShemaLavanda.Services
{
    internal class SvgSchemeService
    {
        public readonly Dictionary<string, List<GeometryDrawing>> elements = new();
        public IReadOnlyDictionary<string, List<GeometryDrawing>> Elements => elements;

        private readonly Dictionary<GeometryDrawing, string> hitToEquipment = new();
        public IReadOnlyDictionary<GeometryDrawing, string> HitToEquipment => hitToEquipment;

        public ObservableCollection<EquipmentItem> Equipment { get; } = new();

        internal void Parse(Drawing drawing)
        {
            ParseSVG(drawing);
            AddToElements();
            Debug.WriteLine($"Parsed {hitToEquipment.Count} equipment items from SVG.");
            foreach (KeyValuePair<GeometryDrawing, string> entry in hitToEquipment)
            {
                Debug.WriteLine($"item {entry.Value} : {entry.Key} equipment items from SVG.");
                entry.Key.Brush = Brushes.Red; // для отладки - покрасим hit-зоны в красный
            }
            MessageBox.Show($"Equipment {Equipment.Count} equipment items from SVG.", "Parsing Result", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        bool IsEquipmentType(string id)
        {
            return id.StartsWith("BE_") ||  //нория
                   id.StartsWith("BC_") ||  //ленточный конвейер
                   id.StartsWith("CC_") ||  //цепной конвейер
                   id.StartsWith("D_")  ||  //дистрибьютер
                   id.StartsWith("HS_") ||  //весы
                   id.StartsWith("TWV_");   //двухходовой клапан
        }

        //private static readonly Dictionary<string, Brush> tileBrushes = new()
        //{
        //   {"2", tile2Brush },
        //   {"4", tile4Brush },
        //   {"8", tile8Brush },
        //   {"16", tile16Brush },
        //};

        private void ParseSVG(Drawing drawing, string currentId = null)
        {
            if (drawing is DrawingGroup group)
            {
                string id = group.GetValue(SvgObject.IdProperty) as string;
                
                // 1️⃣ Корень оборудования
                if (!string.IsNullOrEmpty(id) && IsEquipmentType(id))
                    currentId = id;

                foreach (var child in group.Children)
                    ParseSVG(child, currentId);
            }
            else if (drawing is GeometryDrawing geo && currentId != null)
            {
                string geoId = geo.GetValue(SvgObject.IdProperty) as string;

                // 2️⃣ HIT-зона
                if (geoId.EndsWith("_hit"))
                {
                    hitToEquipment[geo] = currentId;

                    //MessageBox.Show($"Found hit zone for equipment: {currentId} - {id}");
                    //// запоминаем: ВСЕ геометрии внутри этой группы — hit-зона
                    //foreach (var child in group.Children)
                    //{
                    //    if (child is GeometryDrawing geo)
                    //    {
                    //        hitToEquipment[geo] = currentId;
                    //    }
                    //}

                    return; // ⛔ дальше не идём
                }

                // 3️⃣ Визуальная часть оборудования
                if (!elements.TryGetValue(currentId, out var list))
                {
                    list = new List<GeometryDrawing>();
                    elements[currentId] = list;
                }

                list.Add(geo);
            }
        }

        private void AddToElements()
        {
            foreach (string id in elements.Keys.OrderBy(x => x))
            {
                Equipment.Add(new EquipmentItem(id, "Нория"));
            }
        }
    }
}