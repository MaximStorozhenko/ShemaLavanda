using SharpVectors.Runtime;
using ShemaLavanda.Models;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Media;
using System.Xml.Linq;

namespace ShemaLavanda.Services
{
    internal class SvgSchemeService
    {
        private readonly Dictionary<string, EquipmentItemVisual> items = new();
        public IReadOnlyDictionary<string, EquipmentItemVisual> Items => items;

        private static readonly Dictionary<string, string> EquipmentType = new()
        {
           {"BE", "Нория" },
           {"BC", "Ленточный конвейер" },
           {"CC", "Цепной конвейер" },
           {"D", "Дистрибьютер" },
           {"HS", "Весы" },
           {"TWV", "Двухходовой клапан" },
        };

        internal void Parse(Drawing drawing, string fileNmae)
        {
            XDocument doc = XDocument.Load(fileNmae);
            XNamespace ns = "http://www.w3.org/2000/svg";

            foreach (XElement element in doc.Descendants())
            {
                string hitId = element.Attribute("id")?.Value;

                if (!string.IsNullOrEmpty(hitId) && IsEquipmentType(hitId) && hitId.EndsWith("_hit"))
                {
                    string id = hitId.Split('_')[0] + "_" + hitId.Split('_')[1];

                    Rect? bounds = GetElementBounds(element);

                    if (bounds.HasValue)
                    {
                        if (!items.TryGetValue(id, out EquipmentItemVisual item))
                        {
                            item = new EquipmentItemVisual();
                            items[id] = item;
                            item.Id = id;
                            item.Type = EquipmentType.TryGetValue(id.Split('_')[0] as string, out string type) ? type : "Неизвестно";
                            item.Name = id.Split('_')[1];
                        }

                        item.AddBounds(bounds.Value);
                    }
                }
            }

            ParseVisual(drawing);

            #if DEBUG
            foreach (var kvp in items)
            {
                Debug.WriteLine($"Key: {kvp.Key}");
                Debug.WriteLine($"Value: {kvp.Value}\n____________");
            }
            #endif
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

        bool IsEquipmentType(string id)
        {
            return id.StartsWith("BE_") ||  //нория
                   id.StartsWith("BC_") ||  //ленточный конвейер
                   id.StartsWith("CC_") ||  //цепной конвейер
                   id.StartsWith("D_")  ||  //дистрибьютер
                   id.StartsWith("HS_") ||  //весы
                   id.StartsWith("TWV_");   //двухходовой клапан
        }

        private void ParseVisual(Drawing drawing, string currentId = null, bool insideEq = false, bool insideNotPaint = false)
        {
            if (drawing is DrawingGroup group)
            {
                string id = group.GetValue(SvgObject.IdProperty) as string;

                //MessageBox.Show($"id {id}", "Debug Info", MessageBoxButton.OK, MessageBoxImage.Information);
                // 1️⃣ Корень оборудования
                if (!string.IsNullOrEmpty(id) && IsEquipmentType(id) && !id.EndsWith("_hit") && !id.StartsWith("text"))
                    currentId = id;

                if (!string.IsNullOrEmpty(id) && id.StartsWith("eq_"))
                    insideEq = true;

                // 3️⃣ Вошли в not_paint
                if (!string.IsNullOrEmpty(id) && id.EndsWith("not_paint"))
                    insideNotPaint = true;

                //MessageBox.Show($"Id: {id}\nInsideEq: {insideEq}\nInsideNotPaint: {insideNotPaint}", "Debug Info", MessageBoxButton.OK, MessageBoxImage.Information);

                foreach (Drawing child in group.Children)
                {
                    ParseVisual(child, currentId, insideEq, insideNotPaint);
                }
            }
            else if (drawing is GeometryDrawing geo && currentId != null && insideEq && !insideNotPaint)
            {
                if (!Items.TryGetValue(currentId, out EquipmentItemVisual equipmentItem))
                {
                    equipmentItem = new EquipmentItemVisual();
                    items[currentId] = equipmentItem;
                }

                equipmentItem.AddGeometry(geo);
            }
        }
    }
}