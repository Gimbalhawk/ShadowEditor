using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShadowEditor.Code.Data
{
	[AttributeUsage(AttributeTargets.Property)]
	public class NonSerializedPropertyAttribute : System.Attribute
	{
	}
}
