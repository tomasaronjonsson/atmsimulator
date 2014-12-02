using DevExpress.Xpf.Map;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel
{
    public class ViewModelMapObject
    {
        /*
         * These variables hold the information from the map file.
         * Each relevant tag in the map file has its own equivalent variable.
         * */
        public String groupname;
        public String name;
        public double altitude;
        public String active;
        public DateTime startDate;
        public DateTime endDate;
        public List<MapItem> mapitems;


        //Default constructor with no arguments for the Linq statements
        public ViewModelMapObject()
        {
            //Initialize the list of MapItems
            mapitems = new List<MapItem>();

            //Assign default values to variables
            groupname = "n/a";
            name = "n/a";
            altitude = 0;
            active = "n/a";
            startDate = DateTime.MinValue;
            endDate = DateTime.MaxValue;
        }

        public ViewModelMapObject(MapImporter.MapObject obj)
        {
            mapitems = new List<MapItem>();

            groupname = obj.groupname;
            name = obj.name;

            try
            {
                altitude = Double.Parse(obj.altitude);
            }
            catch (Exception e)
            {
                altitude = 0;
            }

            active = obj.active;

            try
            {
                startDate = DateTime.Parse(obj.startDate + " " + obj.startTime);
            }
            catch (Exception e)
            {
                startDate = DateTime.MinValue;
            }

            try
            {
                endDate = DateTime.Parse(obj.endDate + " " + obj.endTime);
            }
            catch (Exception e)
            {
                endDate = DateTime.MaxValue;
            }

            //Initialize a list of map items
            List<MapItem> map = new List<MapItem>();

            //Convert from Insero's map objects to our
            foreach (MapImporter.Shape s in obj.shapes)
            {
                //Convert circle to DevExpress MapDot
                if (s is MapImporter.Circle)
                {
                    MapImporter.Circle circle = (MapImporter.Circle)s;

                    MapDot tempDot = new MapDot();
                    tempDot.Size = circle.Radius;

                    mapitems.Add(tempDot);
                }

                //Convert Polygon to DevExpress Polygon
                else if (s is MapImporter.Polygon)
                {
                    MapImporter.Polygon polygon = (MapImporter.Polygon)s;

                    MapPolygon tempPolygon = new MapPolygon();

                    for (int i = 0; i < polygon.Points.Count; i++)
                    {
                        GeoPoint newGeoPoint = new GeoPoint();

                        var split = polygon.Points[i].Split(',');

                        newGeoPoint.Latitude = Double.Parse(split[0], System.Globalization.CultureInfo.InvariantCulture);
                        newGeoPoint.Longitude = Double.Parse(split[1], System.Globalization.CultureInfo.InvariantCulture);

                        tempPolygon.Points.Add(newGeoPoint);
                    }
                    mapitems.Add(tempPolygon);
                }

                //Convert Polyline to DevExpress Polyline
                else if (s is MapImporter.Polyline)
                {
                    MapImporter.Polyline polyline = (MapImporter.Polyline)s;

                    MapPolyline tempPolyline = new MapPolyline();

                    for (int i = 0; i < polyline.Points.Count; i++)
                    {
                        GeoPoint newGeoPoint = new GeoPoint();

                        var split = polyline.Points[i].Split(',');

                        newGeoPoint.Latitude = Double.Parse(split[0], System.Globalization.CultureInfo.InvariantCulture);
                        newGeoPoint.Longitude = Double.Parse(split[1], System.Globalization.CultureInfo.InvariantCulture);

                        tempPolyline.Points.Add(newGeoPoint);
                    }
                    mapitems.Add(tempPolyline);
                }
            }
        }
    }
}
