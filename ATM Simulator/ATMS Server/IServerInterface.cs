using ATMS_Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace ATMS_Server
{
    [ServiceContract(Namespace = "ATMS_Server", SessionMode = SessionMode.Allowed,
                     CallbackContract = typeof(IClientCallbackInterface))]
    public interface IServerInterface
    {
        [OperationContract]
        void populateClient();

        [OperationContract]
        void createScenario();

        [OperationContract]
        void playSimulation();

        /**
         *  todo: review
         *  sprint 6
         * 
         * */
        [OperationContract]
        void createNewTrack(Track t);

        [OperationContract]
        void removeTrack(Track t);

        [OperationContract]
        void editTrack(Track t);
    }

    public interface IClientCallbackInterface
    {
        //notifies all clients of all the time related changes
        [OperationContract(IsOneWay = true)]
        void notifyTimeUpdate(int currentServerTime);

        //notifies all clients of all the changes
        [OperationContract(IsOneWay = true)]
        void notifyNewScenario(Scenario data);

        /**
         *  todo: review
         *   Sprint 6
         * */
        //notifies all clients of all the changes
        [OperationContract(IsOneWay = true)]
        void notifyNewTrack(Track t);

        //notifies the clients that a track has been removed
        [OperationContract(IsOneWay = true)]
        void notifyRemoveTrack(Track t);

        
        //notifies that a track as been edited
        [OperationContract(IsOneWay = true)]
        void notifyEditedTrack(Track t);
    }
}
