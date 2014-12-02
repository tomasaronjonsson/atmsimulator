using ATMS_Model;
using GalaSoft.MvvmLight.Messaging;
using Model.ServiceReference1;
using System;
using System.Diagnostics;
using System.ServiceModel;
using System.Threading.Tasks;
using ATMS_Model;
using System.Collections.Generic;
using System.Linq;

namespace Model
{
    public class SimulationModel : Messenger, IServerInterfaceCallback
    {
        //This stores the handler for the callbacks from the server
        //CallbackHandler modelCallbackHandler;

        //This stores the instance of the server client
        public ServerInterfaceClient server;

        #region Properties

        //This indicates if the server is playing the simmulation
        private bool _serverIsPlaying;
        public bool serverIsPlaying
        {
            get { return _serverIsPlaying; }
            set
            {
                if (value != _serverIsPlaying)
                {
                    _serverIsPlaying = value;
                    Messenger.Default.Send(value, "serverIsPlaying");
                }
            }
        }

        //This indicates if the server is available for requests
        private bool _serverIsAvailable;
        public bool serverIsAvailable
        {
            get { return _serverIsAvailable; }
            set
            {
                if (value != _serverIsAvailable)
                {
                    _serverIsAvailable = value;
                    Messenger.Default.Send(value, "serverAvailability");
                }
            }
        }

        //This indicates what the current server time is
        private int _currentServerTime;
        public int currentServerTime
        {
            get { return _currentServerTime; }
            set
            {
                if (value != _currentServerTime)
                {
                    _currentServerTime = value;
                    Messenger.Default.Send(value, "serverTime");
                }
            }
        }

        #endregion

        
        public SimulationModel()
        {
            //indicates if the server is available, set to default value which is not avilable
            serverIsAvailable = false;
            //indicates of the server is playing a scenario, by default he is not
            serverIsPlaying = false;

        }

        //Updates the time on the model and raises the serverIsPlaying flag
        public void notifyTimeUpdate(int currentServerTime)
        {
            this.currentServerTime = currentServerTime;
            serverIsPlaying = true;
        }

        #region Connection & debugging methods

        //Checks the server status & channel
        private void checkServerStatus()
        {
            //Lower the server availability flag while this method is running
            serverIsAvailable = false;

            try
            {
                //Check if the server has been initialized
                if (server == null)
                {
                    //Prepare the instance context
                    InstanceContext instanceContext = new InstanceContext(this);
                    //Initialize the server using the instance context above
                    server = new ServerInterfaceClient(instanceContext);
                }
                //Check the server state
                if (server.State != CommunicationState.Opened)
                {
                    //Prepare the instance context
                    InstanceContext instanceContext = new InstanceContext(this);
                    //Initialize the server using the instance context above
                    server = new ServerInterfaceClient(instanceContext);
                }
            }
            catch (Exception e)
            {
                debugMessage(e.StackTrace);
                //If the current method fails - raise this custom exception
                throw new Exception("ATMS/Model-0002: Failed to check the server.");
            }
        }

        //Populates the new clients with data
        public void startUp()
        {
            //Check the server status & channel
            checkServerStatus();
            //Add the client to the list of clients on the server side and populate with data if there is any
            server.populateClientAsync();
            //Raise the server availability flag
            serverIsAvailable = true;
        }

        //Prints the stack trace
        public void debugMessage(string stackTrace)
        {
            Debug.WriteLine("Client" + stackTrace);
        }

        #endregion

        #region Calls from the ViewModel
        /*
         * Every Task below needs to:
         * -checkServerStatus()                 first
         * 
         * -do work here
         * 
         * -raise the serverIsAvailable flag    lastly
         * */

        //Create the scenario
        public async Task createScenario()
        {
            checkServerStatus();
            await server.createScenarioAsync();
            serverIsAvailable = true;
        }

        //Play / Resume simulation
        public async Task playSimulation()
        {
            checkServerStatus();
            await server.playSimulationAsync();
            serverIsAvailable = true;
        }

        //Pause simulation
        public async Task pauseSimulation()
        {
            checkServerStatus();
            await server.pauseSimulationAsync();
            serverIsAvailable = true;
        }

        //Create a new track
        public async Task createNewTrack()
        {
            Track t = new Track();
            checkServerStatus();
            await server.createNewTrackAsync(t);
            serverIsAvailable = true;
        }

        //Create a new track on the map
        public async Task createNewTrackOnMap(Plot p)
        {
            checkServerStatus();
            await server.createNewTrackOnMapAsync(p);
            serverIsAvailable = true;
        }

        //Remove a track
        public async Task removeTrack(Track t)
        {
            checkServerStatus();
            await server.removeTrackAsync(t);
            serverIsAvailable = true;
        }

        //Edit a track
        public async Task editTrack(Track t)
        {
            checkServerStatus();
            await server.editTrackAsync(t);
            serverIsAvailable = true;
        }

        //Create a new plot and assign a track to it
        public async Task createNewPlot(Track t)
        {
            Plot p = new Plot();
            //Make the plot belong to a track by taking the trackID from the selected track
            p.trackID = t.trackID;
            checkServerStatus();
            await server.createNewPlotAsync(p);
            serverIsAvailable = true;
        }

        //Create a new waypoint on the map and assign a track to it
        public async Task addWaypointToMap(Track t, Plot newPlot, Plot oldPlot)
        {
            Plot p = new Plot();
            //Make the plot belong to a track by taking the trackID from the selected track
            p.trackID = t.trackID;
            //Set the latitude & longitude from the input Plot
            p.latitude = newPlot.latitude;
            p.longitude = newPlot.longitude;

            checkServerStatus();
            await server.createNewWaypointAsync(p, oldPlot);
            serverIsAvailable = true;
        }

        //Remove a plot
        public async Task removePlot(Plot p)
        {
            checkServerStatus();
            await server.removePlotAsync(p);
            serverIsAvailable = true;
        }

        //Edit a plot
        public async Task editPlot(Plot p)
        {
            checkServerStatus();
            await server.editPlotAsync(p);
            serverIsAvailable = true;
        }

        #endregion

        #region ICallbackInterface  methods
        /*
         * These methods are being called from the server
         * and they brodcast a Message containing the object
         * that we need to send to the ViewModel.
         * 
         * Each message sent also has a token. (see the second parameter)
         * This token helps the ViewModel identify and handle
         * each message individually.
         * */

        public void notifyNewScenario(Scenario s)
        {
            Messenger.Default.Send<Scenario>(s, "newScenario");
        }

        public void notifyNewTrack(Track t)
        {
            Messenger.Default.Send<Track>(t, "createTrack");
        }

        public void notifyRemoveTrack(Track t)
        {
            Messenger.Default.Send<Track>(t, "removeTrack");
        }

        public void notifyEditedTrack(Track t)
        {
            Messenger.Default.Send<Track>(t, "editTrack");
        }

        public void notifyNewPlot(Plot p)
        {
            Messenger.Default.Send(p, "createPlot");
        }

        public void notifyNewPlots(Plot[] t)
        {
            foreach (Plot p in t)
            {
                notifyNewPlot(p);
            }
        }

        public void notifyRemovePlot(Plot p)
        {
            Messenger.Default.Send(p, "removePlot");
        }

        public void notifyRemovePlots(Plot[] p)
        {
            foreach (Plot t in p)
            {
                notifyRemovePlot(t);
            }
        }
       

        public void notifyEditedPlot(Plot p)
        {
            Messenger.Default.Send(p, "editPlot");
        }

        public void createNewAutoGeneratedPlots(Plot[] p)
        {
            foreach (Plot item in p)
            {
                notifyNewPlot(item);
            }
        }

        #endregion


        
    }
}
