using ATMS_Client.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ATMS_Client.Commands
{
    class PokeServerCommand : ICommand
    {
        ViewModel viewModel;

        public PokeServerCommand(ViewModel vm)
        {
            this.viewModel = vm;
        }
        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public void Execute(object parameter)
        {
            ThreadPool.QueueUserWorkItem(delegate(object state)
            {
                viewModel.Poke();
                viewModel.Register();
            });

        }
    }
}
