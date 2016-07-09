using ShadowEditor.Code.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ShadowEditor.Code.ViewModel
{
	public class ValueEditorViewModel : DataObjectViewModel<ManagedValue>
	{
		public ValueEditorViewModel(ManagedValue value)
			: base(value)
		{
		}

		public string Name { get { return Data.Name; } }

		public int Value
		{
			get { return Data.Value; }
			set
			{
				SetValue(value);
			}
		}

		private void SetValue(int value)
		{
			if (Data.WillValueDiffer(value))
			{
				SetPropertyValue("Value", value);
			}
		}

		public RelayCommand IncrementValueCommand { get { return new RelayCommand(param => SetValue(Data.Value + 1)); } }
		public RelayCommand DecrementValueCommand { get { return new RelayCommand(param => SetValue(Data.Value - 1)); } }
	}
}
