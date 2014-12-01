using System;

namespace ATMS_Model
{
    /*
     * This class holds some methods that help us
     * test, debug and prototype the project.
     * */
    public class Test
    {
        //Create scenario and populate it with a few tracks and a number of plots for each track
        public static Scenario populateInitialScenario()
        {
            Scenario sc = new Scenario();
            int tracks = 1;
            int plots = 1;

            //starting point for the test flights
            double baseLatitude = 0;
            double baseLongitude = 0;

            for (int i = 0; i < tracks; i++)
            {
                Track track = new Track();
                track.trackID = i;
                track.callSign = "ISS" + i;
                track.ADEP = BuisnessLogic.ADEP;
                track.ADES = BuisnessLogic.ADES;
                track.ArType = BuisnessLogic.ArType;
                track.SSR = BuisnessLogic.SSR;
                track.WTC = BuisnessLogic.WTC;

                for (int a = 0; a < plots; a++)
                {
                    Plot plot = new Plot();
                    plot.time = a * BuisnessLogic.radarInterval;
                    plot.trackID = track.trackID;

                    if (i % 2 == 0)
                    {
                        plot.latitude = baseLatitude + (i * 0.01) + (a * 0.01);
                        plot.longitude = baseLongitude + (i * 0.01) + (a * 0.01);
                    }
                    else
                    {
                        plot.latitude = baseLatitude - (i * 0.01) - (a * 0.01);
                        plot.longitude = baseLongitude - (i * 0.01) - (a * 0.01);
                    }

                    plot.altitude = BuisnessLogic.altitude;
                    plot.course = BuisnessLogic.course;
                    plot.speed = BuisnessLogic.speed;

                    track.plots.Add(plot);
                }
                sc.tracks.Add(track);
            }
            return sc;
        }
    }
}
