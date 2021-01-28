using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using WfpBotConsole.Core.Attributes;
using WfpBotConsole.Core.Enums;

namespace WfpBotConsole.Core.Extensions
{
	public static class IServiceCollectionExtensions
	{
		public static IServiceCollection RegisterType(
			this IServiceCollection serviceCollection,
			Type type,
			InjectAttribute injectAttribute)
		{
			switch (injectAttribute.Type)
			{
				case RegistrationType.AsImplementedInterfaces:
					{
						var implementedInterfaces = type.GetDirectlyImplementedInterfacesSet().ToList();

						foreach (var implementedInterface in implementedInterfaces)
						{
							switch (injectAttribute.Scope)
							{
								case RegistrationScope.Singleton:
									{
										serviceCollection.AddSingleton(implementedInterface, type);
										break;
									}
								case RegistrationScope.Transient:
									{
										serviceCollection.AddTransient(implementedInterface, type);
										break;
									}
							}
						}

						break;
					}
				case RegistrationType.AsSelf:
					{
						switch (injectAttribute.Scope)
						{
							case RegistrationScope.Singleton:
								{
									serviceCollection.AddSingleton(type);
									break;
								}
							case RegistrationScope.Transient:
								{
									serviceCollection.AddTransient(type);
									break;
								}
						}

						break;
					}
			}

			return serviceCollection;
		}
	}
}
