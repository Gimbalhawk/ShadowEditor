using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShadowEditor.Code.Data
{
	public class Metatype : DataObject
	{
		public AttributeList BaseAttributes { get; set; } = new AttributeList();
	}

	public class MetatypeCategory : DataObject
	{
		public List<Metatype> Metavariants { get; set; } = new List<Metatype>();
	}
}
