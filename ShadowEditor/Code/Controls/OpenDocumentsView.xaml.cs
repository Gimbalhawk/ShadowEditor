using ShadowEditor.Code.Data;
using ShadowEditor.Code.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ShadowEditor.Code.Controls
{
	/// <summary>
	/// Interaction logic for OpenDocumentsView.xaml
	/// </summary>
	public partial class OpenDocumentsView : UserControl
	{
		public OpenDocumentsView()
		{
			InitializeComponent();

			DataManager.Instance.OpenFilesChanged += OpenFilesChanged;
		}

		~OpenDocumentsView()
		{
			DataManager.Instance.OpenFilesChanged += OpenFilesChanged;
		}

		private void TabControlSelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			TabControl tabControl = sender as TabControl;
			if (tabControl != null)
				return;

			var selection = (sender as TabControl).SelectedItem as DocumentViewModel;
			if (selection != null)
			{
				selection.MakeActiveDocument();
			}

			// Remove focus from whatever has it, commiting any changes.
			tabControl.Focus();
		}

		private void OpenFilesChanged(EventArgs e)
		{
			Document activeDocument = DataManager.Instance.ActiveDocument;
			if (activeDocument != null)
			{
				DocumentTabControl.SelectedIndex = DataManager.Instance.OpenFiles.IndexOf(activeDocument);
			}
			else
			{
				// Needed??
				DocumentTabControl.SelectedItem = null;
			}
		}
	}
}
