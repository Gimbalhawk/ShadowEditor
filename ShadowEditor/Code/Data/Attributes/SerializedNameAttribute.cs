using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShadowEditor.Code.Data.Attributes
{
	class SerializedNameAttribute : System.Attribute
	{
		public string Name { get; set; }

		public SerializedNameAttribute(string name)
		{
			Name = name;
		}
	}
}
