using ShadowEditor.Code.Data.Undo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShadowEditor.Code.Data
{
	// Represents a single open file
	public class Document
	{
		#region Members

		// The name of the file the document was loaded from, if any
		[NonSerializedProperty]
		public string Filename { get; set; }

		// The data the document represents
		public DataObject Root { get; set; }

		#endregion

		private UndoStack UndoStack { get; set; }

		public Document(DataObject root)
		{
			Root = root;
			UndoStack = new UndoStack();
		}

		public void UndoChange()
		{
			UndoStack.Undo();
		}

		public void RedoChange()
		{
			UndoStack.Redo();
		}

		public void AddChange(UndoUnit unit)
		{
			UndoStack.Push(unit);
		}

		public bool IsDirty()
		{
			return UndoStack.HasUnsavedChanges();
		}
	}
}
