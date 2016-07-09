using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShadowEditor.Code.Data.Attributes
{
	// Used to sort properties when serialization time comes around. 
	// Properties with this attribute will appear before properties 
	// without it. If both have it, lower Order values have priority.
	class SerializedOrderAttribute : System.Attribute
	{
		public int Order { get; set; }

		public SerializedOrderAttribute(int order)
		{
			Order = order;
		}
	}
}
