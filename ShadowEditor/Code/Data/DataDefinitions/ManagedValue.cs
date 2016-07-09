using ShadowEditor.Code.Data.Undo;
using ShadowEditor.Code.Util;
using ShadowEditor.Code.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ShadowEditor.Code.Data
{
	abstract public class ManagedValue : DataObject
	{
		protected override void OnInit()
		{
			base.OnInit();
			Value = Min;
		}

		private int m_value;
		public int Value
		{
			get { return m_value; }
			set
			{
				// Ensure that our new attribute value is within our allowed limits
				m_value = MathUtil.Clamp<int>(value, Min, Max);
			}
		}

		// Negative min/max values represent an attribute with no min/max
		abstract public int Min { get; set; }
		abstract public int Max { get; set; }

		public bool WillValueDiffer(int value)
		{
			return MathUtil.Clamp<int>(value, Min, Max) != m_value;
		}

		/// <summary>
		/// Finds the maximum allowed augmented value based on the current actual value
		/// </summary>
		/// <returns></returns>
		public virtual int GetAugmentedMaximum()
		{
			return Value;
		}
	}
}
