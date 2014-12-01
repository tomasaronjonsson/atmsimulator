﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18444
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Model.ServiceReference1 {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="ATMS_Server", ConfigurationName="ServiceReference1.IServerInterface", CallbackContract=typeof(Model.ServiceReference1.IServerInterfaceCallback))]
    public interface IServerInterface {
        
        [System.ServiceModel.OperationContractAttribute(Action="ATMS_Server/IServerInterface/populateClient", ReplyAction="ATMS_Server/IServerInterface/populateClientResponse")]
        void populateClient();
        
        [System.ServiceModel.OperationContractAttribute(Action="ATMS_Server/IServerInterface/populateClient", ReplyAction="ATMS_Server/IServerInterface/populateClientResponse")]
        System.Threading.Tasks.Task populateClientAsync();
        
        [System.ServiceModel.OperationContractAttribute(Action="ATMS_Server/IServerInterface/createScenario", ReplyAction="ATMS_Server/IServerInterface/createScenarioResponse")]
        void createScenario();
        
        [System.ServiceModel.OperationContractAttribute(Action="ATMS_Server/IServerInterface/createScenario", ReplyAction="ATMS_Server/IServerInterface/createScenarioResponse")]
        System.Threading.Tasks.Task createScenarioAsync();
        
        [System.ServiceModel.OperationContractAttribute(Action="ATMS_Server/IServerInterface/playSimulation", ReplyAction="ATMS_Server/IServerInterface/playSimulationResponse")]
        void playSimulation();
        
        [System.ServiceModel.OperationContractAttribute(Action="ATMS_Server/IServerInterface/playSimulation", ReplyAction="ATMS_Server/IServerInterface/playSimulationResponse")]
        System.Threading.Tasks.Task playSimulationAsync();
        
        [System.ServiceModel.OperationContractAttribute(Action="ATMS_Server/IServerInterface/createNewTrack", ReplyAction="ATMS_Server/IServerInterface/createNewTrackResponse")]
        void createNewTrack(ATMS_Model.Track t);
        
        [System.ServiceModel.OperationContractAttribute(Action="ATMS_Server/IServerInterface/createNewTrack", ReplyAction="ATMS_Server/IServerInterface/createNewTrackResponse")]
        System.Threading.Tasks.Task createNewTrackAsync(ATMS_Model.Track t);
        
        [System.ServiceModel.OperationContractAttribute(Action="ATMS_Server/IServerInterface/createNewTrackOnMap", ReplyAction="ATMS_Server/IServerInterface/createNewTrackOnMapResponse")]
        void createNewTrackOnMap(ATMS_Model.Plot p);
        
        [System.ServiceModel.OperationContractAttribute(Action="ATMS_Server/IServerInterface/createNewTrackOnMap", ReplyAction="ATMS_Server/IServerInterface/createNewTrackOnMapResponse")]
        System.Threading.Tasks.Task createNewTrackOnMapAsync(ATMS_Model.Plot p);
        
        [System.ServiceModel.OperationContractAttribute(Action="ATMS_Server/IServerInterface/removeTrack", ReplyAction="ATMS_Server/IServerInterface/removeTrackResponse")]
        void removeTrack(ATMS_Model.Track t);
        
        [System.ServiceModel.OperationContractAttribute(Action="ATMS_Server/IServerInterface/removeTrack", ReplyAction="ATMS_Server/IServerInterface/removeTrackResponse")]
        System.Threading.Tasks.Task removeTrackAsync(ATMS_Model.Track t);
        
        [System.ServiceModel.OperationContractAttribute(Action="ATMS_Server/IServerInterface/editTrack", ReplyAction="ATMS_Server/IServerInterface/editTrackResponse")]
        void editTrack(ATMS_Model.Track t);
        
        [System.ServiceModel.OperationContractAttribute(Action="ATMS_Server/IServerInterface/editTrack", ReplyAction="ATMS_Server/IServerInterface/editTrackResponse")]
        System.Threading.Tasks.Task editTrackAsync(ATMS_Model.Track t);
        
        [System.ServiceModel.OperationContractAttribute(Action="ATMS_Server/IServerInterface/createNewPlot", ReplyAction="ATMS_Server/IServerInterface/createNewPlotResponse")]
        void createNewPlot(ATMS_Model.Plot p);
        
        [System.ServiceModel.OperationContractAttribute(Action="ATMS_Server/IServerInterface/createNewPlot", ReplyAction="ATMS_Server/IServerInterface/createNewPlotResponse")]
        System.Threading.Tasks.Task createNewPlotAsync(ATMS_Model.Plot p);
        
        [System.ServiceModel.OperationContractAttribute(Action="ATMS_Server/IServerInterface/createNewWaypoint", ReplyAction="ATMS_Server/IServerInterface/createNewWaypointResponse")]
        void createNewWaypoint(ATMS_Model.Plot p, ATMS_Model.Plot oldPlot);
        
        [System.ServiceModel.OperationContractAttribute(Action="ATMS_Server/IServerInterface/createNewWaypoint", ReplyAction="ATMS_Server/IServerInterface/createNewWaypointResponse")]
        System.Threading.Tasks.Task createNewWaypointAsync(ATMS_Model.Plot p, ATMS_Model.Plot oldPlot);
        
        [System.ServiceModel.OperationContractAttribute(Action="ATMS_Server/IServerInterface/removePlot", ReplyAction="ATMS_Server/IServerInterface/removePlotResponse")]
        void removePlot(ATMS_Model.Plot p);
        
        [System.ServiceModel.OperationContractAttribute(Action="ATMS_Server/IServerInterface/removePlot", ReplyAction="ATMS_Server/IServerInterface/removePlotResponse")]
        System.Threading.Tasks.Task removePlotAsync(ATMS_Model.Plot p);
        
        [System.ServiceModel.OperationContractAttribute(Action="ATMS_Server/IServerInterface/editPlot", ReplyAction="ATMS_Server/IServerInterface/editPlotResponse")]
        void editPlot(ATMS_Model.Plot p);
        
        [System.ServiceModel.OperationContractAttribute(Action="ATMS_Server/IServerInterface/editPlot", ReplyAction="ATMS_Server/IServerInterface/editPlotResponse")]
        System.Threading.Tasks.Task editPlotAsync(ATMS_Model.Plot p);
        
        [System.ServiceModel.OperationContractAttribute(Action="ATMS_Server/IServerInterface/pauseSimulation", ReplyAction="ATMS_Server/IServerInterface/pauseSimulationResponse")]
        void pauseSimulation();
        
        [System.ServiceModel.OperationContractAttribute(Action="ATMS_Server/IServerInterface/pauseSimulation", ReplyAction="ATMS_Server/IServerInterface/pauseSimulationResponse")]
        System.Threading.Tasks.Task pauseSimulationAsync();
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IServerInterfaceCallback {
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="ATMS_Server/IServerInterface/notifyTimeUpdate")]
        void notifyTimeUpdate(int currentServerTime);
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="ATMS_Server/IServerInterface/notifyNewScenario")]
        void notifyNewScenario(ATMS_Model.Scenario data);
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="ATMS_Server/IServerInterface/notifyNewTrack")]
        void notifyNewTrack(ATMS_Model.Track t);
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="ATMS_Server/IServerInterface/notifyRemoveTrack")]
        void notifyRemoveTrack(ATMS_Model.Track t);
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="ATMS_Server/IServerInterface/notifyEditedTrack")]
        void notifyEditedTrack(ATMS_Model.Track t);
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="ATMS_Server/IServerInterface/notifyEditedPlot")]
        void notifyEditedPlot(ATMS_Model.Plot t);
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="ATMS_Server/IServerInterface/notifyNewPlot")]
        void notifyNewPlot(ATMS_Model.Plot t);
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="ATMS_Server/IServerInterface/notifyRemovePlot")]
        void notifyRemovePlot(ATMS_Model.Plot t);
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="ATMS_Server/IServerInterface/createNewAutoGeneratedPlots")]
        void createNewAutoGeneratedPlots(ATMS_Model.Plot[] p);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IServerInterfaceChannel : Model.ServiceReference1.IServerInterface, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class ServerInterfaceClient : System.ServiceModel.DuplexClientBase<Model.ServiceReference1.IServerInterface>, Model.ServiceReference1.IServerInterface {
        
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
        
        public void populateClient() {
            base.Channel.populateClient();
        }
        
        public System.Threading.Tasks.Task populateClientAsync() {
            return base.Channel.populateClientAsync();
        }
        
        public void createScenario() {
            base.Channel.createScenario();
        }
        
        public System.Threading.Tasks.Task createScenarioAsync() {
            return base.Channel.createScenarioAsync();
        }
        
        public void playSimulation() {
            base.Channel.playSimulation();
        }
        
        public System.Threading.Tasks.Task playSimulationAsync() {
            return base.Channel.playSimulationAsync();
        }
        
        public void createNewTrack(ATMS_Model.Track t) {
            base.Channel.createNewTrack(t);
        }
        
        public System.Threading.Tasks.Task createNewTrackAsync(ATMS_Model.Track t) {
            return base.Channel.createNewTrackAsync(t);
        }
        
        public void createNewTrackOnMap(ATMS_Model.Plot p) {
            base.Channel.createNewTrackOnMap(p);
        }
        
        public System.Threading.Tasks.Task createNewTrackOnMapAsync(ATMS_Model.Plot p) {
            return base.Channel.createNewTrackOnMapAsync(p);
        }
        
        public void removeTrack(ATMS_Model.Track t) {
            base.Channel.removeTrack(t);
        }
        
        public System.Threading.Tasks.Task removeTrackAsync(ATMS_Model.Track t) {
            return base.Channel.removeTrackAsync(t);
        }
        
        public void editTrack(ATMS_Model.Track t) {
            base.Channel.editTrack(t);
        }
        
        public System.Threading.Tasks.Task editTrackAsync(ATMS_Model.Track t) {
            return base.Channel.editTrackAsync(t);
        }
        
        public void createNewPlot(ATMS_Model.Plot p) {
            base.Channel.createNewPlot(p);
        }
        
        public System.Threading.Tasks.Task createNewPlotAsync(ATMS_Model.Plot p) {
            return base.Channel.createNewPlotAsync(p);
        }
        
        public void createNewWaypoint(ATMS_Model.Plot p, ATMS_Model.Plot oldPlot) {
            base.Channel.createNewWaypoint(p, oldPlot);
        }
        
        public System.Threading.Tasks.Task createNewWaypointAsync(ATMS_Model.Plot p, ATMS_Model.Plot oldPlot) {
            return base.Channel.createNewWaypointAsync(p, oldPlot);
        }
        
        public void removePlot(ATMS_Model.Plot p) {
            base.Channel.removePlot(p);
        }
        
        public System.Threading.Tasks.Task removePlotAsync(ATMS_Model.Plot p) {
            return base.Channel.removePlotAsync(p);
        }
        
        public void editPlot(ATMS_Model.Plot p) {
            base.Channel.editPlot(p);
        }
        
        public System.Threading.Tasks.Task editPlotAsync(ATMS_Model.Plot p) {
            return base.Channel.editPlotAsync(p);
        }
        
        public void pauseSimulation() {
            base.Channel.pauseSimulation();
        }
        
        public System.Threading.Tasks.Task pauseSimulationAsync() {
            return base.Channel.pauseSimulationAsync();
        }
    }
}
