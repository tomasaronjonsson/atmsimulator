using System.Threading;

namespace ATMS_Server
{
    /*
     * The TimeWorker class implements the concept of
     * time in the system. It makes use of the 
     * BusinessLogicValues in order to independently play 
     * the simulation.
     * */
    public class TimeWorker
    {
        /*
         * We need an instance of the MainSimulation 
         * in order to call the tickTock method.
         * */
        private MainSimulation mainSimulation;
        /*
         * This flag indicates if the TimeWorker is stopped.
         * The volatile keyword is used in order to allow 
         * any number of threads to update the value.
         * */
        private volatile bool stopped;


        public TimeWorker(MainSimulation mainSimulation)
        {
            this.mainSimulation = mainSimulation;
        }


        /*
         * As long as the 'stopped' flag is down,
         * this method makes use of the BusinessLogicValues
         * and the MainSimulation instance to increment the time
         * */
        public void DoWork()
        {
            while (!stopped) 
            {
                //Sleep as much as the radarInterval dictates
                Thread.Sleep(ATMS_Model.BuisnessLogicValues.radarInterval * 1000);
                //call the tickTock method to increment the time on the server
                mainSimulation.tickTock();
            }
        }

        //This method raises the flag and stops the TimeWorker
        public void RequestStop()
        {
            stopped = true;
        }
    }
}
