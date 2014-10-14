using ATMS_Model;
using ATMS_Server;
using GalaSoft.MvvmLight.Messaging;
using Model.ServiceReference1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class SimulationModel : Messenger
    {
        private int clientID;

        //to store the handler for the callbacks from the server
        CallbackHandler callbackhandling;

        //to store the instance of the server client
        public ServerInterfaceClient server;

        //to indicate if the server is aviable for query
        public bool isServerAvailable;

        private Scenario _mainScenario;
        public Scenario mainScenario
        {
            set
            {
                _mainScenario = value;
                Messenger.Default.Send(value);
            }
            get
            {
                return _mainScenario;
            }
        }

        private int _currentServerTime;
        public int currentServerTime
        {
            get { return _currentServerTime; }
            set
            {
                if (value != _currentServerTime)
                {
                    _currentServerTime = value;
                    Messenger.Default.Send(value);
                }
            }
        }


        public SimulationModel()
        {
            //callback handler
            callbackhandling = new CallbackHandler(this);

            checkServer();

            isServerAvailable = false;
            clientID = 0;
            if (checkServer())
            {
                isServerAvailable = true;
            }
        }

        //use this method to check the server status and channel every time you call the server
        private bool checkServer()
        {
            try
            {
                if (server == null)
                {
                    InstanceContext instanceContext = new InstanceContext(callbackhandling);
                    server = new ServerInterfaceClient(instanceContext);
                }
                if (server.State != CommunicationState.Opened)
                {
                    InstanceContext instanceContext = new InstanceContext(callbackhandling);
                    server = new ServerInterfaceClient(instanceContext);
                }
                if (!server.checkIfRegistered())
                {
                    server.RegisterClient();
                }
                return true;
            }
            catch (Exception)
            {
                throw new Exception("ATMS/Model-0002: Failed to check the server.");
            }
            return false;
        }

        #region ViewModel calls
        public void createScenario()
        {
            isServerAvailable = false;
            if (checkServer())
            {
                server.createScenario();
            }
            isServerAvailable = true;
        }
        public void play()
        {
            isServerAvailable = false;
            if (checkServer())
            {
                server.playSimulation();
            }
            isServerAvailable = true;
        }
        #endregion

        public void notifyNewScenario(Scenario data)
        {
            mainScenario = data;
        }
    }
}
