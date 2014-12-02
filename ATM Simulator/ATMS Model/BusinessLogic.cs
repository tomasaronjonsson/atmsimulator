using System;
namespace ATMS_Model
{
    /*
     * This class holds some values
     * that are essential for the business
     * logic to operate.
     * */
    public static class BuisnessLogic
    {
        /*
         * This is the radar interval 
         * and it is set to 4 because
         * this is the update interval
         * of the real radar.
         * */
        public static int radarInterval = 4;

        /*
         * These are default values used to create new plots
         * */
        #region Default Values

        //Plot default values
        public static int altitude = 8000;
        public static int speed = 100;
        public static int course = 0;

        //Track default values
        public static char WTC = 'M';
        public static string SSR = "000";
        public static string ADEP = "CPH1";
        public static string ADES = "BLL2";
        public static string ArType = "Airbus A320";

        #endregion

        /*
         * This method generates the next plot from another plot
         * 
         * -creates a new plot
         * -validates the input
         * -prepares the next plot
         * -returns it
         * */
        public static Plot generateNextLogicPlot(Plot p)
        {
            Plot nextLogicalPlot = new Plot();

            if (p != null)
            {
                //Use the same trackID, speed, altitude & course for the next plot
                nextLogicalPlot.trackID = p.trackID;
                nextLogicalPlot.speed = p.speed;
                nextLogicalPlot.altitude = p.altitude;
                nextLogicalPlot.course = p.course;

                //Set the right time for the next plot
                nextLogicalPlot.time = p.time + radarInterval;

                //calculate the travel distance in kilometers
                double travelDistanceInKm = p.speed * radarInterval / 1000;

                //Earth Radius in Km
                double R = 6371;

                //Calculate the next Latitude & Longitude points
                double lat2 = Math.Asin(Math.Sin(Math.PI / 180 * p.latitude) * Math.Cos(travelDistanceInKm / R) + Math.Cos(Math.PI / 180 * p.latitude) * Math.Sin(travelDistanceInKm / R) * Math.Cos(Math.PI / 180 * p.course));
                double lon2 = Math.PI / 180 * p.longitude + Math.Atan2(Math.Sin(Math.PI / 180 * p.course) * Math.Sin(travelDistanceInKm / R) * Math.Cos(Math.PI / 180 * p.latitude), Math.Cos(travelDistanceInKm / R) - Math.Sin(Math.PI / 180 * p.latitude) * Math.Sin(lat2));

                //Assign the location points to the plot
                nextLogicalPlot.latitude = 180 / Math.PI * lat2;
                nextLogicalPlot.longitude = 180 / Math.PI * lon2;
            }

            return nextLogicalPlot;
        }

        /*
         * This method finds the time of a newly created waypoint
         * 
         * -creates a final time to be returned
         * -validates the input
         * -calculates the distance to the new waypoint
         * -calculates the time to the new plot
         * -matches the new time to either the next or the previously available radar intervals
         * -returns it
         * */
        public static int findTimeOfNewPlot(Plot newPlot, Plot oldPlot)
        {
            double finalTime = 0;

            if (newPlot != null && oldPlot != null)
            {
                //Earth radius in km
                double R = 6371;

                double oldLat = oldPlot.latitude * Math.PI / 180;
                double newLat = newPlot.latitude * Math.PI / 180;
                double deltaLat = (newPlot.latitude - oldPlot.latitude) * Math.PI / 180;
                double deltaLon = (newPlot.longitude - oldPlot.longitude) * Math.PI / 180;

                double a = Math.Sin(deltaLat / 2) * Math.Sin(deltaLat / 2) +
                        Math.Cos(oldLat) * Math.Cos(newLat) *
                        Math.Sin(deltaLon / 2) * Math.Sin(deltaLon / 2);
                double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

                //Distance in km
                double d = R * c;

                //Turn distance in meters because speed is in m/s
                double timeResult = d * 1000 / oldPlot.speed;

                finalTime = timeResult;

                //If closer to the previous interval
                if (timeResult % radarInterval < 2 && timeResult % radarInterval > 0)
                {
                    finalTime = timeResult - timeResult % radarInterval;
                }
                //If closer to the next interval
                if (timeResult % radarInterval >= 2)
                {
                    finalTime = timeResult + (radarInterval - (timeResult % radarInterval));
                }
            }

            return (int)finalTime + oldPlot.time;
        }

        /* review Alex
         * 
         * This method calculates the course of an old plot depending on the position of the a new plot
         * 
         * -validate the input
         * -creates the angle
         * 
         * Formulas from: http://www.sunearthtools.com/tools/distance.php#txtDist_3
         * */
        public static double calculateOldPlotCourse(Plot newPlot, Plot oldPlot)
        {
            //set the standard output for this method expected output should betwen 0 and 360 setting -1 to return "nothing"
            double course = -1;

            if (newPlot != null && oldPlot != null)
            {
                double op = Math.Log(Math.Tan(newPlot.latitude / 2 + Math.PI / 4) / Math.Tan(oldPlot.latitude / 2 + Math.PI / 4));
                //finding the difference of long, going east or west?
                double longitudeDifference = oldPlot.longitude - newPlot.longitude;
                //for the bearing caluclating we need absolute value of the  longitude difference
                double lon = Math.Abs(longitudeDifference);
                //calculate the bearings from oldPlot to newPlot
                double bearing = Math.Atan2(lon, op);
                //convert radians to degrees
                course = bearing * 180 / Math.PI;

                //check if the course is in the west or east, if we are going west we need to correct the value (north beeing zero)
                if (longitudeDifference > 0)
                {
                    //invert the value
                    course = Math.Abs(course - 180) + 180;
                }
            }
            return course;

        }
    }
}
