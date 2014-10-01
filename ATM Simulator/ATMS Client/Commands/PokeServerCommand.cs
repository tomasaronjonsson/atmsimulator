using ATMS_Client.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ATMS_Client.Commands
{
    class PokeServerCommand : ICommand
    {
        ViewModel vm;

        public PokeServerCommand(ViewModel vm)
        {
            this.vm = vm;
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
            vm.Poke();
            vm.Register();
        }
    }
}
