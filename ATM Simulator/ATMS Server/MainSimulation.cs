using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Text;
using System.Threading;
using ATMS_Model;
using System.Diagnostics;
using System.Threading.Tasks;

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
                if(mainScenario != null)
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

                //notify the clients of the newly added track
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
            //Check the client registration
            checkIfRegistered();

            //Validate the input
            if (t != null)
            {
                //Remove the track 
                mainScenario.tracks.Remove(t);

                //notify all the clients 
                clients.ForEach(
                 delegate(IClientCallbackInterface callback)
                 {
                     callback.notifyRemoveTrack(t);

                 });
            }
        }

        /**
         * sprint 6
         * */
        public void editTrack(Track t)
        {

            //lets check if the client is registered
            checkIfRegistered();
            //check if null else we skip
            if (t != null)
            {
                //finding the track to be changed
                Track trackToBeChanged = mainScenario.tracks.First(x => x.Equals(t));

                //check if we found something
                if (trackToBeChanged != null)
                {
                    //edit what we found
                    trackToBeChanged.edit(t);


                    //notify the other clients
                    clients.ForEach(
                 delegate(IClientCallbackInterface callback)
                 {
                     callback.notifyEditedTrack(t);

                 });
                }
            }
        }

        public void createNewPlot(Plot p)
        {
            //lets check if the client is registered
            checkIfRegistered();
            //check if the value incoming is Null
            if (p != null)
            {
                //make a reference to the track that we need to add to (the selectedTrack)
                Track trackToBeAddedTo = mainScenario.tracks.First(x => x.trackID == p.trackID);
                //Add the plot
                trackToBeAddedTo.plots.Add(p);

                //notify the clients of the newly added track
                clients.ForEach(
                    delegate(IClientCallbackInterface callback)
                    {
                        callback.notifyNewPlot(p);
                    });
            }
        }

        public void removePlot(Plot p)
        {
            checkIfRegistered();
            if (p != null)
            {
                Track trackToBeRemovedFrom = mainScenario.tracks.First(x => x.trackID == p.trackID);
                //remove the plot
                trackToBeRemovedFrom.plots.Remove(p);

                clients.ForEach(
                 delegate(IClientCallbackInterface callback)
                 {
                     callback.notifyRemovePlot(p);
                 });
            }
        }

        public void editPlot(Plot p)
        {
            checkIfRegistered();
            if (p != null)
            {
                //finding the track to be changed
                Track trackToLookInto = mainScenario.tracks.First(x => x.trackID == p.trackID);

                //check if we found something
                if (trackToLookInto != null)
                {
                    //edit what we found
                    Plot plotToBeChanged = trackToLookInto.plots.First(x => x.trackID == p.trackID);

                    //check if we found something and then edit it
                    if (plotToBeChanged != null)
                        plotToBeChanged.edit(p);


                    //notify the other clients
                    clients.ForEach(
                 delegate(IClientCallbackInterface callback)
                 {
                     callback.notifyEditedPlot(p);
                 });
                }
            }
        }







        #endregion

        /*
         * The implementation of the callbacks to the callbackinteerface in the duplex contract
         * */
        #region Communication with client



        public void sendNewScenario()
        {
            foreach (IClientCallbackInterface entry in clients)
            {
                try
                {
                    //Handle the client callbacks, 1st argument is the function, 2nd is the client and 3rd is the client list
                    clients.ForEach(
              delegate(IClientCallbackInterface callback)
              {
                  callback.notifyNewScenario(mainScenario);

              });
                }
                catch (Exception e)
                {
                    //handle that the scenario is to big and can't be sent like this 
                    debugMessage("Failed to send new scenario", e);

                }
            }
        }




        #endregion
    }
}
