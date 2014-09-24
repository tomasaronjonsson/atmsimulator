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
        string ReturnPoke();

        [OperationContract]
        int RegisterClient(int id);
    }

    public interface IClientCallbackInterface
    {
        [OperationContract(IsOneWay = true)]
        void updateClient(string data);
        // TODO: Add your callback service operations here
    }
}
