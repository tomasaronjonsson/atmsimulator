using System;
using System.Drawing;
using System.Globalization;
using System.IO;

namespace Dse.MapFile
{
   public static class MapFileParser
   {
      public static MapFile Parse( string path )
      {
         var mapFile = new MapFile();
         mapFile.Path = path;

         ColorConverter colorConverter = new ColorConverter();

         Nullable<Color> color = null;
         LineStyle lineStyle = LineStyle.Solid;

         string originalLine = null;
         using ( var reader = File.OpenText( path ) )
         {
            var isProcessingPolygon = false;
            var isProcessingPolyline = false;
            Polygon currentPolygon = null;
            Polyline currentPolyline = null;

            while ( ( originalLine = reader.ReadLine() ) != null )
            {
               var line = originalLine.ToUpper();

               // Skip header, empty lines and comments.
               if ( String.IsNullOrWhiteSpace( line ) ||
                  line.StartsWith( "*" ) ||
                  line.StartsWith( "$" ) ||
                  line.StartsWith( "/" ) ||
                  line.StartsWith( "HEADER" ) )
               {
                  continue;
               }

               if ( line.StartsWith( "UNIT" ) )
               {
                  var lineParts = line.Split( ' ' );
                  mapFile.Unit = lineParts[ 1 ];
               }
               else if ( line.StartsWith( "ATB" ) )
               {
                  var lineParts = line.Split( ' ' );
                  foreach ( var linePart in lineParts )
                  {
                     if ( linePart.StartsWith( "COL" ) )
                     {
                        var linePartParts = linePart.Split( '=' );
                        var colorObject = colorConverter.ConvertFromString( linePartParts[ 1 ] );
                        if ( colorObject != null )
                        {
                           color = (Color)colorObject;
                        }
                     }
                     else if ( linePart.StartsWith( "LINE" ) )
                     {
                        var linePartParts = linePart.Split( '=' );
                        Enum.TryParse( linePartParts[ 1 ], true, out lineStyle );
                     }
                  }
               }
               else if ( line.StartsWith( "C (" ) && line.Contains( "POLYGON" ) ) // Start of polygon
               {
                  isProcessingPolygon = true;

                  var lineParts = line.Split( ' ' );

                  var polygon = new Polygon();
                  polygon.Color = color;
                  polygon.LineStyle = lineStyle;
                  polygon.Points.Add( TrimPoint( lineParts[ 1 ] ) );

                  currentPolygon = polygon;
               }
               else if ( line.StartsWith( "C (" ) ) // Start of polyline
               {
                  isProcessingPolyline = true;

                  var lineParts = line.Split( ' ' );

                  var polyline = new Polyline();
                  polyline.Color = color;
                  polyline.LineStyle = lineStyle;
                  polyline.Points.Add( TrimPoint( lineParts[ 1 ] ) );

                  currentPolyline = polyline;
               }
               else if ( line.StartsWith( "C +" ) && isProcessingPolygon )
               {
                  var lineParts = line.Split( ' ' );
                  currentPolygon.Points.Add( TrimPoint( lineParts[ 1 ] ) );
               }
               else if ( line.StartsWith( "C +" ) && isProcessingPolyline )
               {
                  var lineParts = line.Split( ' ' );
                  currentPolyline.Points.Add( TrimPoint( lineParts[ 1 ] ) );
               }
               else if ( line.StartsWith( "C )" ) && isProcessingPolygon )
               {
                  var lineParts = line.Split( ' ' );
                  currentPolygon.Points.Add( TrimPoint( lineParts[ 1 ] ) );

                  mapFile.Shapes.Add( currentPolygon );
                  currentPolygon = null;

                  isProcessingPolygon = false;
               }
               else if ( line.StartsWith( "C )" ) && isProcessingPolyline )
               {
                  var lineParts = line.Split( ' ' );
                  currentPolyline.Points.Add( TrimPoint( lineParts[ 1 ] ) );

                  mapFile.Shapes.Add( currentPolyline );
                  currentPolyline = null;

                  isProcessingPolyline = false;
               }
               else if ( line.StartsWith( "CIR" ) )
               {
                  var circle = new Circle();

                  var lineParts = line.Split( ' ' );
                  foreach ( var linePart in lineParts )
                  {
                     if ( linePart.StartsWith( "C" ) && !linePart.StartsWith( "CIR" ) )
                     {
                        var linePartParts = linePart.Split( '=' );
                        circle.Center = linePartParts[ 1 ];
                     }
                     else if ( linePart.StartsWith( "R" ) )
                     {
                        var linePartParts = linePart.Split( '=' );

                        var radius = default( double );
                        try
                        {
                           radius = Double.Parse( linePartParts[ 1 ], CultureInfo.InvariantCulture );
                        }
                        catch ( Exception ) { }

                        circle.Radius = radius;
                     }
                  }

                  mapFile.Shapes.Add( circle );
               }
               else
               {
                  var message = String.Format( "Could not parse the line \"{0}\" in MAP file \"{1}\".", line, mapFile.Path );
                  throw new Exception( message );
               }
            }
         }

         return mapFile;
      }

      private static string TrimPoint( string point )
      {
         return point.Replace( "(", "" ).Replace( "+", "" ).Replace( ")", "" );
      }
   }
}
