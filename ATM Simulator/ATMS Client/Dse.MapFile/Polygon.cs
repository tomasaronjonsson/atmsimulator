using System.Collections.Generic;

namespace Dse.MapFile
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
