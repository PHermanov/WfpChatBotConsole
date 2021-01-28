using System;
using WfpBotConsole.Core.Enums;

namespace WfpBotConsole.Core.Attributes
{
	[AttributeUsage(AttributeTargets.Class, Inherited = false)]
	public class InjectAttribute : Attribute
	{
		public RegistrationScope Scope { get; }

		public RegistrationType Type { get; set; }

		public InjectAttribute(
			RegistrationScope scope = RegistrationScope.Transient,
			RegistrationType type = RegistrationType.AsImplementedInterfaces)
		{
			Scope = scope;
			Type = type;
		}
	}
}
