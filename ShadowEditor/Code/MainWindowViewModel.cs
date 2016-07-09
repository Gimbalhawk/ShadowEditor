using ShadowEditor.Code.Data;
using ShadowEditor.Code.Settings;
using ShadowEditor.Code.ViewModel;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ShadowEditor.Code
{
	class MainWindowViewModel : ViewModelBase
	{
		public MainWindowViewModel()
		{
			SettingsManager.Instance.InitializeSettings();
			DataManager.Instance.InitializeData();

			System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en-US");

			DataManager.Instance.OpenFilesChanged += OnOpenFilesChanged;
		}

		~MainWindowViewModel()
		{
			DataManager.Instance.OpenFilesChanged -= OnOpenFilesChanged;
		}

		void OnOpenFilesChanged(EventArgs e)
		{
			OnPropertyChanged("OpenDocuments");
		}

		public IEnumerable<DocumentViewModel> OpenDocuments { get { return DataManager.Instance.OpenFiles.Select(d => new DocumentViewModel(d)); } }

		public ICommand SaveCharacterCommand { get { return new SaveCommand(false); } }
		public ICommand SaveCharacterAsCommand { get { return new SaveCommand(true); } }

		public ICommand LoadCharacterCommand { get { return new LoadCommand(); } }
		public ICommand NewCharacterCommand { get { return new CreateCharacterCommand(); } }

		public ICommand UndoCommand { get { return new UndoChangeCommand(); } }
		public ICommand RedoCommand { get { return new RedoChangeCommand(); } }

		public ICommand OpenPreferencesCommand { get { return new OpenPreferencesCommand(); } }
	}

	class SaveCommand : CommandBase
	{
		bool m_requestNewFilename;

		public SaveCommand(bool requestNewName)
		{
			DataManager.Instance.ActiveFileChanged += OnOpenFilesChanged;

			m_requestNewFilename = requestNewName;
		}

		~SaveCommand()
		{
			DataManager.Instance.ActiveFileChanged -= OnOpenFilesChanged;
		}

		public void OnOpenFilesChanged(EventArgs e)
		{
			OnCanExecuteChanged(e);
		}

		public override bool CanExecute(object parameter)
		{
			return DataManager.Instance.ActiveDocument != null;
		}

		public override void Execute(object parameter)
		{
			Document activeDocument = DataManager.Instance.ActiveDocument;
			if (activeDocument != null)
			{
				string filename = activeDocument.Filename;

				if (String.IsNullOrEmpty(filename) || m_requestNewFilename)
				{
					string savePath = Path.Combine(System.IO.Directory.GetCurrentDirectory(), ConfigurationManager.AppSettings["SavePath"]);
					SaveFileDialog saveDialog = new SaveFileDialog();
					
					saveDialog.InitialDirectory = savePath;
					saveDialog.RestoreDirectory = true;
					saveDialog.DefaultExt = ".char";
					saveDialog.Filter = "Character files (.char)|*.char";

					bool? result = saveDialog.ShowDialog();
					if (result == true)
					{
						filename = saveDialog.FileName;
					}
					else
					{
						// Save was cancelled.
						return;
					}
				}

				// TODO: move filename into document. What was I thinking?
				if (DataManager.Instance.SaveDocument(activeDocument, filename))
				{
					activeDocument.Filename = filename;
				}
			}
		}
	}

	class LoadCommand : CommandBase
	{
		public override void Execute(object parameter)
		{
			OpenFileDialog loadDialog = new OpenFileDialog();
			loadDialog.InitialDirectory = Path.Combine(System.IO.Directory.GetCurrentDirectory(), ConfigurationManager.AppSettings["SavePath"]);
			loadDialog.RestoreDirectory = true;
			loadDialog.DefaultExt = ".char";
			loadDialog.Filter = "Character files (.char)|*.char";

			if (loadDialog.ShowDialog() == true)
			{
				DataManager.Instance.LoadCharacter(loadDialog.FileName);
			}
		}
	}

	class CreateCharacterCommand : CommandBase
	{
		public override void Execute(object parameter)
		{
			DataManager.Instance.CreateCharacter();
		}
	}

	class UndoChangeCommand : CommandBase
	{
		public override void Execute(object parameter)
		{
			DataManager.Instance.ActiveDocument?.UndoChange();
		}
	}

	class RedoChangeCommand : CommandBase
	{
		public override void Execute(object parameter)
		{
			DataManager.Instance.ActiveDocument?.RedoChange();
		}
	}

	class OpenPreferencesCommand : CommandBase
	{
		public override void Execute(object parameter)
		{
			
		}
	}
}
