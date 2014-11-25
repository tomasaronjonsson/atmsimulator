using System;
namespace ATMS_Model
{
    /*
     * This class holds some values
     * that are essential for the business
     * logic to operate.
     * */
    public static class BuisnessLogicValues
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
                var R = 6371; 

                //Calculate the next Latitude & Longitude points
                var lat2 = Math.Asin(Math.Sin(Math.PI / 180 * p.latitude) * Math.Cos(travelDistanceInKm / R) + Math.Cos(Math.PI / 180 * p.latitude) * Math.Sin(travelDistanceInKm / R) * Math.Cos(Math.PI / 180 * p.course));
                var lon2 = Math.PI / 180 * p.longitude + Math.Atan2(Math.Sin( Math.PI / 180 * p.course) * Math.Sin(travelDistanceInKm / R) * Math.Cos( Math.PI / 180 * p.latitude ), Math.Cos(travelDistanceInKm / R) - Math.Sin( Math.PI / 180 * p.latitude) * Math.Sin(lat2));

                //Assign the location points to the plot
                nextLogicalPlot.latitude = 180 / Math.PI * lat2;
                nextLogicalPlot.longitude = 180 / Math.PI * lon2;
            }

            return nextLogicalPlot;
        }
    }
}
