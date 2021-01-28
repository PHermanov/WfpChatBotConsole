using System;
using System.Collections.Generic;
using System.Linq;

namespace WfpBotConsole.Core.Extensions
{
	public static class TypeExtensions
	{
		public static IEnumerable<Type> GetDirectlyImplementedInterfacesSet(this Type type)
		{
			var allInterfaces = type.GetInterfaces();
			return allInterfaces.Except(allInterfaces.SelectMany(t => t.GetInterfaces()));
		}
	}
}
