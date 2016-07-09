using ShadowEditor.Code.Data.Serialization;
using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ShadowEditor.Code.Data
{
	class DataManager
	{
		public static DataManager Instance = new DataManager();

		#region Members

		private List<Document> m_openFiles = new List<Document>();
		public List<Document> OpenFiles { get { return m_openFiles; } }

		private Document m_activeDocument;
		public Document ActiveDocument
		{
			get { return m_activeDocument; }
			private set
			{
				if (m_activeDocument != value)
				{
					m_activeDocument = value;
					ActiveFileChanged(new EventArgs());
				}
			}
		}

		#endregion

		#region Events

		public event ActiveFileChangedHandler ActiveFileChanged;
		public delegate void ActiveFileChangedHandler(EventArgs e);

		public event OpenFilesChangedHandler OpenFilesChanged;
		public delegate void OpenFilesChangedHandler(EventArgs e);

		#endregion
		
		public void InitializeData()
		{
			LoadDataFilesAsync();
		}

		public bool LoadDataFilesAsync()
		{
			// TODO: Load the data files the editor will use.
			string dataPath = ConfigurationManager.AppSettings["DataPath"];

			return false;
		}

		public void LoadCharacter(string filename)
		{
			Character character = FileReader.LoadFile(filename) as Character;
			if (character != null)
			{
				OpenNewDocument(character);
			}
		}

		public void CreateCharacter()
		{
			OpenNewDocument(new Character());
		}

		public void OpenNewDocument(DataObject root)
		{
			ActiveDocument = new Document(root);
			m_openFiles.Add(ActiveDocument);

			OpenFilesChanged(new EventArgs());
		}

		public void SwitchToDocument(Document document)
		{
			if (OpenFiles.Contains(document))
			{
				ActiveDocument = document;
			}
		}

		#region File Saving

		public bool SaveDocument(Document document, string filename)
		{
			return SaveFile(document.Root, filename);
		}

		public bool SaveFile(DataObject data, string filename)
		{
			if (!Directory.Exists(Path.GetDirectoryName(filename)))
			{
				Directory.CreateDirectory(Path.GetDirectoryName(filename));
			}

			return FileWriter.WriteDataFile(data, filename);
		}

		#endregion
	}
}
