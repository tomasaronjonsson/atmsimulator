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

            // Construct InstanceContext to handle messages on callback interface
            InstanceContext instanceContext = new InstanceContext(callbackhandling);

            Console.WriteLine(instanceContext.State);
            try
            {
                //create a client
                server = new ServerInterfaceClient(instanceContext);
            }
            catch (Exception e)
            {
                //exception thrown! print it somewhere
                Console.WriteLine(e.ToString());
                throw new Exception("ATMS-0001: Couldn't connect to the server");
            }

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
            isServerAvailable = false;
            server.createScenario();
            isServerAvailable = true;
        }
        public void play()
        {
            isServerAvailable = false;
            server.playSimulation();
            isServerAvailable = true;
        }
        #endregion

       

        public void notifyNewScenario(Scenario data)
        {
            mainScenario = data;
        }
    }
}
