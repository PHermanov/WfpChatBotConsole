using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using WfpBotConsole.Core.Attributes;

namespace WfpBotConsole.Core.Extensions
{
	public static class AssemblyExtensions
	{
		public static IEnumerable<KeyValuePair<Type, InjectAttribute>> GetInjectedTypes(this Assembly assembly)
		{
			foreach (var type in assembly.GetTypes())
			{
				var registerModuleAttributes = type.GetCustomAttributes(typeof(InjectAttribute), false);

				if (registerModuleAttributes.Any()
					&& registerModuleAttributes.First() is InjectAttribute attribute)
				{
					yield return new KeyValuePair<Type, InjectAttribute>(type, attribute);
				}
			}
		}
	}
}
