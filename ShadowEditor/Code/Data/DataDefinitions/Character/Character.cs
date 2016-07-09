using ShadowEditor.Code.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShadowEditor.Code.Data
{
	public class Character : DataObject
	{
		public AttributeList Attributes { get; set; } = new AttributeList();
		public Metatype Metatype { get; set; }
		public CharacterCreationSettings CharacterCreationSettings { get; set; }

		protected override void OnInit()
		{
			base.OnInit();
		}
	}
}
