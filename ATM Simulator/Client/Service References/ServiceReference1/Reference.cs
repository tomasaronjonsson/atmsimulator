﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18444
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ATMS_Client.ServiceReference1 {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="ATMS_Server", ConfigurationName="ServiceReference1.IServerInterface", CallbackContract=typeof(ATMS_Client.ServiceReference1.IServerInterfaceCallback))]
    public interface IServerInterface {
        
        [System.ServiceModel.OperationContractAttribute(Action="ATMS_Server/IServerInterface/ReturnPoke", ReplyAction="ATMS_Server/IServerInterface/ReturnPokeResponse")]
        ATMS_Model.Plot ReturnPoke();
        
        [System.ServiceModel.OperationContractAttribute(Action="ATMS_Server/IServerInterface/ReturnPoke", ReplyAction="ATMS_Server/IServerInterface/ReturnPokeResponse")]
        System.Threading.Tasks.Task<ATMS_Model.Plot> ReturnPokeAsync();
        
        [System.ServiceModel.OperationContractAttribute(Action="ATMS_Server/IServerInterface/RegisterClient", ReplyAction="ATMS_Server/IServerInterface/RegisterClientResponse")]
        int RegisterClient(int id);
        
        [System.ServiceModel.OperationContractAttribute(Action="ATMS_Server/IServerInterface/RegisterClient", ReplyAction="ATMS_Server/IServerInterface/RegisterClientResponse")]
        System.Threading.Tasks.Task<int> RegisterClientAsync(int id);
        
        [System.ServiceModel.OperationContractAttribute(Action="ATMS_Server/IServerInterface/createSimulation", ReplyAction="ATMS_Server/IServerInterface/createSimulationResponse")]
        void createSimulation(string timestamp);
        
        [System.ServiceModel.OperationContractAttribute(Action="ATMS_Server/IServerInterface/createSimulation", ReplyAction="ATMS_Server/IServerInterface/createSimulationResponse")]
        System.Threading.Tasks.Task createSimulationAsync(string timestamp);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IServerInterfaceCallback {
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="ATMS_Server/IServerInterface/updateClient")]
        void updateClient(string data);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IServerInterfaceChannel : ATMS_Client.ServiceReference1.IServerInterface, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class ServerInterfaceClient : System.ServiceModel.DuplexClientBase<ATMS_Client.ServiceReference1.IServerInterface>, ATMS_Client.ServiceReference1.IServerInterface {
        
        public ServerInterfaceClient(System.ServiceModel.InstanceContext callbackInstance) : 
                base(callbackInstance) {
        }
        
        public ServerInterfaceClient(System.ServiceModel.InstanceContext callbackInstance, string endpointConfigurationName) : 
                base(callbackInstance, endpointConfigurationName) {
        }
        
        public ServerInterfaceClient(System.ServiceModel.InstanceContext callbackInstance, string endpointConfigurationName, string remoteAddress) : 
                base(callbackInstance, endpointConfigurationName, remoteAddress) {
        }
        
        public ServerInterfaceClient(System.ServiceModel.InstanceContext callbackInstance, string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(callbackInstance, endpointConfigurationName, remoteAddress) {
        }
        
        public ServerInterfaceClient(System.ServiceModel.InstanceContext callbackInstance, System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(callbackInstance, binding, remoteAddress) {
        }
        
        public ATMS_Model.Plot ReturnPoke() {
            return base.Channel.ReturnPoke();
        }
        
        public System.Threading.Tasks.Task<ATMS_Model.Plot> ReturnPokeAsync() {
            return base.Channel.ReturnPokeAsync();
        }
        
        public int RegisterClient(int id) {
            return base.Channel.RegisterClient(id);
        }
        
        public System.Threading.Tasks.Task<int> RegisterClientAsync(int id) {
            return base.Channel.RegisterClientAsync(id);
        }
        
        public void createSimulation(string timestamp) {
            base.Channel.createSimulation(timestamp);
        }
        
        public System.Threading.Tasks.Task createSimulationAsync(string timestamp) {
            return base.Channel.createSimulationAsync(timestamp);
        }
    }
}
