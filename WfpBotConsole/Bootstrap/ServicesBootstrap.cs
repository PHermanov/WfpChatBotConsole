using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Reflection;
using WfpBotConsole.Core.Extensions;

namespace WfpBotConsole.Bootstrap
{
	public static class ServicesBootstrap
	{
		public static IServiceCollection AddInjectedServices(this IServiceCollection serviceCollection)
		{
			var execAssembly = Assembly.GetExecutingAssembly();

			var assemblyNames = execAssembly
				.GetReferencedAssemblies()
				.Select(x => x.FullName).ToList();

			assemblyNames.Add(execAssembly.FullName);

			foreach (var assemblyName in assemblyNames)
			{
				var injectedTypes = Assembly
					.Load(assemblyName)
					.GetInjectedTypes();

				foreach (var injectedType in injectedTypes)
				{
					serviceCollection.RegisterType(injectedType.Key, injectedType.Value);
				}
			}

			return serviceCollection;
		}
	}
}
