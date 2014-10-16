﻿using ATMS_Model;
using ATMS_Server;
using GalaSoft.MvvmLight.Messaging;
using Model.ServiceReference1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

//for debugging
using System.Diagnostics;

namespace Model
{
    public class SimulationModel : Messenger
    {
        //to store the handler for the callbacks from the server
        CallbackHandler callbackhandling;

        //to store the instance of the server client
        public ServerInterfaceClient server;

        //to indicate if the server is aviable for query
        public bool serverIsAvailable;

        public bool serverIsPlaying;


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
            mainScenario = new Scenario();
            //callback handler
            callbackhandling = new CallbackHandler(this);
            startUp();
        }

        //takes care of the server connection and populating the new instance of the client if that is the case
        public void startUp()
        {
            handleServerConnection();
            server.populateClientAsync();

            serverIsAvailable = true;
            serverIsPlaying = false;
        }

        //use this method to check the server status and channel every time you call the server
        private void handleServerConnection()
        {
            serverIsAvailable = false;

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
            }
            catch (Exception e)
            {
                debugMessage(e.StackTrace);
                throw new Exception("ATMS/Model-0002: Failed to check the server.");
            }
        }

        #region Calls from the ViewModel
        public async Task createScenario()
        {
            handleServerConnection();
            await server.createScenarioAsync();
            serverIsAvailable = true;
        }
        public async Task playSimulation()
        {
            handleServerConnection();
            await server.playSimulationAsync();
            serverIsAvailable = true;
        }
        #endregion

        public void notifyNewScenario(Scenario data)
        {
            mainScenario = data;
        }

        public void debugMessage(string stackTrace)
        {
            Debug.WriteLine("Client" + stackTrace);
        }
    }
}
