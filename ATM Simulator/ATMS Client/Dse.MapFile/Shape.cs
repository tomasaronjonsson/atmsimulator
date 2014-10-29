using System;
using System.Drawing;

namespace Dse.MapFile
{
   public class Shape
   {
      public Nullable<Color> Color { get; set; }
      public LineStyle LineStyle { get; set; }

      public Shape()
      {
         LineStyle = Dse.MapFile.LineStyle.Solid;
      }
   }
}
