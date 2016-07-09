using ShadowEditor.Code.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShadowEditor.Code.ViewModel
{
	public class CharacterViewModel : DataObjectViewModel<Character>
	{
		public IEnumerable<ValueEditorViewModel> Attributes { get { return Data.Attributes.GetProperties<Data.Attribute>().Select(a => new ValueEditorViewModel(a)); } }

		public CharacterViewModel(Character character)
			: base(character)
		{
		}
	}
}
