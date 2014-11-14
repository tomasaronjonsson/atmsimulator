﻿using ATMS_Model;
using System.ServiceModel;

namespace ATMS_Server
{
    /*
     * This is the interface through
     * which the clients send requests
     * towards the server.
     * 
     * A ServiceContract needs to be specified
     * together with a CallbackContract.
     * */
    [ServiceContract(Namespace = "ATMS_Server", SessionMode = SessionMode.Allowed,
                     CallbackContract = typeof(IClientCallbackInterface))]
    public interface IServerInterface
    {
        /*
         * Below are all the methods that can be called by the clients.
         * 
         * They have suggestive names which leave no doubt to what they are 
         * meant to do.
         * 
         * All methods need to be an Operation Contract
         * [OperationContract]
         * */
        [OperationContract]
        void populateClient();

        [OperationContract]
        void createScenario();

        [OperationContract]
        void playSimulation();

        [OperationContract]
        void createNewTrack(Track t);

        [OperationContract]
        void removeTrack(Track t);

        [OperationContract]
        void editTrack(Track t);

        [OperationContract]
        void createNewPlot(Plot p);

        [OperationContract]
        void removePlot(Plot p);

        [OperationContract]
        void editPlot(Plot p);
    }


    /*
     * This is the interface through which the server
     * is calling back towards the client.
     * */
    public interface IClientCallbackInterface
    {
        /*
         * Below are all the methods that can be called by the server.
         * 
         * They have suggestive names which leave no doubt to what they are 
         * meant to do.
         * 
         * All methods need to be a one way Operation Contract
         * [OperationContract(IsOneWay = true)]
         * */

        [OperationContract(IsOneWay = true)]
        void notifyTimeUpdate(int currentServerTime);

        [OperationContract(IsOneWay = true)]
        void notifyNewScenario(Scenario data);

        [OperationContract(IsOneWay = true)]
        void notifyNewTrack(Track t);

        [OperationContract(IsOneWay = true)]
        void notifyRemoveTrack(Track t);

        [OperationContract(IsOneWay = true)]
        void notifyEditedTrack(Track t);

        [OperationContract(IsOneWay = true)]
        void notifyEditedPlot(Plot t);

        [OperationContract(IsOneWay = true)]
        void notifyNewPlot(Plot t);

        [OperationContract(IsOneWay = true)]
        void notifyRemovePlot(Plot t);
    }
}
