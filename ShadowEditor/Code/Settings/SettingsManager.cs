using ShadowEditor.Code.Debug;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace ShadowEditor.Code.Settings
{
	public class SettingsManager
	{
		public static SettingsManager Instance = new SettingsManager();

		public void InitializeSettings()
		{
			LoadBrushSetting("ControlBrushVeryDark");
			LoadBrushSetting("ControlBrushDark");
			LoadBrushSetting("ControlBrush");
			LoadBrushSetting("ControlBrushLight");
			LoadBrushSetting("ControlBrushVeryLight");

			LoadBrushSetting("TextBrushDark");
			LoadBrushSetting("TextBrush");
			LoadBrushSetting("TextBrushLight");
			LoadBrushSetting("TextBrushVeryLight");

			Properties.Settings.Default.Save();
		}

		public void SetSettingValue(string name, object value)
		{

		}

		private void LoadBrushSetting(string name)
		{
			try
			{
				// Settings are stored as System.Drawing.Colors, but our brushes are from System.Windows.Media. Sickeningly, there isn't a good way to convert between the two.
				var settingsColor = (System.Drawing.Color)Properties.Settings.Default[name];
				var color = System.Windows.Media.Color.FromArgb(settingsColor.A, settingsColor.R, settingsColor.G, settingsColor.B);
				Application.Current.Resources[name] = new SolidColorBrush(color);
			}
			catch(System.Configuration.SettingsPropertyNotFoundException)
			{
				Log.Instance.WriteLine(String.Format("Couldn't find property named {0}", name));
			}
		}
	}
}
