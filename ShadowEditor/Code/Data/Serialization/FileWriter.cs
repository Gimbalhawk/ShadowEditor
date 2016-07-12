using ShadowEditor.Code.Data.Attributes;
using ShadowEditor.Code.Debug;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace ShadowEditor.Code.Data.Serialization
{
	class FileWriter
	{
		/// <summary>
		/// Writes a new XML file that represents the given DataObject.
		/// </summary>
		/// <returns>True if the write was a success</returns>
		static public bool WriteDataFile(DataObject root, string filename)
		{
			if (root == null)
				return false;

			bool succeeded = true;
			try
			{
				XmlWriterSettings settings = new XmlWriterSettings();
				settings.Indent = true;
				settings.IndentChars = "\t";

				using (var writer = XmlWriter.Create(filename, settings))
				{
					writer.WriteStartDocument();
					WriteValue(root, "Root", writer);
					writer.WriteEndDocument();
				}
			}
			catch (Exception e)
			{
				Log.Instance.WriteLine("Exception caught trying to write file!");
				Log.Instance.WriteLine(e.StackTrace);
				succeeded = false;
			}

			return succeeded;
		}

		static private void WriteValue(Object value, string name, XmlWriter writer)
		{
			if (value != null)
			{
				if (value is DataObject)
				{
					DataObject data = value as DataObject;

					// For DataObjects we write all the properties individually.
					writer.WriteStartElement(name);
					writer.WriteAttributeString("Type", data.GetType().FullName);

					foreach (PropertyInfo property in data.GetSerializableProperties())
					{
						Object propertyValue = property.GetValue(data);

						// There's no point writing properties that have their default value. This should make the file less cluttered.
						DefaultValueAttribute valueAttribute = property.GetCustomAttribute<DefaultValueAttribute>();
						if (valueAttribute != null && valueAttribute.Value.Equals(propertyValue))
							continue;
						
						WriteValue(propertyValue, property.Name, writer);
					}

					writer.WriteEndElement();
				}
				else if (value is String)
				{
					writer.WriteElementString(name, value as String);
				}
				else if (value is IEnumerable)
				{
					// For lists, iterate through the elements and write each independantly.
					writer.WriteStartElement(name);

					foreach (Object entry in (value as IEnumerable))
					{
						WriteValue(entry, entry.GetType().Name, writer);
					}

					writer.WriteEndElement();
				}
				else if (value.GetType().IsValueType)
				{
					// Basic types should simply be converted to string and writen to the file.
					writer.WriteElementString(name, value.ToString());
				}
				else
				{
					Log.Instance.WriteLine("Attempting to write unsupported type: " + value.GetType().Name);
				}
			}
		}
	}
}
