using ShadowEditor.Code.Debug;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShadowEditor.Code.Data.Undo
{
	public class UndoStack
	{
		#region Events

		public enum UndoActionType
		{
			UnitAdded,
			Undo,
			Redo
		}

		public class UndoEventArgs : EventArgs
		{
			UndoActionType Type;

			public UndoEventArgs(UndoActionType type)
				: base()
			{
				Type = type;
			}
		}

		public event UndoStackChangedHandler UndoStackChanged;
		public delegate void UndoStackChangedHandler(UndoEventArgs e);

		#endregion

		// The change that was at the top of the undo stack when the last save happened.
		private UndoUnit m_lastSavedChange;

		// Changes that have been executed that we can undo
		private Stack<UndoUnit> UndoUnits { get; set; }

		// Changes that have been undone that we can execute again
		private Stack<UndoUnit> RedoUnits { get; set; }

		public UndoStack()
		{
			UndoUnits = new Stack<UndoUnit>();
			RedoUnits = new Stack<UndoUnit>();
		}
		
		/// <summary>
		/// Adds a new undo unit to the undo stack and executes it.
		/// </summary>
		/// <param name="unit">The new unit to add</param>
		public void Push(UndoUnit unit)
		{
			try
			{
				// TODO: Limit size of the stack
				unit.Execute();
				UndoUnits.Push(unit);

				// A new change invalidates any undone changes
				RedoUnits.Clear();

				if (UndoStackChanged != null)
					UndoStackChanged(new UndoEventArgs(UndoActionType.UnitAdded));
			}
			catch (NullReferenceException e)
			{
				Log.Instance.WriteLine(e.StackTrace);
			}
		}

		// Undoes the most recently done change and adds it to the redo stack
		public void Undo()
		{
			if (!UndoUnits.Any())
			{
				return;
			}

			UndoUnit unit = UndoUnits.Pop();
			if (unit != null)
			{
				unit.Undo();
				RedoUnits.Push(unit);

				if (UndoStackChanged != null)
					UndoStackChanged(new UndoEventArgs(UndoActionType.Undo));
			}
		}

		// Executes the most recently undone change and returns it to the undo stacxk
		public void Redo()
		{
			if (!RedoUnits.Any())
			{
				return;
			}

			UndoUnit unit = RedoUnits.Pop();
			if (unit != null)
			{
				unit.Execute();
				UndoUnits.Push(unit);

				if (UndoStackChanged != null)
					UndoStackChanged(new UndoEventArgs(UndoActionType.Redo));
			}
		}

		// Check to see if there's any changes that need to be saved by comparing the most recent
		// change to the one that was most recent the last time we saved.
		public bool HasUnsavedChanges()
		{
			UndoUnit lastChange = UndoUnits.FirstOrDefault();

			return lastChange != m_lastSavedChange;
		}

		public void OnSaved()
		{
			m_lastSavedChange = UndoUnits.FirstOrDefault();
		}
	}
}
