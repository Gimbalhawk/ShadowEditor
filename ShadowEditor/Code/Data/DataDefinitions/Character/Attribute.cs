using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShadowEditor.Code.Data
{
	/// <summary>
	/// Represents an attribute on a character, vehicle or piece of gear.
	/// </summary>
	public class Attribute : ManagedValue
	{
		[DefaultValue(1)]
		override public int Min { get; set; }
		[DefaultValue(6)]
		override public int Max { get; set; }

		public static void ChangeAttributeBaseline(Attribute currentValue, Attribute newValue)
		{
			// Change the minimum and maximum values of the attribute. We should also change our current value
			// so that it varies from the minimum by the same amount.
			int valueDiffersFromMin = currentValue.Value - currentValue.Min;
			currentValue.Min = newValue.Min;
			currentValue.Max = newValue.Max;
			currentValue.Value = currentValue.Min + valueDiffersFromMin;
		}

		public override int GetAugmentedMaximum()
		{
			// TODO: Where should this magic number go?
			return Value + 4;
		}
	}
}
