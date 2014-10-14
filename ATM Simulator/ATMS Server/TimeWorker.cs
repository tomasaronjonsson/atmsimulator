using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace ATMS_Server
{
    public class TimeWorker
    {
        private MainSimulation mainSimulation;

        private volatile bool stop;

        public TimeWorker(MainSimulation mainSimulation)
        {
            this.mainSimulation = mainSimulation;
        }

        public void DoWork()
        {
            while (!stop) 
            {
                Thread.Sleep(ATMS_Model.BuisnessLogicValues.radarInterval * 1000);
                mainSimulation.tickTock();

            }
        }

        public void RequestStop()
        {
            stop = true;
        }
    }
}
