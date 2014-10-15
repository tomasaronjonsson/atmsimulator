using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATMS_Server
{
    public class TimeoutWorker
    {
        public TimeoutWorker() { }

        public void DoWork(Action a, IClientCallbackInterface client, List<IClientCallbackInterface> clientsList)
        {
            try
            {
                a.Invoke();
            }
            catch (Exception)
            {
                clientsList.Remove(client);
            }
        }

        public void RequestStop()
        {
        }
    }
}
