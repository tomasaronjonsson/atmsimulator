using System.Collections.Generic;

namespace MapImporter
{
    public class Polyline : Shape
    {
        public List<string> Points { get; set; }

        public Polyline()
        {
            Points = new List<string>();
        }
    }
}
