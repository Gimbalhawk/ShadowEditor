using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShadowEditor.Code.Data
{
	// This represents a link to 
	class DataReference : DataObject
	{
		public Guid RefId { get; set; }

		public DataReference(DataObject link)
			: base()
		{
			RefId = link.Id;
			Name = link.Name;
		}
	}
}
