using ShadowEditor.Code.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Linq.Dynamic;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ShadowEditor.Code.Debug;

namespace ShadowEditor.Code.Util
{
	public static class FormulaParser
	{
		/// <summary>
		/// Tries to parse the given formula and convert it into a series of arithmetic operations. 
		/// Can pull properties off of 
		/// </summary>
		/// <param name="formula">A string representing a mathematical function</param>
		/// <param name="referenceObject">The DataObject that any property references are drawn from</param>
		public static int ParseFormulaInt(string formula, DataObject referenceObject)
		{
			int result = 0;
			try
			{
				var param = System.Linq.Expressions.Expression.Parameter(referenceObject.GetType());
				//var p = referenceObject.GetProperties<DataObject>().Select(d => System.Linq.Expressions.Expression.Parameter(d.GetType(), d.Name)).ToList();
				//p.Add(param);
				List<ParameterExpression> list = new List<ParameterExpression>();
				list.Add(param);

				if (referenceObject is Character)
				{
					list.Add(System.Linq.Expressions.Expression.Parameter(typeof(AttributeList)));
				}

				var exp = System.Linq.Dynamic.DynamicExpression.ParseLambda(list.ToArray(), null, formula);

				var objResult = exp.Compile().DynamicInvoke(referenceObject);

				result =  Convert.ToInt32(objResult);
			}
			catch(Exception e)
			{
				Log.Instance.WriteString(String.Format("Exception caught trying to parse formula: {0}", formula));
				Log.Instance.WriteString(e.StackTrace);

				System.Windows.MessageBox.Show(String.Format("Error trying to calculate value from formula: {0}", formula));
			}

			return result;
		}
	}
}
