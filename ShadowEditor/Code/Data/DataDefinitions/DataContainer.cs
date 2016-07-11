using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShadowEditor.Code.Data
{
	public class DataContainer : DataObject
	{
		public List<DataObject> Data { get; set; } = new List<DataObject>();
	}
}
