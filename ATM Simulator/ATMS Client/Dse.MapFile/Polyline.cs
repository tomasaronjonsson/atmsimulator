using System.Collections.Generic;

namespace Dse.MapFile
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
