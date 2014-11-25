using System.Collections.Generic;

namespace MapImporter
{
   public class MapFile
   {
      public List<Shape> Shapes { get; set; }
      public string Path { get; set; }
      public string Unit { get; set; }

      public MapFile()
      {
         Shapes = new List<Shape>();

         Unit = "NM";
      }
   }
}
