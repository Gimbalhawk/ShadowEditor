using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShadowEditor.Code.Data
{
	public enum CreationType
	{
		Priority,
		SumToTen,
		Karma,
		LifeModules
	}

	public class CharacterCreationSettings : DataObject
	{
		CreationType CreationType { get; set; }
		bool IsInCharacterCreation { get; set; }
	}
}
