using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ShadowEditor.Code.Data.Undo
{
	public class PropertyUndoUnit : UndoUnit
	{
		DataObject m_dataObject;
		PropertyInfo m_property;

		Object m_oldValue;
		Object m_newValue;

		public PropertyUndoUnit(DataObject dataObject, String property, Object value)
		{
			m_dataObject = dataObject;
			m_property = dataObject.GetType().GetProperty(property);
			m_oldValue = m_property.GetValue(dataObject);
			m_newValue = value;
		}

		public override void Execute()
		{
			if (m_property.GetValue(m_dataObject) != m_newValue)
			{
				m_property.SetValue(m_dataObject, m_newValue);
				m_dataObject.OnPropertyChanged(m_property.Name);
			}
		}

		public override void Undo()
		{
			if (m_property.GetValue(m_dataObject) != m_oldValue)
			{
				m_property.SetValue(m_dataObject, m_oldValue);
				m_dataObject.OnPropertyChanged(m_property.Name);
			}
		}
	}
}
