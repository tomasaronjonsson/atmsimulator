using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace ATMS_Server
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract(Namespace = "ATMS_Server", SessionMode = SessionMode.Required,
                  CallbackContract = typeof(IClientCallbackInterface))]
    public interface IServerInterface
    {

        [OperationContract]
        string ReturnPoke();


    }


    public interface IClientCallbackInterface
    {
        [OperationContract(IsOneWay = true)]
        void updateClient(string data);
        // TODO: Add your callback service operations here
    }

}
