using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;

namespace MapImporter
{
    public static class MapFileParser
    {
        /*
         * We change the way they parse - we take a string and split it up according to what we need
         */
        public static List<Shape> Parse(string toParse)
        {
            ColorConverter colorConverter = new ColorConverter();

            Nullable<Color> color = null;
            LineStyle lineStyle = LineStyle.Solid;

            List<Shape> listOfShapes = new List<Shape>();

            var originalLine = String.Empty;

            using (StringReader reader = new StringReader(toParse))
            {
                var isProcessingPolygon = false;
                var isProcessingPolyline = false;
                Polygon currentPolygon = null;
                Polyline currentPolyline = null;

                while ((originalLine = reader.ReadLine()) != null)
                {
                    var line = originalLine.ToUpper().Trim();

                    // Skip header, empty lines and comments.
                    if (String.IsNullOrWhiteSpace(line) ||
                       line.StartsWith("*") ||
                       line.StartsWith("$") ||
                       line.StartsWith("/") ||
                       line.StartsWith("HEADER"))
                    {
                        continue;
                    }

                    if (line.StartsWith("UNIT"))
                    {
                        var lineParts = line.Split(' ');
                        //mapFile.Unit = lineParts[ 1 ];
                    }
                    else if (line.StartsWith("ATB"))
                    {
                        var lineParts = line.Split(' ');
                        foreach (var linePart in lineParts)
                        {
                            if (linePart.StartsWith("COL"))
                            {
                                var linePartParts = linePart.Split('=');
                                //Catches the exception if it cannot parse the color (due to the file format)
                                try
                                {
                                    var colorParted = linePartParts[1].Split(',');
                                    String colorString = string.Format("#{0:X}{1:X}{2:X}", colorParted[0], colorParted[1], colorParted[2]);

                                    var colorObject = colorConverter.ConvertFromString(colorString);
                                    if (colorObject != null)
                                    {
                                        color = (Color)colorObject;
                                    }
                                }
                                catch (Exception)
                                {
                                    color = Color.Black;
                                }
                            }
                            else if (linePart.StartsWith("LINE"))
                            {
                                var linePartParts = linePart.Split('=');
                                Enum.TryParse(linePartParts[1], true, out lineStyle);
                            }
                        }
                    }
                    else if (line.StartsWith("C (") && line.Contains("POLYGON")) // Start of polygon
                    {
                        isProcessingPolygon = true;

                        var lineParts = line.Split(' ');

                        var polygon = new Polygon();
                        polygon.Color = color;
                        polygon.LineStyle = lineStyle;
                        polygon.Points.Add(TrimPoint(lineParts[1] + "," + lineParts[2]));

                        currentPolygon = polygon;
                    }
                    else if (line.StartsWith("C (")) // Start of polyline
                    {
                        isProcessingPolyline = true;

                        var lineParts = line.Split(' ');

                        var polyline = new Polyline();
                        polyline.Color = color;
                        polyline.LineStyle = lineStyle;
                        polyline.Points.Add(TrimPoint(lineParts[1] + "," + lineParts[2]));

                        currentPolyline = polyline;
                    }
                    else if (line.StartsWith("C +") && isProcessingPolygon)
                    {
                        var lineParts = line.Split(' ');
                        currentPolygon.Points.Add(TrimPoint(lineParts[1] + "," + lineParts[2]));
                    }
                    else if (line.StartsWith("C +") && isProcessingPolyline)
                    {
                        var lineParts = line.Split(' ');
                        currentPolyline.Points.Add(TrimPoint(lineParts[1] + "," + lineParts[2]));
                    }
                    else if (line.StartsWith("C )") && isProcessingPolygon)
                    {
                        var lineParts = line.Split(' ');
                        currentPolygon.Points.Add(TrimPoint(lineParts[1] + "," + lineParts[2]));

                        listOfShapes.Add(currentPolygon);
                        currentPolygon = null;

                        isProcessingPolygon = false;
                    }
                    else if (line.StartsWith("C )") && isProcessingPolyline)
                    {
                        var lineParts = line.Split(' ');
                        currentPolyline.Points.Add(TrimPoint(lineParts[1] + "," + lineParts[2]));

                        listOfShapes.Add(currentPolyline);
                        currentPolyline = null;

                        isProcessingPolyline = false;
                    }
                    else if (line.StartsWith("CIR"))
                    {
                        var circle = new Circle();

                        var lineParts = line.Split(' ');
                        foreach (var linePart in lineParts)
                        {
                            if (linePart.StartsWith("C") && !linePart.StartsWith("CIR"))
                            {
                                var linePartParts = linePart.Split('=');
                                circle.Center = linePartParts[1];
                            }
                            else if (linePart.StartsWith("R"))
                            {
                                var linePartParts = linePart.Split('=');

                                var radius = default(double);
                                try
                                {
                                    radius = Double.Parse(linePartParts[1], CultureInfo.InvariantCulture);
                                }
                                catch (Exception) { }

                                circle.Radius = radius;
                            }
                        }

                        listOfShapes.Add(circle);
                    }
                    else
                    {
                        var message = String.Format("Could not parse.");
                    }
                }
            }

            return listOfShapes;
        }

        private static string TrimPoint(string point)
        {
            return point.Replace("(", "").Replace("+", "").Replace(")", "").Replace("#", "");
        }
    }
}
