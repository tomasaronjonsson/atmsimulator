using System.Collections.Generic;

namespace MapImporter
{
   public class Polygon : Shape
   {
      public List<string> Points { get; set; }

      public Polygon()
      {
         Points = new List<string>();
      }
   }
}
