using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATMS_Model
{
    public class Test
    {

        #region test methods

        public static Scenario populateScenarioBigger()
        {

            Scenario sc = new Scenario();
            int tracks = 2;
            int plots = 300;

            //starting point for the test flights
            double baseLatitude = 55.850223;
            double baseLongitude = 9.829360;

            //random number object to randomly simulate a track (testing purposes)
            Random randGen = new Random();

            for (int i = 0; i < tracks; i++)
            {
                Track track = new Track();
                track.trackID = i;
                track.callsign = "ISS" + i;

                for (int a = 0; a < plots; a++)
                {
                    Plot plot = new Plot();
                    plot.time = a * BuisnessLogicValues.radarInterval;

                    if (i % 2 == 0)
                    {
                        plot.latitude = baseLatitude + (i * 0.01) + (a * 0.01);
                        plot.longitude = baseLongitude + (i * 0.01) + (a * 0.01);
                    }
                    else
                    {
                        plot.latitude = baseLatitude - (i * 0.01) - (a * 0.01);
                        plot.longitude = baseLongitude - (i * 0.01) -(a * 0.01);
                    }

               

                    //todo
                    plot.altitude = a * 4;

                    track.plots.Add(plot);
                }
                sc.tracks.Add(track);
            }
            return sc;
        }


        //TEST method for populating scenarios with test data
        public static void populateScenario(Scenario sc)
        {
            //here we are going to add 3 tracks, with 3 plots each for testing later

            #region track1
            //creating the first track for out test scenario
            Track t1 = new Track();

            //first plot within the first track
            Plot t1_1 = new Plot();
            t1_1.time = 0;
            t1_1.speed = 500;
            t1_1.latitude = 20;
            t1_1.longitude = 30;
            t1_1.altitude = 40;
            //add the plot to the track
            t1.plots.Add(t1_1);

            //second plot within the first track after 4 seonds
            Plot t1_2 = new Plot();
            t1_2.time = 4;
            t1_2.speed = 500;
            t1_2.latitude = 25;
            t1_2.longitude = 35;
            t1_2.altitude = 35;
            //add the plot to the track
            t1.plots.Add(t1_2);


            //third plot within the first track after 8 seonds
            Plot t1_3 = new Plot();
            t1_3.time = 8;
            t1_3.speed = 500;
            t1_3.latitude = 30;
            t1_3.longitude = 40;
            t1_3.altitude = 30;
            //add the plot to the track
            t1.plots.Add(t1_3);



            #endregion track1

            //addting track 1 the scenario
            sc.tracks.Add(t1);

            #region track2
            //creating the second track for out test scenario
            Track t2 = new Track();

            //first plot within the second track
            Plot t2_1 = new Plot();
            t2_1.time = 0;
            t2_1.speed = 450;
            t2_1.latitude = 120;
            t2_1.longitude = 130;
            t2_1.altitude = 140;
            //add the plot to the track
            t2.plots.Add(t2_1);

            //second plot within the second track after 4 seonds
            Plot t2_2 = new Plot();
            t2_2.time = 4;
            t2_2.speed = 500;
            t2_2.latitude = 125;
            t2_2.longitude = 135;
            t2_2.altitude = 135;
            //add the plot to the track
            t2.plots.Add(t2_2);

            //third plot within the second track after 8 seonds
            Plot t2_3 = new Plot();
            t2_3.time = 8;
            t2_3.speed = 200;
            t2_3.latitude = 130;
            t2_3.longitude = 140;
            t2_3.altitude = 130;
            //add the plot to the track
            t2.plots.Add(t2_3);

            #endregion track2

            //addting track 2 the scenario
            sc.tracks.Add(t2);

            #region track3
            //creating the third track for our test scenario
            Track t3 = new Track();

            //first plot within the third track
            Plot t3_1 = new Plot();
            t3_1.time = 0;
            t3_1.speed = 333;
            t3_1.latitude = 220;
            t3_1.longitude = 230;
            t3_1.altitude = 240;
            //add the plot to the track
            t2.plots.Add(t3_1);

            //second plot within the third track after 4 seonds
            Plot t3_2 = new Plot();
            t3_2.time = 4;
            t3_2.speed = 500;
            t3_2.latitude = 225;
            t3_2.longitude = 225;
            t3_2.altitude = 235;
            //add the plot to the track
            t2.plots.Add(t3_2);

            //third plot within the third track after 8 seonds
            Plot t3_3 = new Plot();
            t3_3.time = 8;
            t3_3.speed = 200;
            t3_3.latitude = 230;
            t3_3.longitude = 240;
            t3_3.altitude = 230;
            //add the plot to the track
            t2.plots.Add(t3_3);
            #endregion track3

            //addting track 3 the scenario
            sc.tracks.Add(t3);
        }
        #endregion
    }
}
