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

        public String groupname;

        public String name;

        public double altitude;

        public String active;

        public DateTime startDate;

        public DateTime endDate;

        public List<MapItem> mapitems;

        public ViewModelMapObject()
        {
            mapitems = new List<MapItem>();

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


            //prepeare a list of mapitem it's a binding list because we are going to use it for the property
            List<MapItem> map = new List<MapItem>();


            //change from insero's custom shape to a mapitem
            foreach (MapImporter.Shape s in obj.shapes)
            {

                //chance their circle to devex mapdot
                if (s is MapImporter.Circle)
                {

                    MapImporter.Circle circle = (MapImporter.Circle)s;

                    MapDot tempDot = new MapDot();
                    tempDot.Size = circle.Radius;


                    mapitems.Add(tempDot);

                }
                //inseros polygon to devex polygon
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
                //inseros polyline to devex polyline
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
