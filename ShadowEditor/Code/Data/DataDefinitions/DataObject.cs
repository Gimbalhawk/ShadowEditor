using ShadowEditor.Code.Data.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ShadowEditor.Code.Data
{
	/// <summary>
	/// This is the base class for anything that will be stored in an xml file.
	/// </summary>
	public class DataObject : INotifyPropertyChanged
	{
		#region Constructors

		public DataObject()
			: this(Guid.NewGuid())
		{
		}

		public DataObject(Guid guid)
		{
			Id = guid;
			//Name = GetType().Name; //Debug

			Init();
		}

		#endregion

		#region Functions

		private void Init()
		{
			// Set all of our properties to their default value if one is specified
			foreach (var property in GetType().GetProperties())
			{
				var attr = property.GetCustomAttribute<DefaultValueAttribute>();
				if (attr != null)
				{
					property.SetValue(this, attr.Value);
				}
			}

			OnInit();

			// Once we're initialized, set any unset property names to the property's name
			foreach (var property in GetType().GetProperties())
			{
				DataObject value = property.GetValue(this) as DataObject;
				if (value != null && String.IsNullOrEmpty(value.Name))
				{
					value.Name = property.Name;
				}
			}
		}

		protected virtual void OnInit()
		{
		}

		// Return the value of all properties of type T
		public IEnumerable<T> GetProperties<T>()
		{
			return GetType().GetProperties().Where(p => p.PropertyType == typeof(T) && p.GetValue(this) != null).Select(p => (T)(p.GetValue(this)));
		}

		#endregion

		#region PropertyChanged

		public event PropertyChangedEventHandler PropertyChanged;

		public void OnPropertyChanged(string propertyName)
		{
			if (PropertyChanged != null)
			{
				PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		#endregion

		#region Properties

		[SerializedOrder(1)]
		public Guid Id { get; set; }

		// Default value is the name of the property that the DataObject represents
		[SerializedOrder(2)]
		public string Name { get; set; }

		#endregion

		#region Data Serialization

		public virtual IEnumerable<PropertyInfo> GetSerializableProperties()
		{
			// Find all the properties that aren't specifically marked as non-serialized
			List<PropertyInfo> list = GetType().GetProperties()
				.Where(p => p.GetCustomAttribute<NonSerializedPropertyAttribute>() == null)
				.ToList();

			// Find any properties that specify sort order. This lets us put certain properties first while leaving the rest unchanged.
			list.Sort(delegate (PropertyInfo a, PropertyInfo b)
			{
				var aOrder = a.GetCustomAttribute<SerializedOrderAttribute>();
				var bOrder = b.GetCustomAttribute<SerializedOrderAttribute>();

				if (aOrder == null && bOrder == null) return 0;
				else if (aOrder == null) return 1;
				else if (bOrder == null) return -1;
				else return aOrder.Order.CompareTo(bOrder.Order);
			});

			return list;
		}

		#endregion
	}
}
