using ATMS_Model;
using ATMS_Server;
using GalaSoft.MvvmLight.Messaging;
using Model.ServiceReference1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class SimulationModel : Messenger
    {
        private int clientID;

        public ServerInterfaceClient server;
        public bool isServerAvailable;

        public static Scenario mainScenario
        {
            get;
            set
            {
                mainScenario = value;
                Messenger.Default.Send(value);
            }
        }

        public SimulationModel()
        {
            isServerAvailable = false;
            clientID = 0;
            clientID = server.RegisterClient(clientID);
            if (clientID != 0)
            {
                isServerAvailable = true;
            }

        }
        #region ViewModel calls
        public void createScenario()
        {
            server.createScenario();
        }
        #endregion

        class CallbackClient : IClientCallbackInterface
        {
            public void updateClient(string data)
            {
                throw new NotImplementedException();
            }

            public void notifyNewScenario(Scenario data)
            {
                mainScenario = data;
            }
        }
    }
}
