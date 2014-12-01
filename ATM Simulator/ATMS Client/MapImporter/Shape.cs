using System;
using System.Drawing;

namespace MapImporter
{
    public class Shape
    {
        public Nullable<Color> Color { get; set; }
        public LineStyle LineStyle { get; set; }

        public Shape()
        {
            LineStyle = MapImporter.LineStyle.Solid;
        }
    }
}
