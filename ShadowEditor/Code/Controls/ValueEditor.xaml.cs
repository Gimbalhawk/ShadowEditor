using ShadowEditor.Code.Data;
using ShadowEditor.Code.Data.Undo;
using ShadowEditor.Code.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
	/// Interaction logic for ValueEditor.xaml
	/// </summary>
	public partial class ValueEditor : UserControl
	{
		private int m_currentValue;
		private ValueEditorViewModel m_viewModel;

		public ValueEditor()
		{
			InitializeComponent();
		}

		private bool ValidateText(string text)
		{
			// Ensure that the given text is an integer
			int newValue = 0;
			return int.TryParse(text, out newValue);
		}

		private void ValidateTextChanged(object sender, TextChangedEventArgs e)
		{
			TextBox textBox = sender as TextBox;

			int newValue = 0;
			if (int.TryParse(textBox.Text, out newValue))
			{
				// Our new value is an integer, so cache it
				m_currentValue = newValue;
			}
			else
			{
				// If the new value isn't an integer, cancel the change.
				textBox.Text = m_currentValue.ToString();
			}
		}

		// Validate changes before they're made, which lets us prevent non-numeric characters from being entered
		private void ValidateTextInput(object sender, TextCompositionEventArgs e)
		{
			TextBox textBox = sender as TextBox;

			int newValue = 0;
			if (!int.TryParse(textBox.Text, out newValue))
			{
				e.Handled = true;
			}
		}

		// Commit our changes
		private void ValueTextBoxLostKeyboardFocus(object sender, RoutedEventArgs e)
		{
			if (m_viewModel == null)
				return;

			m_viewModel.Value = m_currentValue;
		}

		private void ValueTextBoxDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
		{
			var viewModel = e.NewValue as ValueEditorViewModel;
			if (viewModel != null)
			{
				m_viewModel = viewModel;
			}
		}
	}
}
