using MapImporter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MapImporter
{
    public class Tools
    {
        /*
         * This class is used as a tool to import the objects from an xml file with a given path.
         * The method returns a list of MapObjects
         */
        public static List<MapObject> Parse(String path)
        {
            //Create the lsit of objects
            List<MapObject> list = null;

            try
            {
                //Load the XML file
                XDocument xml = XDocument.Load(path);

                /*
                 * Using Linq, the map objects are extracted from the XML and added to the list
                 */
                list = (from x in xml.Root.Elements("maps").Elements("map")
                        select new MapObject
                        {
                            groupname = (string)x.Element("groupname").Value ?? string.Empty,
                            name = (string)x.Element("name").Value ?? string.Empty,
                            image = (string)x.Element("image").Value ?? string.Empty,
                            altitude = (string)x.Element("altitude").Value ?? string.Empty,
                            active = (string)x.Element("active").Value ?? string.Empty,
                            startDate = (string)x.Element("startdate").Value ?? string.Empty,
                            startTime = (string)x.Element("starttime").Value ?? string.Empty,
                            endDate = (string)x.Element("enddate").Value ?? string.Empty,
                            endTime = (string)x.Element("altitude").Value ?? string.Empty,
                        }).ToList();

                //using the parser they gave us, sliglhy modified for our needs we parse the image for each map and store them in the shapes variable
                foreach (MapObject m in list)
                {
                    m.shapes = MapFileParser.Parse(m.image);
                }
            }
            catch (Exception e)
            {
                throw new Exception("File not found.");
            }
            return list;
        }
    }
}
