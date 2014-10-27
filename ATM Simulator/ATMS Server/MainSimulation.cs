using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Text;
using System.Threading;
using ATMS_Model;

//for debugging
using System.Diagnostics;

namespace ATMS_Server
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class MainSimulation : IServerInterface
    {
        private static List<IClientCallbackInterface> clients;

        //this is the main scenario
        private Scenario mainScenario;

        //declaring a dictionary to store the layered scenarios , the 
        private Dictionary<int, Scenario> layeredScenarios;

        //current server time in seconds
        int currentServerTime;

        //the thread for incrementing time (current server time)
        Thread timeThread;


        public MainSimulation()
        {
            #region Initilizing variables
            clients = new List<IClientCallbackInterface>();
            layeredScenarios = new Dictionary<int, Scenario>();
            currentServerTime = 0;
            mainScenario = new Scenario();
            #endregion
        }

        /*
         *  The implementation of the IServerInterface for the duplex contract
         * */
        #region IserverInterface

        public void playSimulation()
        {
            //check if the client is in the callback list
            checkIfRegistered();

            if (timeThread == null)
            {
                //start the timeworker thread who is responsable for calling the incremental time funciton depending on the radarinterval
                TimeWorker worker = new TimeWorker(this);
                timeThread = new Thread(worker.DoWork);
                timeThread.Start();
            }
        }

        public void populateClient()
        {
            try
            {
                checkIfRegistered();
                IClientCallbackInterface callback = OperationContext.Current.GetCallbackChannel<IClientCallbackInterface>();

                //send the data 
                ThreadPool.QueueUserWorkItem(work => handleClientCallback(() => { callback.notifyNewScenario(mainScenario); }, callback));

            }
            catch (Exception e)
            {
                debugMessage(e);
                throw new Exception("ATMS-MainSimulation-0001: Failed to register client");

            }
        }

        public void createScenario()
        {
            try
            {
                //check if the client is in the callback list
                checkIfRegistered();

                mainScenario = new Scenario();

                mainScenario = Test.populateScenarioBigger();
              
                //sending the new scenario to the clients
                ThreadPool.QueueUserWorkItem(a => sendNewScenario());

            }
            catch (Exception e)
            {
                debugMessage(e);
                throw new Exception("ATMS-MainSimulation-0001: Failed to create a new scenario");
            };
        }

        public void checkIfRegistered()
        {
            IClientCallbackInterface callback = OperationContext.Current.GetCallbackChannel<IClientCallbackInterface>();

            if (!clients.Contains(callback))
            {
                clients.Add(callback);
            }
        }



        #endregion

        /*
         *  The implementation of the callbacks to the callbackinteerface in the duplex contract
         * */
        #region Communication with client



        public void sendNewScenario()
        {
            foreach (IClientCallbackInterface entry in clients)
            {
                try
                {
                    //Handle the client callbacks, 1st argument is the function, 2nd is the client and 3rd is the client list
                    ThreadPool.QueueUserWorkItem(work => handleClientCallback(() => { entry.notifyNewScenario(mainScenario); }, entry));
                }
                catch (Exception e)
                {
                    //handle that the scenario is to big and can't be sent like this 
                    debugMessage(e);

                }
            }
        }

        #endregion

        /*
         *  Implementation of the time incrementer and the notification of the clients
         * */
        public void tickTock()
        {
            //incrementing the time by the value in the radarinterval
            currentServerTime += ATMS_Model.BuisnessLogicValues.radarInterval;

            //notifying listening clients of the update
            foreach (IClientCallbackInterface entry in clients)
            {
                ThreadPool.QueueUserWorkItem(work => handleClientCallback(() => { entry.notifyTimeUpdate(currentServerTime); }, entry));

            }
        }

        public void handleClientCallback(Action action, IClientCallbackInterface client)
        {

            try
            {
                action.Invoke();
            }
            catch (Exception e)
            {
                clients.Remove(client);
                debugMessage(e);
            }
        }
       

        public void debugMessage(Exception e)
        {
            Debug.WriteLine("Excpetion" + e);
            Debug.WriteLine("Stacktrace:" + e.StackTrace);
        }
    }
}
