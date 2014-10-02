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
        Plot ReturnPoke();

        [OperationContract]
        int RegisterClient(int id);

        [OperationContract]
        void createSimulation();
    }

    public interface IClientCallbackInterface
    {
        [OperationContract(IsOneWay = true)]
        void updateClient(string data);
        // TODO: Add your callback service operations here

        [OperationContract(IsOneWay = true)]
        void notifyNewScenario(string data);
    }
}
