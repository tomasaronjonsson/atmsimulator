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

        [OperationContract]
        void createNewTrack(Track t);

       
    }

    public interface IClientCallbackInterface
    {
        //notifies all clients of all the time related changes
        [OperationContract(IsOneWay = true)]
        void notifyTimeUpdate(int currentServerTime);

        //notifies all clients of all the changes
        [OperationContract(IsOneWay = true)]
        void notifyNewScenario(Scenario data);

        //notifies all clients of all the changes
        [OperationContract(IsOneWay = true)]
        void notifyNewTrack(Track t);


    }
}
