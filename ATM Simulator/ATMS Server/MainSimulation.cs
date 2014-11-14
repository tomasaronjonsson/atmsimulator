using ATMS_Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using System.Threading;

namespace ATMS_Server
{
    /*
     * This is the Server
     * 
     * The server implements the IServerInterface
     * 
     * A Service Behaviour must be set with the following arguments
     * [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Multiple)]
     * */
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class MainSimulation : IServerInterface
    {
        //This holds the list of clients that are connected to the server
        private static List<IClientCallbackInterface> clients;

        //This is the main scenario
        private Scenario mainScenario;

        //This is the current server time in seconds
        int currentServerTime;

        //This is the currently available trackID (for incoming new tracks)
        int avilableTrackID;

        //This is the Thread that runs the system time
        Thread timeThread;


        public MainSimulation()
        {
            //Initialize the list of clients
            clients = new List<IClientCallbackInterface>();

            //Initialize the mainScenario
            mainScenario = new Scenario();

            //currentServerTime & availableTrackID are initialized as 0
            currentServerTime = 0;
            avilableTrackID = 0;
        }


        #region Time, connection & debugging methods

        //Increments the currentServerTime using the BusinessLogicValues
        public void tickTock()
        {
            currentServerTime += ATMS_Model.BuisnessLogicValues.radarInterval;

            //Call back towards the client with the reply
            clients.ForEach(delegate(IClientCallbackInterface callback)
                {
                    callback.notifyTimeUpdate(currentServerTime);
                }
            );
        }

        //Handles debugging message
        public void debugMessage(string explanation, Exception e)
        {
            Debug.WriteLine("ATMS/" + this.GetType().Name + "- " + explanation);
            Debug.WriteLine("Excpetion" + e);
            Debug.WriteLine("Stacktrace:" + e.StackTrace);
        }

        //Check if the client is already on the client list, if not - add it
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
         * The implementation of the IServerInterface
         * */
        #region Service contracts implementation

        /*
         * Populate the newly connected client with any existing data on the Simulation
         * */
        public void populateClient()
        {
            try
            {
                //Check the client registration
                checkIfRegistered();

                //Instantiate the callback using the current channel
                IClientCallbackInterface callback = OperationContext.Current.GetCallbackChannel<IClientCallbackInterface>();

                //Call back towards the client with the reply
                callback.notifyNewScenario(mainScenario);
            }
            catch (Exception e)
            {
                //Catch and report the exception
                debugMessage("Failed to populate the client", e);
                throw new Exception("ATMS-MainSimulation-0001: Failed to populate the client.");
            }
        }

        /*
         * Create the main scenario and notify the connected clients
         * */
        public void createScenario()
        {
            try
            {
                //Check the client registration
                checkIfRegistered();

                //Create the new scenario
                mainScenario = new Scenario();

                //Populate the scenario with the Test data
                if (mainScenario != null)
                    mainScenario = Test.populateInitialScenario();

                //Update the available ID
                avilableTrackID = mainScenario.tracks.Count;

                //Call back towards the client with the reply
                clients.ForEach(delegate(IClientCallbackInterface callback)
                    {
                        callback.notifyNewScenario(mainScenario);
                    }
                );
            }
            catch (Exception e)
            {
                //Catch and report the exception
                debugMessage("Failed to create and send the scenario", e);
                throw new Exception("ATMS-MainSimulation-0002: Failed to create a new scenario");
            };
        }

        /*
         * Play the simulation
         * */
        public void playSimulation()
        {
            try
            {
                //Check the client registration
                checkIfRegistered();

                //Start the time worker thread which plays the simulation
                if (timeThread == null)
                {
                    //Instantiate the TimeWorker
                    TimeWorker worker = new TimeWorker(this);
                    if (worker != null)
                    {
                        //Call the DoWork method
                        timeThread = new Thread(worker.DoWork);
                        //Start the thread
                        timeThread.Start();
                    }
                }
            }
            catch (Exception e)
            {
                //Catch and report the exception
                debugMessage("Failed to play the simulation", e);
                throw new Exception("ATMS-MainSimulation-0003: Failed to play the simulation");
            }
        }

        /**
         * Create a new track
         * */
        public void createNewTrack(Track t)
        {
            try
            {
                //Check the client registration
                checkIfRegistered();

                //Validate the input
                if (t == null)
                    t = new Track();

                //Assign an available ID and increment the availableTrackID
                t.trackID = avilableTrackID;
                avilableTrackID++;

                //Add the track to the main scenario
                mainScenario.tracks.Add(t);

                //Call back towards the client with the reply
                clients.ForEach(delegate(IClientCallbackInterface callback)
                    {
                        callback.notifyNewTrack(t);
                    }
                );
            }
            catch (Exception e)
            {
                //Catch and report the exception
                debugMessage("Failed to create a new track", e);
                throw new Exception("ATMS-MainSimulation-0004: Failed to create a new track");
            }
        }

        /**
         * Remove a track
         * */
        public void removeTrack(Track t)
        {
            try
            {
                //Check the client registration
                checkIfRegistered();

                //Validate the input
                if (t != null)
                {
                    //Remove the track 
                    mainScenario.tracks.Remove(t);

                    //Call back towards the client with the reply
                    clients.ForEach(delegate(IClientCallbackInterface callback)
                        {
                            callback.notifyRemoveTrack(t);
                        }
                    );
                }
            }
            catch (Exception e)
            {
                //Catch and report the exception
                debugMessage("Failed to remove the track", e);
                throw new Exception("ATMS-MainSimulation-0005: Failed to remove the track");
            }
        }

        /**
         * Edit a track
         * */
        public void editTrack(Track t)
        {
            try
            {
                //Check the client registration
                checkIfRegistered();

                //Validate the input
                if (t != null)
                {
                    //Reference the track that will be changed
                    Track trackToBeChanged = mainScenario.tracks.First(x => x.Equals(t));

                    //Validate the track
                    if (trackToBeChanged != null)
                    {
                        //Edit the track
                        trackToBeChanged.edit(t);

                        //Call back towards the client with the reply
                        clients.ForEach(delegate(IClientCallbackInterface callback)
                            {
                                callback.notifyEditedTrack(t);
                            }
                        );
                    }
                }
            }
            catch (Exception e)
            {
                //Catch and report the exception
                debugMessage("Failed to edit the track", e);
                throw new Exception("ATMS-MainSimulation-0006: Failed to edit the track");
            }
        }

        /*
         * Create a new plot
         * */
        public void createNewPlot(Plot p)
        {
            try
            {
                //Check the client registration
                checkIfRegistered();

                //Validate the input
                if (p != null)
                {
                    //Reference the track that will hold the new plot
                    Track trackToBeAddedTo = mainScenario.tracks.First(x => x.trackID == p.trackID);

                    //Add the new plot
                    trackToBeAddedTo.plots.Add(p);

                    //Call back towards the client with the reply
                    clients.ForEach(delegate(IClientCallbackInterface callback)
                        {
                            callback.notifyNewPlot(p);
                        }
                    );
                }
            }
            catch (Exception e)
            {
                //Catch and report the exception
                debugMessage("Failed to create the plot", e);
                throw new Exception("ATMS-MainSimulation-0007: Failed to create the plot");
            }
        }

        /*
         * Remove a plot
         * */
        public void removePlot(Plot p)
        {
            try
            {
                //Check the client registration
                checkIfRegistered();

                //Validate the input
                if (p != null)
                {
                    //Reference the track where the plot will be removed from
                    Track trackToBeRemovedFrom = mainScenario.tracks.First(x => x.trackID == p.trackID);

                    //Remove the plot
                    trackToBeRemovedFrom.plots.Remove(p);

                    //Call back towards the client with the reply
                    clients.ForEach(delegate(IClientCallbackInterface callback)
                        {
                            callback.notifyRemovePlot(p);
                        }
                    );
                }
            }
            catch (Exception e)
            {
                //Catch and report the exception
                debugMessage("Failed to remove the plot", e);
                throw new Exception("ATMS-MainSimulation-0008: Failed to remove the plot");
            }
        }

        /*
         * Edit a plot
         * */
        public void editPlot(Plot p)
        {
            try
            {
                //Check the client registration
                checkIfRegistered();

                //Validate the input
                if (p != null)
                {
                    //Reference the track where the plot will be removed from
                    Track trackToLookInto = mainScenario.tracks.First(x => x.trackID == p.trackID);

                    //Validate the track
                    if (trackToLookInto != null)
                    {
                        //Reference the plot that will be edited
                        Plot plotToBeChanged = trackToLookInto.plots.First(x => x.trackID == p.trackID);

                        //Validate the plot
                        if (plotToBeChanged != null)
                            plotToBeChanged.edit(p);

                        //Call back towards the client with the reply
                        clients.ForEach(delegate(IClientCallbackInterface callback)
                            {
                                callback.notifyEditedPlot(p);
                            }
                        );
                    }
                }
            }
            catch (Exception e)
            {
                //Catch and report the exception
                debugMessage("Failed to edit the plot", e);
                throw new Exception("ATMS-MainSimulation-0009: Failed to edit the plot");
            }
        }

        #endregion
    }
}
