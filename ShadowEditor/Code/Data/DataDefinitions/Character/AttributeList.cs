using ShadowEditor.Code.Debug;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShadowEditor.Code.Data
{
	public class AttributeList : DataObject
	{
		// Physical
		public Attribute Body      { get; private set; }
		public Attribute Agility   { get; private set; }
		public Attribute Reaction  { get; private set; }
		public Attribute Strength  { get; private set; }

		// Mental
		public Attribute Willpower { get; private set; }
		public Attribute Logic     { get; private set; }
		public Attribute Intuition { get; private set; }
		public Attribute Charisma  { get; private set; }

		// Special
		public Attribute Magic     { get; private set; }
		public Attribute Resonance { get; private set; }
		public Attribute Essence   { get; private set; }

		protected override void OnInit()
		{
			base.OnInit();

			Body      = new Attribute();
			Agility   = new Attribute();
			Reaction  = new Attribute();
			Strength  = new Attribute();

			Willpower = new Attribute();
			Logic     = new Attribute();
			Intuition = new Attribute();
			Charisma  = new Attribute();

			Essence   = new Attribute();
		}

		public void ChangeAttributes(AttributeList newAttributes)
		{
			foreach (Attribute newAttribute in newAttributes.GetProperties<Attribute>())
			{
				var property = GetType().GetProperty(newAttribute.Name);
				if (property == null)
				{
					Log.Instance.WriteLine(String.Format("Trying to set attribute that doesn't exist: {0}. Is the data formatted wrong?", newAttribute.Name));
					continue;
				}

				// Make sure that we have an instance of the new attribute
				Attribute attribute = property.GetValue(this) as Attribute;
				if (attribute == null)
				{
					attribute = new Attribute();
					attribute.Name = newAttribute.Name;
					property.SetValue(this, attribute);
				}
			}
		}
	}
}
