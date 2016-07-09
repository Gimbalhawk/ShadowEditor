using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShadowEditor.Code.Data
{
	class Skill : ManagedValue
	{
		[DefaultValue(0)]
		override public int Min { get; set; }
		[DefaultValue(12)]
		override public int Max { get; set; }

		public override int GetAugmentedMaximum()
		{
			return (int)Math.Ceiling(Value * 1.5);
		}
	}
}
