using ShadowEditor.Code.Data.Serialization;
using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using ShadowEditor.Code.Debug;
using System.Diagnostics;

namespace ShadowEditor.Code.Data
{
	class DataManager
	{
		public static DataManager Instance = new DataManager();

		#region Members

		public List<Document> OpenFiles { get; private set; }

		public CachedData CachedData { get; private set; }

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

		#region Constructor

		private DataManager()
		{
			OpenFiles = new List<Document>();

			CachedData = new CachedData();
		}

		#endregion

		public void InitializeData()
		{
			LoadDataFiles();

			DataContainer metatypes = new DataContainer() { Name = "Metatypes" };

			MetatypeCategory human = new MetatypeCategory() { Name = "Human" };
			human.Metavariants.Add(new Metatype() { Name = "Human" });
			metatypes.Data.Add(human);

			MetatypeCategory troll = new MetatypeCategory() { Name = "Troll" };
			troll.Metavariants.Add(new Metatype() { Name = "Troll" });
			metatypes.Data.Add(troll);

			SaveFile(metatypes, Path.Combine(System.IO.Directory.GetCurrentDirectory(), "Metatypes.xml"));
		}

		public bool LoadDataFiles()
		{
			string dataPath = Path.Combine(System.IO.Directory.GetCurrentDirectory(), ConfigurationManager.AppSettings["DataPath"]);

			try
			{
				Stopwatch stopwatch = Stopwatch.StartNew();

				var dataFiles = Directory.GetFiles(dataPath);

				// For each data file, pull out the root DataContainer and register each DataObject inside
				CachedData.ClearCache();
				dataFiles
					.Select(file => FileReader.LoadFile(file))
					.OfType<DataContainer>()                  
					.ToList()
					.ForEach(container => container.Data.ToList().ForEach(data => CachedData.RegisterData(data)));

				stopwatch.Stop();
				Log.Instance.WriteLine(String.Format("Read {0} data files in {1} ms", dataFiles.Count(), stopwatch.ElapsedMilliseconds));
			}
			catch (DirectoryNotFoundException)
			{
				Log.Instance.WriteLine(String.Format("Couldn't open data folder: {0}", dataPath));
				return false;
			}

			return true;
		}

		public void LoadData(string filename)
		{
			DataContainer container = FileReader.LoadFile(filename) as DataContainer;
			container?.Data.ForEach(d => CachedData.RegisterData(d));
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
			OpenFiles.Add(ActiveDocument);

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
