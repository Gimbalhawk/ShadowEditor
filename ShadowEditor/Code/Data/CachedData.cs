using ShadowEditor.Code.Debug;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShadowEditor.Code.Data
{
	public class CachedData
	{
		private Dictionary<Guid, DataObject> Cache { get; set; } = new Dictionary<Guid, DataObject>();

		#region Individual data storage
		public List<MetatypeCategory> Metatypes { get; private set; } = new List<MetatypeCategory>();
		#endregion

		public void RegisterData(DataObject data)
		{
			if (data == null)
				return;

			if (Cache.ContainsKey(data.Id))
				System.Windows.MessageBox.Show(String.Format("Guid {0} already exists in data cache!", data.Id));

			Cache.Add(data.Id, data);

			// TODO: Better solution for this
			if (data is MetatypeCategory)
				Metatypes.Add(data as MetatypeCategory);
		}

		public T GetInstance<T>(Guid id) where T : DataObject
		{
			return GetInstance(id) as T;
		}

		public DataObject GetInstance(Guid id)
		{
			DataObject data = null;
			bool findResult = Cache.TryGetValue(id, out data);
			if (!findResult)
			{
				Log.Instance.WriteLine(String.Format("Can't find data with id: {0}", id));
			}

			return data;
		}

		public void ClearCache()
		{
			Cache.Clear();
		}
	}
}
