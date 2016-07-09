using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShadowEditor.Code.Data.Undo
{
	// Class used to store a batch of undo units that are executed or undone as one.
	class UndoUnitContainer : UndoUnit
	{
		List<UndoUnit> m_undoUnits = new List<UndoUnit>();

		public void Add(UndoUnit unit)
		{
			m_undoUnits.Add(unit);
		}

		public override void Execute()
		{
			foreach (UndoUnit unit in m_undoUnits)
			{
				unit.Execute();
			}
		}

		public override void Undo()
		{
			foreach (UndoUnit unit in m_undoUnits)
			{
				unit.Undo();
			}
		}
	}
}
