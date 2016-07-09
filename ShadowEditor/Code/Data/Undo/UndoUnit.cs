using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShadowEditor.Code.Data.Undo
{
	// This is the base class for undo units.
	public abstract class UndoUnit
	{
		public abstract void Execute();
		
		public abstract void Undo();
	}
}
