using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ShadowEditor.Code.ViewModel
{
	abstract class CommandBase : ICommand
	{
		public event EventHandler CanExecuteChanged;

		public virtual bool CanExecute(object parameter)
		{
			return true;
		}

		public virtual void Execute(object parameter)
		{
			throw new NotImplementedException();
		}

		public virtual void OnCanExecuteChanged(EventArgs e)
		{
			if (CanExecuteChanged != null)
			{
				CanExecuteChanged(this, e);
			}
		}
	}
}
