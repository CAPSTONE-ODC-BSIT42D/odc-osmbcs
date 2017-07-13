using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace prototype2
{
    public class Commands
    {
        private ICommand _saveEmployee;
        public ICommand SaveEmployeeCommand
        {
            get
            {
                if (_saveEmployee == null)
                    _saveEmployee = new SaveEmployee();
                return _saveEmployee;
            }
            set
            {
                _saveEmployee = value;
            }
        }
    }

    class SaveEmployee : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            throw new NotImplementedException();
        }
    }

}
