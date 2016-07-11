using ShadowEditor.Code.Debug;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml;

namespace ShadowEditor.Code.Data.Serialization
{
	class FileReader
	{
		public static DataObject LoadFile(string filename)
		{
			DataObject data = null;

			try
			{
				Stopwatch stopwatch = Stopwatch.StartNew();

				XmlDocument document = new XmlDocument();
				document.Load(filename);

				XmlNode root = document.SelectSingleNode("Root");

				if (root != null)
				{
					data = ReadDataObject(root);
				}

				stopwatch.Stop();
				Log.Instance.WriteLine(String.Format("Loaded file {0} in {1} ms", filename, stopwatch.ElapsedMilliseconds));
			}
			catch (FileNotFoundException)
			{
				MessageBox.Show(String.Format("Could not find file {0}!", filename));
			}
			catch (Exception e)
			{
				Log.Instance.WriteLine(String.Format("Exception caught trying to read file {0}!", filename));
				Log.Instance.WriteLine(e.StackTrace);

				MessageBox.Show(String.Format("Error trying to read file {0}, see the log for details.", filename));

				return null;
			}

			return data;
		}

		private static object ReadNode(XmlNode node, Type type)
		{
			object value = null;

			if (typeof(DataObject).IsAssignableFrom(type))
			{
				value = ReadDataObject(node);
			}
			else if (typeof(String).IsAssignableFrom(type))
			{
				value = node.InnerText;
			}
			else if (typeof(IEnumerable).IsAssignableFrom(type))
			{
				value = ReadIEnumerable(node, type);
			}
			else if (type.IsValueType)
			{
				TypeConverter converter = TypeDescriptor.GetConverter(type);
				value = (converter.ConvertFromInvariantString(node.InnerText));
			}

			return value;
		}

		private static DataObject ReadDataObject(XmlNode node)
		{
			string typename = node.Attributes["Type"].Value;
			if (typename == null)
				return null;

			Type type = Type.GetType(typename, true, true);
			if (type == null)
				return null;

			DataObject obj = Activator.CreateInstance(type) as DataObject;
			if (obj != null)
			{
				foreach (XmlNode child in node.ChildNodes)
				{
					PropertyInfo childProperty = type.GetProperty(child.Name);
					if (childProperty != null)
					{
						Type propertyType = childProperty.PropertyType;
						object value = ReadNode(child, propertyType);
						if (value != null)
						{
							childProperty.SetValue(obj, value);
						}
					}
					else
					{
						// The proper Id may not have been read yet, so try to read it manually
						Guid guid = new Guid();
						XmlNode idNode = node.SelectSingleNode("Id");
						if (idNode != null)
						{
							if (idNode.InnerText != null)
							{
								guid = Guid.Parse(idNode.InnerText);
								break;
							}
						}

						Log.Instance.WriteLine(String.Format("Can't find property {0} on DataObject {1}", child.Name, guid));
					}
				}
			}

			return obj;
		}

		private static IEnumerable ReadIEnumerable(XmlNode node, Type type)
		{
			IEnumerable collection = Activator.CreateInstance(type) as IEnumerable;
			Type typeArgument = type.GetGenericArguments()[0];

			foreach (XmlNode childNode in node.ChildNodes)
			{
				object nodeValue = ReadNode(childNode, typeArgument);
				if (nodeValue != null)
				{
					type.GetMethod("Add").Invoke(collection, new[] { nodeValue });
				}
			}

			return collection;
		}
	}
}
