using ATMS_Client.ServiceReference1;
using ATMS_Client.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace ATMS_Client.Models
{
    class Model : IServerInterfaceCallback
    {
        ServerInterfaceClient c1;

        string someData;
        private ViewModel viewModel;


        public Model(ViewModel viewModel)
        {

            // Construct InstanceContext to handle messages on callback interface
            InstanceContext instanceContext = new InstanceContext(this);

            //create a client
            this.c1 = new ServerInterfaceClient(instanceContext);

            this.viewModel = viewModel;
        }

        public void updateClient(string data)
        {
            if (data != null)
                someData = data;

        }

        public void poke()
        {

            viewModel.plotModel = new PlotModel(c1.ReturnPoke());
        }
    }
}
