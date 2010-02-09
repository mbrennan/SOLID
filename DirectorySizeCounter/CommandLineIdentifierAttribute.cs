using System;

namespace DirectorySizeCounter
{
	internal class CommandLineIdentifierAttribute : Attribute
	{
		public CommandLineIdentifierAttribute(string commandLineIdentifier, string description)
		{
			CommandLineIdentifier = commandLineIdentifier;
			Description = description;
		}

		public string CommandLineIdentifier
		{
			get;
			private set;
		}

		public string Description
		{
			get;
			private set;
		}
	}
}
