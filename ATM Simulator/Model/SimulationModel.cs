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

        public async Task editTrack(Track t)
        {
            handleServerConnection();
            await server.editTrackAsync(t);
            serverIsAvailable = true;
        }

        /*
         * review Tomas - PLOT MANAGEMENT 
         * 
         * */
        public async Task createNewPlot(Track t)
        {
            Plot p = new Plot();
            p.trackID = t.trackID;
            handleServerConnection();
            await server.createNewPlotAsync(p);
            serverIsAvailable = true;
        }

        public async Task removePlot(Plot p)
        {
            handleServerConnection();
            await server.removePlotAsync(p);
            serverIsAvailable = true;
        }

        public async Task editPlot(Plot p)
        {
            handleServerConnection();
            await server.editPlotAsync(p);
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

            //sending a messeng out that the main scenario has been changed triggering the update process on the view
            Messenger.Default.Send(t, "createTrack");
        }

        public void notifyRemoveTrack(Track t)
        {

            //remove the track from our model
            mainScenario.tracks.Remove(t);

            //broadcasting the track with the token "removetrack"
            Messenger.Default.Send<Track>(t, "removeTrack");
        }
        public void notifyEditedTrack(Track t)
        {
            //find the track to edit
            var trackToEdit = mainScenario.tracks.First(x => x.Equals(t));

            if (trackToEdit != null)
            {
                trackToEdit.edit(t);
                //broadcasting the track with the token "editTrack"
                Messenger.Default.Send<Track>(t, "editTrack");
            }

        }

        public void notifyNewPlot(Plot p)
        {
            //make a reference to the track that we need to add to (the selectedTrack)
            Track trackToBeAddedTo = mainScenario.tracks.First(x => x.trackID == p.trackID);
            //Add the plot
            trackToBeAddedTo.plots.Add(p);

            Messenger.Default.Send(p, "createPlot");
        }

        public void notifyRemovePlot(Plot p)
        {
            //make a reference to the track that we need to add to (the selectedTrack)
            Track trackToBeRemovedFrom = mainScenario.tracks.First(x => x.trackID == p.trackID);
            //Add the plot
            trackToBeRemovedFrom.plots.Remove(p);

            Messenger.Default.Send(p, "removePlot");
        }

        public void notifyEditedPlot(Plot p)
        {
            //finding the track to be changed
            Track trackToLookInto = mainScenario.tracks.First(x => x.trackID == p.trackID);

            //check if we found something
            if (trackToLookInto != null)
            {
                //search for the correct plot in the list of plots
                Plot plotToBeChanged = trackToLookInto.plots.First(x => x.Equals(p));
                //edit what we found
                if (plotToBeChanged != null)
                    plotToBeChanged.edit(p);

                Messenger.Default.Send(p, "editPlot");
            }

        }

    }
}
