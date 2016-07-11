using ShadowEditor.Code.Data;
using ShadowEditor.Code.Debug;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ShadowEditor.Code.ViewModel
{
	public class DocumentViewModel : ViewModelBase
	{
		private Document m_document;

		public string DocumentName { get { return m_document?.Root?.Name ?? "Unknown Document"; } }
		public CharacterViewModel Character { get { return new CharacterViewModel(m_document?.Root as Character); } }

		public DocumentViewModel(Document document)
		{
			if (document == null)
			{
				Log.Instance.WriteLine("Creating DocumentViewModel for null document. What happened?");
			}

			m_document = document;
		}

		public void MakeActiveDocument()
		{
			DataManager.Instance.SwitchToDocument(m_document);
		}
	}
}
