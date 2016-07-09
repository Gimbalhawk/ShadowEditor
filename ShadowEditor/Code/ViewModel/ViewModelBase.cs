using ShadowEditor.Code.Data;
using ShadowEditor.Code.Data.Undo;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShadowEditor.Code.ViewModel
{
	public class ViewModelBase : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;

		public void OnPropertyChanged(string propertyName)
		{
			if (PropertyChanged != null)
			{
				PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
	}

	public class DataObjectViewModel<T> : ViewModelBase
		where T : DataObject
	{
		protected T Data { get; set; }

		public DataObjectViewModel(T data)
		{
			if (data != null)
			{
				Data = data;
				Data.PropertyChanged += HandleDataPropertyChanged;
			}
		}

		~DataObjectViewModel()
		{
			if (Data != null)
			{
				Data.PropertyChanged += HandleDataPropertyChanged;
			}
		}

		// Creates a property undo unit to modify the given property on our data
		protected void SetPropertyValue(string propertyName, object newValue)
		{
			if (Data.GetType().GetProperty(propertyName).GetValue(Data) != newValue)
			{
				DataManager.Instance.ActiveDocument.AddChange(new PropertyUndoUnit(Data, propertyName, newValue));
			}
		}

		protected virtual void HandleDataPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			OnPropertyChanged(e.PropertyName);
		}
	}
}
