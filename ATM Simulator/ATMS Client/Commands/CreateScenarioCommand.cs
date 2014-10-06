using ATMS_Client.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ATMS_Client.Commands
{
    class CreateScenarioCommand : ICommand
    {
        ViewModel viewModel;

        public CreateScenarioCommand(ViewModel vm)
        {
            this.viewModel = vm;
        }
        public bool CanExecute(object parameter)
        {
            return viewModel.serverAvailable;
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public void Execute(object parameter)
        {
            viewModel.newScenario();
        }
    }
}
