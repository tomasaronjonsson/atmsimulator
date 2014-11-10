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

        #region properties
        //to indicate if the server is aviable for query
        private bool _serverIsPlaying;
        public bool serverIsPlaying
        {
            get { return _serverIsPlaying; }
            set
            {
                if (value != _serverIsPlaying)
                {
                    _serverIsPlaying = value;
                    Messenger.Default.Send(value);
                }
            }
        }

        private bool _serverIsAvailable;

        public bool serverIsAvailable
        {
            get { return _serverIsAvailable; }
            set
            {
                if (value != _serverIsAvailable)
                {
                    _serverIsAvailable = value;
                    Messenger.Default.Send(value);
                }
            }
        }


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

        #endregion

        public SimulationModel()
        {

            serverIsAvailable = false;
            serverIsPlaying = false;

            mainScenario = new Scenario();
            //callback handler
            callbackhandling = new CallbackHandler(this);
        }

        //takes care of the server connection and populating the new instance of the client if that is the case
        public void startUp()
        {
            handleServerConnection();
            server.populateClientAsync();

            serverIsAvailable = true;
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
        public async Task createNewTrack()
        {
            Track t = new Track();
            handleServerConnection();
            await server.createNewTrackAsync(t);
            serverIsAvailable = true;
        }

        public async Task removeTrack(Track t)
        {
            handleServerConnection();
            try
            {
                await server.removeTrackAsync(t);
            }
            catch (Exception e)
            {
                debugMessage("ATMS/Model-0003: Failed to remove track.");
                debugMessage(e.StackTrace);
                //throw new Exception("ATMS/Model-0003: Failed to remove track.");
                handleServerConnection();
            }
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

        internal void notifyNewTrack(Track t)
        {
            //adding the new track to our local scenario
            mainScenario.tracks.Add(t);
            //token string
            string s = "createTrack";
            //sending a messeng out that the main scenario has been changed triggering the update process on the view
            Messenger.Default.Send(t, s);
        }

        public void notifyRemoveTrack(Track t)
        {
            foreach (Track track in mainScenario.tracks)
                if (track.trackID == t.trackID)
                        mainScenario.tracks.Remove(track);

            //token string
            string s = "removeTrack";
            Messenger.Default.Send<Track>(t, s);
        }
    }
}
