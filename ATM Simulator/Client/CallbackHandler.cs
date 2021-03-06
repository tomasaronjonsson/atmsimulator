﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ATMS_Server;

namespace ATMS_Client
{
    class CallbackHandler : ATMS_Client.ServiceReference1.IServerInterfaceCallback
    {
        public delegate void callbackDelegate(String data);
        callbackDelegate callbackMethod;


        public CallbackHandler(callbackDelegate updateMethod)
        {
            callbackMethod = updateMethod;
        }

        public void updateClient(String data)
        {
            callbackMethod(data);
        }
    }
}
